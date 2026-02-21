using MtgSearch.Server.Models.Data;
using MtgSearch.Server.Models.Logic.Highlighting;
using MtgSearch.Server.Models.Logic.Parsing;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace MtgSearch.Server.Models.Logic.Predicates
{
    public class Operator
    {
        public static Operator GreaterThan = new();
        public static Operator LessThan = new();
        public static Operator GreatherThanOrEquals = new();
        public static Operator LessThanOrEquals = new();
        public static Operator Equal = new();
        public static readonly IReadOnlyDictionary<string, Operator> ByString = new Dictionary<string, Operator>
        {
            { ">", GreaterThan },
            { "<", LessThan },
            { ">=", GreatherThanOrEquals },
            { "<=", LessThanOrEquals },
            { "==", Equal },
            { "=", Equal }
        };
        private readonly string Name;
        public override string ToString() => Name;
        private Operator([CallerMemberName] string toStr = "") 
        { 
            Name = toStr;
        }
    }
    //TODO: checkbox to exclude X mana value or * power or * toughness
    public class NumericAttributeSearchPredicate : ISearchPredicate
    {
        public CardAttributeType Type { get; set; } = CardAttributeType.ConvertedManaCost;
        public Operator Operator { get; set; }
        public int Value { get; set; }
        public bool Apply(ServerCardModel card)
        {
            int compareAgainst = -1;
            try
            {
                if(Type == CardAttributeType.ConvertedManaCost)
                {
                    compareAgainst = (int)card.ManaValue;
                }
                else if (Type == CardAttributeType.Power)
                {
                    compareAgainst = HandleStarable(card.Power, compareAgainst, Type.ToString(), card.Name);
                } 
                else if (Type == CardAttributeType.Toughness)
                {
                    compareAgainst = HandleStarable(card.Toughness, compareAgainst, Type.ToString(), card.Name);
                } 
                else if (Type == CardAttributeType.Loyalty)
                {
                    if(card.Loyalty != null)
                    {
                        if (card.Loyalty.Contains('X')) compareAgainst = 0;
                        else compareAgainst = int.Parse(card.Loyalty);
                    }
                }
                else
                {
                    throw new NotImplementedException($"Dev forgot to handle {nameof(CardAttributeType)}.{Type}");
                }
            }
            catch (FormatException ex)
            {
                throw new QueryParseException($"Couldn't parse {Type} for card `{card.Name}`", ex);
            }
            return Operator switch
            {
                _ when Operator == Operator.GreaterThan => compareAgainst > Value,
                _ when Operator == Operator.LessThan => compareAgainst < Value,
                _ when Operator == Operator.GreatherThanOrEquals => compareAgainst >= Value,
                _ when Operator == Operator.LessThanOrEquals => compareAgainst <= Value,
                _ when Operator == Operator.Equal => compareAgainst == Value,
                _ => throw new NotImplementedException($"Dev forgot to handle {nameof(Operator)}.{Operator}")
            };
        }

        private static Regex ParsePtStar=new Regex("(?<val1>[0-9]+)(?:(?<op>[\\+\\-])(?<val2>[0-9]+))?", RegexOptions.Compiled);
        private static int HandleStarable(string? pt, int deflt, string type, string cardName)
        {
            if(pt == null) return deflt;
            var val = deflt;
            if (pt.Contains("*"))
            {
                var numeric=pt.Replace("*", "0");
                if (!ParsePtStar.IsMatch(numeric))
                {
                    throw new QueryParseException($"Couldn't parse {type} value `{pt}` for card `{cardName}`");
                }
                var match = ParsePtStar.Match(numeric);
                var groups = match.Groups;
                var val1 = int.Parse(groups["val1"].Value);
                var val2 = 0;
                var op = 1;
                if (groups["op"].Value != "" && groups["val2"].Value != "")
                {
                    val2 = int.Parse(groups["val2"].Value);
                    if (groups["op"].Value == "-") op = -1;
                }
                val = val1 + op * val2;
            }
            else
            {
                val = int.Parse(pt);
            }
            return val;
        }

        public List<Highlighter> FetchHighlighters() => [];
    }
}
