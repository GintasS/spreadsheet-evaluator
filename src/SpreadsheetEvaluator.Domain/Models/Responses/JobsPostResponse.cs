using Newtonsoft.Json;

namespace SpreadsheetEvaluator.Domain.Models.Responses
{
    public class JobsPostResponse
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }
    }
}