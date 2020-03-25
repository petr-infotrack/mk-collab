using AlertsAdmin.Domain.Models;
using AlertsAdmin.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AlertsAdmin.Helpers
{
    public class APIHelper : IAPIHelper
    {
        private readonly string _apiHost;
        private readonly ILogger<APIHelper> _logger;
        private static HttpClient _httpClient;
        public APIHelper(IConfiguration config, ILogger<APIHelper> logger)
        {
            _apiHost = config.GetValue<string>("AlertsAdminAPIHost") ?? "http://localhost:51136";
            _logger = logger;
            _httpClient = new HttpClient();
        }

        public async Task<Alert> GetAlertAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{_apiHost}/api/v1/alerts/view/{id}");
            var responseContent = await response.Content.ReadAsStringAsync();
            Alert alert = null;
            if (!response.IsSuccessStatusCode)
                _logger.LogError($"Unable to load alert data from api, response code {response.StatusCode}.\nAddress: {_apiHost}/api/v1/alerts/view/{id}\nResponse:{responseContent}");
            else
            {
                try
                {
                    alert = JsonConvert.DeserializeObject<Alert>(responseContent);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Unable to deserialize response object to Alert\nException: {ex.Message}\nResponse: {responseContent}");
                }
            }
            return alert;
        }

        public async Task<MessageType> GetMessageAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{_apiHost}/api/v1/messages/{id}");
            var responseContent = await response.Content.ReadAsStringAsync();
            MessageType message = null;
            if (!response.IsSuccessStatusCode)
                _logger.LogError($"Unable to load message data from api, response code {response.StatusCode}.\nAddress: {_apiHost}/api/v1/alerts/view/{id}\nResponse:{responseContent}");
            else
            {
                try
                {
                    message = JsonConvert.DeserializeObject<MessageType>(responseContent);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Unable to deserialize response object to Message\nException: {ex.Message}\nResponse: {responseContent}");
                }
            }
            return message;
        }

    }
}
