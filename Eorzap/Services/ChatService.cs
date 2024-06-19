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
using Eorzap.Types;
using System.Text.RegularExpressions;
using OtterGui.Log;
using Dalamud.Game.ClientState.Party;

namespace Eorzap.Services
{
    public class ChatService : IDisposable
    {
        private readonly IChatGui _chat;
        private readonly IPartyList _party;
        private readonly Logger _log;
        private Configuration _config;

        public ChatService(Configuration config, IChatGui chat, IPartyList party, Logger log)
        {
            _config = config;
            _chat = chat;
            _party = party;
            _log = log;
            _chat.ChatMessage += OnChatMessage;

        }

        public void Dispose()
        {
            _chat.ChatMessage -= OnChatMessage;
        }

        private void OnChatMessage(XivChatType type, uint senderId, ref SeString sender, ref SeString message, ref bool isHandled)
        {
            ChatType.ChatTypes? chatType = ChatType.GetChatTypeFromXivChatType(type);
            _log.Information(chatType == null ? "null" : chatType.ToString());
            _log.Information(message.TextValue);
            if (chatType == null)
            {
                return;
            }
            if (_config.Channels.Contains(chatType.Value)) //If the channel can be selected and is activated by the user
            {
                List<Trigger> triggers = _config.Triggers;
                foreach (Trigger trigger in triggers)
                {
                    if (trigger.Enabled && (trigger.Regex != null && trigger.Regex.IsMatch(message.TextValue)))
                    {
                        _log.Information($"Trigger {trigger.Name} triggered. Zap!");
                        _ = PostPishockApi(sender.ToString(), trigger.Intensity, trigger.Duration);
                    }
                }
            }

            if(Array.IndexOf(DeathMode.deathTypes, chatType) > -1 && _config.DeathMode)
            {
                HandleDeathMode(message.TextValue);
            }
        }

        private void HandleDeathMode(string message)
        {
            if (DeathMode.DeathModeDieOtherRegex.IsMatch(message))
            {
                foreach (PartyMember member in _party)
                {
                    _log.Information(message);
                    _log.Information(member.Name.TextValue);
                    if (message.Contains(member.Name.TextValue))
                    {
                        _config.DeathModeCount++;
                        int partysize = _party.Count;
                        int duration = 15 * _config.DeathModeCount / partysize;
                        int intensity = 100 * _config.DeathModeCount / partysize;
                        _log.Information($"Duration: {duration}, Intensity: {intensity}.");
                        _config.Save();
                        _ = PostPishockApi("Death!", intensity, duration);
                        return;
                    }
                }
            }
            if (DeathMode.DeathModeDieSelfRegex.IsMatch(message))
            {
                _config.DeathModeCount++;
                int partysize = _party.Count;
                int duration = 15 * _config.DeathModeCount / partysize;
                int intensity = 100 * _config.DeathModeCount / partysize;
                _log.Information($"Duration: {duration}, Intensity: {intensity}.");
                _config.Save();
                _ = PostPishockApi("Death!", intensity, duration);
                return;
            }
            if (DeathMode.DeathModeLiveOtherRegex.IsMatch(message))
            {
                foreach (PartyMember member in _party)
                {
                    _log.Information(member.Name.TextValue);
                    if (message.Contains(member.Name.TextValue))
                    {
                        _config.DeathModeCount--;
                        _config.Save();
                        return;
                    }
                }
            }
            if (DeathMode.DeathModeLiveSelfRegex.IsMatch(message))
            {
                _config.DeathModeCount--;
                _config.Save();
                return;
            }
        }

        private async Task<bool> PostPishockApi(string sender, int intensity, int duration)
        {
            string jsonContent = $"{{ \"Username\": \"{_config.ShockUsername}\", \"Name\": \"{sender}\",\"Code\":\"{_config.ShockerCode}\",\"Intensity\": {intensity},\"Duration\": {duration},\"ApiKey\":\"{_config.ApiKey}\",\"Op\":0 }}";
            return await PostJsonData(jsonContent);
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
