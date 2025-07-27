using MtgSearch.Server.Models.Logic.Predicates;
using System.Text.RegularExpressions;

namespace MtgSearch.Server.Models.Logic.Parsing
{
    public class AttributeExpressionParser
    {
        private static readonly string __types = string.Join("|", NumericCardAttributeType.ByString.Keys);
        private static readonly string __operators= string.Join("|", Operator.ByString.Keys);
        public static readonly Regex AttributeOperatorExpression = new(@"("+__types+@")\s*("+__operators+@")\s*(\d{1,4})");
        private static readonly string __pt = string.Join("|", PowerOrToughness.ByString.Keys);
        public static readonly Regex AttributeIsExpression = new(@"("+__pt+@") is ?\*");
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
                    $"got `{match}` with {match.Groups.Count} groups"
                );
            }
            var pred = new PowerToughnessIsStarPredicate();
            var pt = match.Groups[1].ToString();
            if(!PowerOrToughness.ByString.TryGetValue(pt, out var powerOrToughness))
            {
                throw new NotImplementedException($"unrecognized `{nameof(PowerOrToughness)}` type `{pt}` at `{match}`");
            }
            pred.PowerOrToughness = powerOrToughness;
            return substitutionSet.AddNext(pred).ToString();
        }
        public string ParseAndSubstituteOperatorBased(Match match)
        {
            if(match.Groups.Count != 4)
            {
                throw new ArgumentException(
                    "expected match to have self as group, attribute as group, operator as group, and value as group, "+
                    $"got `{match}` with {match.Groups.Count} groups"
                );
            }
            var pred = new NumericAttributeSearchPredicate();
            var target = match.Groups[1].ToString();
            if(!NumericCardAttributeType.ByString.TryGetValue(target, out var type))
            {
                throw new NotImplementedException($"unrecognized expression type `{target}` at `{match}`");
            }
            pred.Type = type;
            var op = match.Groups[2].ToString();
            if(!Operator.ByString.TryGetValue(op, out var oper))
            {
                throw new NotImplementedException($"unrecognized operator in expression `{op}` at `{match}`");
            }
            pred.Operator = oper;
            var value = match.Groups[3].ToString();
            if(!int.TryParse(value, out int res))
            {
                throw new FormatException($"invalid int `{value}` at `{match}`");
            }
            pred.Value = res;
            return substitutionSet.AddNext(pred).ToString();
        }

    }
}
