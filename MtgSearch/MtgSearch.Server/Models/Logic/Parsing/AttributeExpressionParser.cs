using MtgSearch.Server.Models.Logic.Predicates;
using System.Text.RegularExpressions;

namespace MtgSearch.Server.Models.Logic.Parsing
{
    public class AttributeExpressionParser
    {
        private static readonly string __types = string.Join("|", CardAttributeType.ByString.Keys);
        private static readonly string __operators= string.Join("|", Operator.ByString.Keys);
        public static readonly Regex AttributeOperatorExpression = new(@"("+__types+@")\s*("+__operators+@")\s*(\d{1,4})");
        private static readonly string __assertion= string.Join("|", HasOrIs.ByString.Keys);
        //need the order by descending to match XXX before X so "mv is XXX" is fully matched instead of "'mv is X'XX"
        private static readonly string __starOrXs= string.Join("|", XorStar.ByString.Keys.Select(x=>x.Replace("*",@"\*").Replace("+",@"\+")).OrderByDescending(x=>x.Length));
        public static readonly Regex AttributeIsExpression = new(@"("+__types+@")\s*("+__assertion+@")\s*("+__starOrXs+")",RegexOptions.IgnoreCase);
        private readonly VariableSubstitutionSet substitutionSet;
        public AttributeExpressionParser(VariableSubstitutionSet substitutionSet)
        {
            this.substitutionSet = substitutionSet;
        }
        public string ParseAndSubstituteStarBased(Match match)
        {
            if(match.Groups.Count != 4)
            {
                throw new ArgumentException(
                    "expected match to have self as group and attribute, assertion, and symbol as groups, " +
                    $"got `{match}` with {match.Groups.Count} groups"
                );
            }
            var target = match.Groups[1].ToString();
            if(!CardAttributeType.ByString.TryGetValue(target, out var type))
            {
                throw new NotImplementedException($"unrecognized expression type `{target}` at `{match}`");
            }
            var assertionText = match.Groups[2].ToString();
            if(!HasOrIs.ByString.TryGetValue(assertionText, out var assertion))
            {
                throw new NotImplementedException($"unrecognized `{nameof(HasOrIs)}` type `{assertionText}` at `{match}`");
            }
            var symbolText= match.Groups[3].ToString();
            if(!XorStar.ByString.TryGetValue(symbolText, out var symbol))
            {
                throw new NotImplementedException($"unrecognized `{nameof(XorStar)}` type `{symbolText}` at `{match}`");
            }
            return substitutionSet.AddNext(new AttributeAssertionPredicate(type,assertion,symbol)).ToString();
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
            if(!CardAttributeType.ByString.TryGetValue(target, out var type))
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
