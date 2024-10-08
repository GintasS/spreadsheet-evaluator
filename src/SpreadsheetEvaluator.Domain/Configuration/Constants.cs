using SpreadsheetEvaluator.Domain.Models.Enums;
using System.Collections.Generic;
using SpreadsheetEvaluator.Domain.Models.MathModels;

namespace SpreadsheetEvaluator.Domain.Configuration
{
    public class Constants
    {
        public static readonly List<FormulaOperator> FormulaOperators = new List<FormulaOperator> 
        {
            new FormulaOperator
            {
                JsonName = "sum",
                MathSymbol = "+",
                FormulaResultType = FormulaResultType.Number
            },
            new FormulaOperator
            {
                JsonName = "multiply",
                MathSymbol = "*",
                FormulaResultType = FormulaResultType.Number
            },
            new FormulaOperator
            {
                JsonName = "divide",
                MathSymbol = "/",
                FormulaResultType = FormulaResultType.Number
            },
            new FormulaOperator
            {
                JsonName = "is_greater",
                MathSymbol = ">",
                FormulaResultType = FormulaResultType.Boolean
            },
            new FormulaOperator
            {
                JsonName = "is_equal",
                MathSymbol = "=",
                FormulaResultType = FormulaResultType.Boolean
            },
            new FormulaOperator
            {
                JsonName = "not",
                MathSymbol = "NOT",
                FormulaResultType = FormulaResultType.Boolean
            },
            new FormulaOperator
            {
                JsonName = "and",
                MathSymbol = " AND ",
                FormulaResultType = FormulaResultType.Boolean
            },
            new FormulaOperator
            {
                JsonName = "or",
                MathSymbol = "OR",
                FormulaResultType = FormulaResultType.Boolean
            },
            new FormulaOperator
            {
                JsonName = "if",
                MathSymbol = "IIF",
                FormulaResultType = FormulaResultType.Undefined
            },
            new FormulaOperator
            {
                JsonName = "concat",
                MathSymbol = null,
                FormulaResultType = FormulaResultType.Text
            },
            new FormulaOperator
            {
                JsonName = "reference",
                MathSymbol = null,
                FormulaResultType = FormulaResultType.Reference
            }
        };

        public static readonly List<string> StandardOperatorNames = new List<string>
        {
            "sum",
            "multiply",
            "divide"
        };

        public static readonly List<string> LogicalOperatorNames = new List<string>
        {
            "is_greater",
            "is_equal",
            "and",
            "or",
            "not"
        };

        public struct Error
        {
            public const string MismatchingTypes = "type does not match";
        }

        public struct HubApi
        {
            public const string PostMediaType = "application/json";
        }

        public struct ExitMessages
        {
            public const string FailedToGetJobs = "Failed to GET jobs from the Hub Api. Message: {0}.";
            public const string FailedToPostJobs = "Failed to POST jobs to the Hub Api. Message: {0}, Server Response: {1}.";

        }

        public struct UserMessages
        {
            public const string OutputFileCreatedSuccessfully = "Output file {0} was created successfully.";
            public const string OutputFileCreationFailure = "We failed to create the output file {0}, because: {1}";
            public const string FailedToParseArgs = "We failed to parse arguments.";
            public const string FailedToReadInputFile = "We failed to read from the nput file {0}, because: {1}";
            public const string InputFileReadSuccessfully = "We read input file {0} successfully. ";
        }
    }
}