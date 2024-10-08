using Newtonsoft.Json;
using System.Collections.Generic;
using SpreadsheetEvaluator.Domain.Models.Enums;
using SpreadsheetEvaluator.Domain.Models.MathModels;

namespace SpreadsheetEvaluator.Domain.Models.Requests
{
    public class JobsPostRequest
    {
        [JsonProperty("email")]
        [JsonRequired]
        public string Email { get; set; }

        [JsonProperty("results")]
        [JsonRequired]
        public List<JobsPostModel> Jobs { get; set; }
    }

    public class JobsPostModel
    {
        [JsonProperty("id")]
        [JsonRequired]
        public string Id { get; set; }

        [JsonProperty("data")]
        [JsonRequired]
        public List<List<JobsPostValueModel>> Values { get; set; }
    }

    public class JobsPostValueModel
    {
        [JsonProperty("value")]
        public Values Values { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }

        public JobsPostValueModel(CellValue cellValue)
        {
            Values = new Values();

            switch (cellValue.CellType)
            {
                case CellType.Number:
                    Values.Number = (decimal)cellValue.Value;
                    break;
                case CellType.Text:
                    Values.Text = cellValue.Value.ToString();
                    break;
                case CellType.Boolean:
                    Values.Boolean = (bool)cellValue.Value;
                    break;
                case CellType.Error:
                    Error = cellValue.Value.ToString();
                    Values = null;
                    break;
            }
        }
    }

    public class Values
    {
        [JsonProperty("boolean")]
        public bool? Boolean { get; set; }
        [JsonProperty("number")]
        public decimal? Number { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
    }
}
