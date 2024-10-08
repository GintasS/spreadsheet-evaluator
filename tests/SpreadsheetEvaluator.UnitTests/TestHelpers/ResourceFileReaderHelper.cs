using System.IO;
using System.Reflection;

namespace SpreadsheetEvaluator.UnitTests.TestHelpers
{
    public static class ResourceFileReaderHelper
    {
        public static string ReadFile(string path)
        {
            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
            using var streamReader = new StreamReader(stream);
            return streamReader.ReadToEnd();
        }
    }
}
