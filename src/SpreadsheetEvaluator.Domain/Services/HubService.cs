using System.Net.Http;
using System.Text;
using Microsoft.Extensions.Options;
using SpreadsheetEvaluator.Domain.Configuration;
using SpreadsheetEvaluator.Domain.Interfaces;

namespace SpreadsheetEvaluator.Domain.Services
{
    public class HubService : IHubService
    {
        private readonly ApplicationSettings _applicationSettings;
        private readonly HttpClient _httpClient;

        public HubService(IOptionsMonitor<ApplicationSettings> configuration, HttpClient httpClient)
        {
            _applicationSettings = configuration.CurrentValue;
            _httpClient = httpClient;
        }

        public HttpResponseMessage GetJobs()
        {
            var httpResponse = _httpClient.GetAsync(_applicationSettings.HubApiUrlGetJobs).Result;
            httpResponse.EnsureSuccessStatusCode();

            return httpResponse;
        }

        public HttpResponseMessage PostJobs(string submissionUrl, string payload)
        {
            var content = new StringContent(payload, Encoding.UTF8, Constants.HubApi.PostMediaType);
            var httpResponse = _httpClient.PostAsync(submissionUrl, content).Result;
            httpResponse.EnsureSuccessStatusCode();

            return httpResponse;
        }
    }
}
