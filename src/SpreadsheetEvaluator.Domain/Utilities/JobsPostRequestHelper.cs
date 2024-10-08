using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SpreadsheetEvaluator.Domain.Configuration;
using SpreadsheetEvaluator.Domain.Models.MathModels;
using SpreadsheetEvaluator.Domain.Models.Requests;
using System.Collections.Generic;
using System.Linq;

namespace SpreadsheetEvaluator.Domain.Utilities
{
    public class JobsPostRequestHelper
    {
        private readonly ApplicationSettings _applicationSettings;

        public JobsPostRequestHelper(IOptionsMonitor<ApplicationSettings> configuration)
        {
            _applicationSettings = configuration.CurrentValue;
        }

        public JobsPostRequest CreatePostRequest(List<JobComputed> jobsComputedList)
        {
            var jobsPostRequest = new JobsPostRequest
            {
                Email = _applicationSettings.DeveloperEmailAddress,
                Jobs = new List<JobsPostModel>()
            };

            var jobs = jobsComputedList.Select(x => new {
                x.Id,
                x.Cells
            }).ToList();

            foreach (var job in jobs)
            {
                var jobsPostModel = new JobsPostModel
                {
                    Id = job.Id,
                    Values = new List<List<JobsPostValueModel>>()
                };

                foreach (var cellRow in job.Cells)
                {
                    var newValueCellRow = new List<JobsPostValueModel>();
                    jobsPostModel.Values.Add(newValueCellRow);

                    foreach (var individualCell in cellRow)
                    {
                        newValueCellRow.Add(new JobsPostValueModel(individualCell.Value));
                    }
                }

                jobsPostRequest.Jobs.Add(jobsPostModel);
            }

            return jobsPostRequest;
        }

        public string SerializePostRequest(JobsPostRequest jobsPostRequest)
        {
            return JsonConvert.SerializeObject(jobsPostRequest, Formatting.Indented,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                }
            );
        }
    }
}
