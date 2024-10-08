using System.Linq;
using Newtonsoft.Json.Linq;
using SpreadsheetEvaluator.Domain.Configuration;
using SpreadsheetEvaluator.Domain.Models.MathModels;

namespace SpreadsheetEvaluator.Domain.Utilities
{
    public static class SpreadsheetCreationHelper
    {

        public static void IterateThroughMultiDimensionalArray(JToken token, JobRaw jobRaw)
        {
            if (token is JArray && token?.Parent is JArray && token.Parent.Count > 1)
            {
                if (jobRaw.IsMultiArray == false && token.Parent.Count > 1)
                {
                    jobRaw.IsMultiArray = true;
                }
                else if (jobRaw.IsMultiArray)
                {
                    jobRaw.ResetLetterIndex();
                    jobRaw.IncrementCellIndex();
                }
            }
        }

        public static void CheckForPrimitiveValue(JToken token, JobRaw jobRaw)
        {
            if (token is JProperty && token?.Parent is JObject firstValuePropertyParent && firstValuePropertyParent.ContainsKey("value"))
            {
                jobRaw.FoundValue = true;
            }
            else if (token is JProperty valueProperty && jobRaw.FoundValue)
            {
                // Get simple values here.
                if (SpreadsheetCreationHelper.IsInFormulaObject(valueProperty, jobRaw) is false)
                {
                    var concreteValue = JsonObjectHelper.GetConcreteValueFromProperty(valueProperty);
                    if (concreteValue != null)
                    {
                        jobRaw.SetCellValue(concreteValue);
                        jobRaw.FoundValue = false;
                    }
                }
            }
        }

        public static void CheckForFormula(JToken token, JobRaw jobRaw)
        {
            // In this place, we say that we have found formula object.
            if (token is JObject formulaObject && formulaObject.ContainsKey("formula") && jobRaw.IsInFormulaObject == false)
            {
                jobRaw.IsInFormulaObject = true;
            }
        }

        public static void CheckForIfStatementInFormula(JToken token, JobRaw jobRaw)
        {
            if (token is JProperty formulaProperty && jobRaw.IsInFormulaObject)
            {
                if (formulaProperty.Name == "if" && jobRaw.FoundIfFormula == false)
                {
                    jobRaw.FoundIfFormula = true;
                }
            }
        }

        public static bool IsInFormulaObject(JProperty valueProperty, JobRaw jobRaw)
        {
            return valueProperty.AncestorsAndSelf()
                .OfType<JObject>()
                .Any(n => n.ContainsKey("formula"));
        }

        public static void GetFormulasFromObject(JToken token, JobRaw jobRaw)
        {
            if (token is JArray ifObjectArray && ifObjectArray.Count == 3 && jobRaw.FoundIfFormula)
            {
                var ifFormula = SpreadsheetCreationHelper.CreateIfFormula(ifObjectArray);
                jobRaw.SetCellValue(ifFormula);
            }
            else if (token is JProperty concatFormulaProperty && concatFormulaProperty.Name == "concat" && jobRaw.FoundIfFormula == false)
            {
                var concatFormula = SpreadsheetCreationHelper.CreateStandardFormula(concatFormulaProperty);
                jobRaw.SetCellValue(concatFormula);
            }
            else if (token is JProperty standardFormulaProperty && Constants.StandardOperatorNames.Exists(x => x.Equals(standardFormulaProperty.Name)))
            {
                var standardFormula = SpreadsheetCreationHelper.CreateStandardFormula(standardFormulaProperty);
                jobRaw.SetCellValue(standardFormula);
            }
            else if (token is JProperty notFormulaProperty && notFormulaProperty.Name == "not" && jobRaw.FoundIfFormula == false)
            {
                var notFormula = SpreadsheetCreationHelper.CreateNotOperatorFormula(notFormulaProperty);
                jobRaw.SetCellValue(notFormula);
            }
            else if (token is JProperty logicalFormulaProperty && Constants.LogicalOperatorNames.Exists(x => x.Equals(logicalFormulaProperty.Name)) && jobRaw.FoundIfFormula == false)
            {
                var standardFormula = SpreadsheetCreationHelper.CreateStandardFormula(logicalFormulaProperty);
                jobRaw.SetCellValue(standardFormula);
            }
            else if (token is JObject formulaReferenceTypeObject && formulaReferenceTypeObject.ContainsKey("formula") && formulaReferenceTypeObject["formula"]["reference"] != null)
            {
                var referenceFormula = SpreadsheetCreationHelper.CreateReferenceFormula(formulaReferenceTypeObject);
                jobRaw.SetCellValue(referenceFormula);
            }
        }

        public static Formula CreateIfFormula(JArray ifObjectArray)
        {
            if (ifObjectArray.Count != 3)
            {
                return null;
            }

            var firstIfObject = ifObjectArray[0] as JObject;
            var secondIfObject = ifObjectArray[1] as JObject;
            var thirdIfObject = ifObjectArray[2] as JObject;

            if (firstIfObject == null || secondIfObject == null || thirdIfObject == null)
            {
                return null;
            }

            var formulaOperator = Constants.FormulaOperators.SingleOrDefault(x => firstIfObject.ContainsKey(x.JsonName));

            if (formulaOperator == null)
            {
                return null;
            }

            var elements = firstIfObject[formulaOperator.JsonName] as JArray;

            if (elements == null || secondIfObject["reference"] == null || thirdIfObject["reference"] == null)
            {
                return null;
            }

            var expr = $"IIF({TryParseFormulaReferences(elements, formulaOperator)},{secondIfObject["reference"]},{thirdIfObject["reference"]})";

            return new Formula(expr, formulaOperator);
        }

        public static Formula CreateReferenceFormula(JObject formulaReferenceTypeObject)
        {
            if (formulaReferenceTypeObject.ContainsKey("formula") == false ||
                formulaReferenceTypeObject["formula"]["reference"] == null)
            {
                return null;
            }

            var expr = formulaReferenceTypeObject["formula"]["reference"].ToString();

            return new Formula(expr, Constants.FormulaOperators[10]);
        }
        
        public static Formula CreateNotOperatorFormula(JProperty formulaProperty)
        {
            var expr = Constants.FormulaOperators[5].MathSymbol + " " +
                       JsonObjectHelper.GetValueOfAnyType(formulaProperty);

            return new Formula(expr, Constants.FormulaOperators[5]);
        }

        public static Formula CreateStandardFormula(JProperty formulaProperty)
        {
            var formulaOperator = Constants.FormulaOperators
                .SingleOrDefault(x => formulaProperty.Name.Equals(x.JsonName));

            if (formulaOperator == null)
            {
                return null;
            }

            var expr = TryParseFormulaReferences(formulaProperty.Value as JArray, formulaOperator);

            return new Formula(expr, formulaOperator);
        }

        private static string TryParseFormulaReferences(JArray elements, FormulaOperator formulaOperator)
        {
            var expr = string.Empty;
            for (var i = 0; i < elements.Count; i++)
            {
                if (elements[i]["reference"] != null)
                {
                    expr += elements[i]["reference"];
                }
                else if (elements[i]["value"] != null)
                {
                    var jsonObject = elements[i]["value"] as JObject;
                    expr += JsonObjectHelper.GetValueOfAnyType(jsonObject);
                }

                if (i + 1 < elements.Count)
                {
                    if (formulaOperator != null && string.IsNullOrEmpty(formulaOperator.MathSymbol) == false)
                    {
                        expr += " " + formulaOperator.MathSymbol + " ";
                    }
                }
            }

            return expr;
        }
    }
}