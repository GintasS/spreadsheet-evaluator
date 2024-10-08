using Newtonsoft.Json;
using SpreadsheetEvaluator.Domain.Configuration;
using SpreadsheetEvaluator.Domain.Interfaces;
using SpreadsheetEvaluator.Domain.Models.Requests;
using SpreadsheetEvaluator.Domain.Models.Responses;
using SpreadsheetEvaluator.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using System.Threading.Tasks;

namespace SpreadsheetEvaluator.Domain.Utilities
{
    public class HttpHelper
    {
        private IHubService _hubService;
        public HttpHelper(IHubService hubService) 
        {
            _hubService = hubService;
        }

        public string SendHttpPostJobsRequest(string url, string payload)
        {
            JobsPostResponse jobsPostResponse = null;
            try
            {
                var postJobsHttpResponse = _hubService.PostJobs(url, payload);
                var responseText = postJobsHttpResponse.Content.ReadAsStringAsync().Result;
                jobsPostResponse = JsonConvert.DeserializeObject<JobsPostResponse>(responseText);
            }
            catch (Exception ex)
            {
                Console.WriteLine(Constants.ExitMessages.FailedToPostJobs, ex.Message, jobsPostResponse?.Error);
                return null;
            }
            return jobsPostResponse.Message;
        }
    }
}
