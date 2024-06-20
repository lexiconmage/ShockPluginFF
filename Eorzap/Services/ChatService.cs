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
        private int DeathModeCount = 0;

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
                HandleDeathMode(chatType.Value, message.TextValue);
            }
        }

        private void HandleDeathMode(ChatType.ChatTypes type, string message)
        {
            int partysize, intensity, duration;
            switch (type)
            {
                case ChatType.ChatTypes.DeathOther:
                    foreach (PartyMember member in _party)
                    {
                        _log.Information(message);
                        _log.Information(member.Name.TextValue);
                        if (message.Contains(member.Name.TextValue))
                        {
                            DeathModeCount++;
                            partysize = _party.Count;
                            duration = _config.DeathModeSettings[2] * DeathModeCount / partysize;
                            intensity = _config.DeathModeSettings[1] * DeathModeCount / partysize;
                            _log.Information($"Duration: {duration}, Intensity: {intensity}.");
                            _ = PostPishockApi("Death!", intensity, duration);
                            return;
                        }
                    }
                    break;
                case ChatType.ChatTypes.DeathSelf:
                case ChatType.ChatTypes.DeathSelf2:
                    DeathModeCount++;
                    partysize = _party.Count;
                    duration = _config.DeathModeSettings[2] * DeathModeCount / partysize;
                    intensity = _config.DeathModeSettings[1] * DeathModeCount / partysize;
                    _log.Information($"Duration: {duration}, Intensity: {intensity}.");
                    _ = PostPishockApi("Death!", intensity, duration);
                    return;

                case ChatType.ChatTypes.ReviveOther:
                    foreach (PartyMember member in _party)
                    {
                        _log.Information(member.Name.TextValue);
                        if (message.Contains(member.Name.TextValue))
                        {
                            DeathModeCount--;
                            return;
                        }
                    }
                    break;
                case ChatType.ChatTypes.ReviveSelf:
                    DeathModeCount--;
                    return;
                default:
                    break;
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
