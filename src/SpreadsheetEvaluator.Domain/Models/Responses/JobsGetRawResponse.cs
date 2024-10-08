using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace SpreadsheetEvaluator.Domain.Models.Responses
{
    public class JobsGetRawResponse
    {
        public string SubmissionUrl { get; set; }
        [JsonExtensionData]
        public Dictionary<string, JToken> Jobs { get; set; }
    }
}