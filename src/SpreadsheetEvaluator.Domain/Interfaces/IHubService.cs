using System.Net.Http;

namespace SpreadsheetEvaluator.Domain.Interfaces
{
    public interface IHubService
    {
        public HttpResponseMessage GetJobs();
        public HttpResponseMessage PostJobs(string submissionUrl, string payload);
    }
}
