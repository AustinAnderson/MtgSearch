using MtgSearch.Server.Models.Logic.Predicates;
using System.Text.RegularExpressions;

namespace MtgSearch.Server.Models.Logic.Parsing
{
    public class AttributeExpressionParser
    {
        public static readonly Regex AttributeOperatorExpression = new(@"(mv|pow|def|loyalty)\s*(>|<|<=|>=|==)\s*(\d{1,4})");
        public static readonly Regex AttributeIsExpression = new(@"(def|pow) is \*");
        private readonly VariableSubstitutionSet substitutionSet;
        public AttributeExpressionParser(VariableSubstitutionSet substitutionSet)
        {
            this.substitutionSet = substitutionSet;
        }
        public string ParseAndSubstituteStarBased(Match match)
        {
            if(match.Groups.Count != 2)
            {
                throw new ArgumentException(
                    "expected match to have self as group and pow or def as group, " +
                    $"got '{match}' with {match.Groups.Count} groups"
                );
            }
            var pred = new PowerToughnessIsStarPredicate();
            var pt = match.Groups[1].ToString();
            pred.PowerOrToughness = pt switch
            {
                "pow" => PowerOrToughness.Power,
                "def" => PowerOrToughness.Toughness,
                _ => throw new NotImplementedException($"unrecognized '{nameof(PowerOrToughness)}' type '{pt}' at '{match}'")
            };
            return substitutionSet.AddNext(pred).ToString();
        }
        public string ParseAndSubstituteOperatorBased(Match match)
        {
            if(match.Groups.Count != 4)
            {
                throw new ArgumentException(
                    "expected match to have self as group, attribute as group, operator as group, and value as group, "+
                    $"got '{match}' with {match.Groups.Count} groups"
                );
            }
            var pred = new NumericAttributeSearchPredicate();
            var target = match.Groups[1].ToString();
            pred.Type = target switch
            {
                "mv" => NumericCardAttributeType.CMC,
                "pow" => NumericCardAttributeType.Power,
                "def" => NumericCardAttributeType.Toughness,
                "loyalty" => NumericCardAttributeType.Loyalty,
                _ => throw new NotImplementedException($"unrecognized expression type '{target}' at '{match}'")
            };
            var op = match.Groups[2].ToString();
            pred.Operator = op switch
            {
                ">" => Operator.Gt,
                "<" => Operator.Lt,
                ">=" => Operator.Gte,
                "<=" => Operator.Lte,
                "==" => Operator.Equals,
                _ => throw new NotImplementedException($"unrecognized operator in expression '{op}' at '{match}'")
            };
            var value = match.Groups[3].ToString();
            if(!int.TryParse(value, out int res))
            {
                throw new FormatException($"invalid int '{value}' at '{match}'");
            }
            pred.Value = res;
            return substitutionSet.AddNext(pred).ToString();
        }

    }
}
