using System.Data;
using SpreadsheetEvaluator.Domain.Models.MathModels;

namespace SpreadsheetEvaluator.Domain.Extensions
{
    public static class ObjectExtensions
    {
        public static bool IsNumber(this object value)
        {
            return value is sbyte
                    || value is byte
                    || value is short
                    || value is ushort
                    || value is int
                    || value is uint
                    || value is long
                    || value is ulong
                    || value is float
                    || value is double
                    || value is decimal;
        }

        public static bool IsValidCellValue(this object value)
        {
            return value.IsNumber()
                   || value is string
                   || value is bool
                   || value is Formula
                   || value is CellValue;
        }

        public static object CalculateMathExpression(this object valueToCompute)
        {
            object returnValue;
            try
            {
                returnValue = new DataTable().Compute(valueToCompute.ToString(), null);
            }
            catch
            {
                returnValue = null;
            }

            return returnValue;
        }
    }
}