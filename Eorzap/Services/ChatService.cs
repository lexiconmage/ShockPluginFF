using Dalamud.Game.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Dalamud.Plugin;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Plugin.Services;

namespace Eorzap.Services
{
    public class ChatService : IDisposable
    {
        private readonly IChatGui _chat;
        private Configuration _config;

        public ChatService(Configuration config, IChatGui chat)
        {
            _config = config;
            _chat = chat;

            _chat.ChatMessage += OnChatMessage;

        }

        public void Dispose()
        {
            _chat.ChatMessage -= OnChatMessage;
        }

        private void OnChatMessage(XivChatType type, uint senderId, ref SeString sender, ref SeString message, ref bool isHandled)
        {
            // for now only check a message if in a FC
            // Need to add a config file to check which chat to check

            // Also need to check for the trigger word that is setup + Do the actual shock with
            // the post request
            _config.InfoChannel();
            string[] Channels = _config.ChatName; //All the name of the channels that can be listened 
            int index = Array.IndexOf(Channels, type.ToString());
            if (index >= 0 && _config.ChannelBool[index] == true) //If the channel can be selected and is activated by the user
            {
                string keyword = _config.mainKeyWord.ToLower(); //Get the keyword
                string[] triggers = _config.triggerWords;
                string[] triggersModifier = new string[triggers.Length]; //That way it does not modify the triggers value
                for (int i = 0; i < triggers.Length; i++)
                {
                    triggersModifier[i] = keyword + " " + triggers[i].ToLower(); //Put every keyword with the triggers like "testing trigger1"
                }
                int indexTrigger = Array.IndexOf(triggersModifier, message.ToString().ToLower());
                if (indexTrigger >= 0 && triggersModifier[indexTrigger].Split(" ").Length > 1) //If the message correspond to an actual keyword + trigger combination
                {
                    //Get list of itensity and duration
                    int[] intensity = _config.intensityArray;
                    int[] duration = _config.durationArray;
                    //Actual shock part
                    // Call the shock with the duration and intensity for the keyword + trigger combination
                    string jsonContent = $"{{ \"Username\": \"{_config.ShockUsername}\", \"Name\": \"{sender.ToString()}\",\"Code\":\"{_config.ShockerCode}\",\"Intensity\": {intensity[indexTrigger]},\"Duration\": {duration[indexTrigger]},\"ApiKey\":\"{_config.ApiKey}\",\"Op\":0 }}";
                    _ = PostJsonData(jsonContent);
                }
            }
        }

        public async Task<bool> PostJsonData(string jsonContent)
        {
            using (var client = new HttpClient())
            {
                // Configure the base address of the API
                client.BaseAddress = new Uri("https://do.pishock.com/api/apioperate/");

                // Set the content type to JSON
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Create a StringContent with the JSON data
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Send the POST request and get the response
                var response = await client.PostAsync("https://do.pishock.com/api/apioperate/", content);

                if (response.IsSuccessStatusCode)
                {
                    // Read and return the response content as a string
                    _config.resultApiCall = await response.Content.ReadAsStringAsync();
                    return true;
                }
                else
                {
                    // Handle the error or throw an exception
                    throw new HttpRequestException($"HTTP request failed with status code {response.StatusCode}");
                }
            }
        }
    }
}
