using SpreadsheetEvaluator.Domain.Configuration;
using SpreadsheetEvaluator.Domain.Extensions;
using SpreadsheetEvaluator.Domain.Models.Enums;

namespace SpreadsheetEvaluator.Domain.Models.MathModels
{
    public class Cell
    {
        public string Key { get; set; }
        public CellValue Value { get; set; }
    }

    public class CellValue
    {
        public object Value { get; private set; }
        public CellType CellType { get; private set; } = CellType.Undefined;
        public bool IsFormulaCell => Value is Formula;
        public bool IsErrorCell => CellType is CellType.Error;

        public CellValue(object value)
        {
            UpdateCell((dynamic)value);
        }

        public void UpdateCell<T>(T value) where T : struct
        {
            if (value.IsNumber() == false)
            {
                return;
            }

            decimal.TryParse(value.ToString(), out var decimalResult);
            Value = decimalResult;
            CellType = CellType.Number;
        }

        public void UpdateCell(string value)
        {
            Value = new string(value);
            CellType = CellType.Text;
        }

        public void UpdateCell(bool value)
        {
            Value = value;
            CellType = CellType.Boolean;
        }

        public void UpdateCell(Formula value)
        {
            Value = new Formula(value);
            CellType = CellType.Formula;
        }

        public void UpdateCell(CellValue cellValue)
        {
            if (cellValue.IsValidCellValue() == false)
            {
                return;
            }

            if (cellValue.Value is string || cellValue.Value is Formula)
            {
                UpdateCell((dynamic)cellValue.Value);
                return;
            }

            Value = cellValue.Value;
            CellType = cellValue.CellType;
        }

        public void SetCellAsErrorCell()
        {
            CellType = CellType.Error;
            Value = Constants.Error.MismatchingTypes;
        }
    }
}
