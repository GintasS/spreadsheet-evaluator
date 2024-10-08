using Newtonsoft.Json;
using SpreadsheetEvaluator.Domain.Configuration;
using SpreadsheetEvaluator.Domain.Models.Responses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetEvaluator.Domain.Utilities
{
    public class FileHelper
    {
        public static JobsGetRawResponse ReadSpreadsheetFromJsonAsClass(string path)
        {
            string readContents;
            using (StreamReader streamReader = new StreamReader(path, Encoding.UTF8))
            {
                readContents = streamReader.ReadToEnd();
            }
            return JsonConvert.DeserializeObject<JobsGetRawResponse>(readContents);
        }

        public static void SaveJsonSpreadsheetToFile(string path, string contents)
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.Write(contents);
            }
        }
    }
}
