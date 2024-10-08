using Newtonsoft.Json.Linq;
using SpreadsheetEvaluator.Domain.Configuration;
using SpreadsheetEvaluator.Domain.Interfaces;
using SpreadsheetEvaluator.Domain.Models.MathModels;
using SpreadsheetEvaluator.Domain.Models.Responses;
using SpreadsheetEvaluator.Domain.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace SpreadsheetEvaluator.Domain.Services
{
    public class SpreadsheetCreationService : ISpreadsheetCreationService
    {
        public List<JobRaw> Create(JobsGetRawResponse jobsGetRawResponse)
        {
            var createdJobs = new List<JobRaw>();
            var jobNodes = jobsGetRawResponse.Jobs.Values.Children();

            foreach (var jobNode in jobNodes)
            {
                var createdSingleJob = ReadTokensRecursively(jobNode, new JobRaw());
                createdJobs.Add(createdSingleJob);
            }

            return createdJobs;
        }

        private JobRaw ReadTokensRecursively(IEnumerable<JToken> tokens, JobRaw jobRaw)
        {
            foreach (var token in tokens)
            {
                // We are at the 1 below root, aka, on on the individual job.

                // Get Job Id here or check if data array has started.
                if (CheckForJobId(token, jobRaw) is false)
                {
                    CheckForDataArrayStart(token, jobRaw);
                }

                // In this block we are going both save values and formulas.
                if (jobRaw.IteratingOverDataArray)
                {
                    IterateOverDataArray(token, jobRaw);
                }

                if (token.Children().Count() != 0)
                {
                    ReadTokensRecursively(token.Children(), jobRaw);
                }
                else
                {
                    ResetForNewJob(jobRaw);
                }
            }        
            return jobRaw;
        }

        private static void ResetForNewJob(JobRaw jobRaw)
        {
            jobRaw.FoundIfFormula = false;
            jobRaw.IsInFormulaObject = false;
        }

        private bool CheckForJobId(JToken token, JobRaw jobRaw)
        {
            if (token is JProperty jobIdProperty && jobIdProperty.Name == "id")
            {
                jobRaw.Id = jobIdProperty.Value.ToString();
                return true;
            }
            return false;
        }

        private void CheckForDataArrayStart(JToken token, JobRaw jobRaw)
        {
            if (token?.Parent != null && token?.Parent is JProperty arrayProperty && arrayProperty.Name == "data" && jobRaw.IteratingOverDataArray == false)
            {
                // In this place, we say that we have found our data array.
                jobRaw.IteratingOverDataArray = true;
            }
        }

        private void IterateOverDataArray(JToken token, JobRaw jobRaw)
        {
            // Detect multidimensional array.
            SpreadsheetCreationHelper.IterateThroughMultiDimensionalArray(token, jobRaw);

            SpreadsheetCreationHelper.CheckForFormula(token, jobRaw);

            SpreadsheetCreationHelper.CheckForPrimitiveValue(token, jobRaw);
            SpreadsheetCreationHelper.CheckForIfStatementInFormula(token, jobRaw);

            // Get formulas here.
            if (jobRaw.IsInFormulaObject)
            {
                SpreadsheetCreationHelper.GetFormulasFromObject(token, jobRaw);
            }
        }
    }
}