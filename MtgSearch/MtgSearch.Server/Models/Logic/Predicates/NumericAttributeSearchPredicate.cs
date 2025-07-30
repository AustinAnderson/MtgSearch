using MtgSearch.Server.Models.Data;
using MtgSearch.Server.Models.Logic.Highlighting;
using System.Runtime.CompilerServices;

namespace MtgSearch.Server.Models.Logic.Predicates
{

    public class NumericCardAttributeType
    {
        public static NumericCardAttributeType ConvertedManaCost = new();
        public static NumericCardAttributeType Power = new();
        public static NumericCardAttributeType Toughness = new();
        public static NumericCardAttributeType Loyalty = new();
        public static readonly IReadOnlyDictionary<string, NumericCardAttributeType> ByString = new Dictionary<string, NumericCardAttributeType>
        {
            { "mv", ConvertedManaCost },
            { "cmc", ConvertedManaCost },
            { "pow", Power },
            { "def", Toughness },
            { "toughness", Toughness },
            { "loyalty", Loyalty }
        };
        public readonly string Name;
        public override string ToString() => Name;
        private NumericCardAttributeType([CallerMemberName] string toStr = "") 
        { 
            Name = toStr;
        }
    }
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
        public NumericCardAttributeType Type { get; set; } = NumericCardAttributeType.ConvertedManaCost;
        public Operator Operator { get; set; }
        public int Value { get; set; }
        public bool Apply(ServerCardModel card)
        {
            int compareAgainst = 0;
            try
            {
                compareAgainst = Type switch
                {
                    _ when Type == NumericCardAttributeType.ConvertedManaCost => (int)card.ManaValue,
                    _ when Type == NumericCardAttributeType.Power => card.Power == "*" || card.Power == null ? 0 : int.Parse(card.Power),
                    _ when Type == NumericCardAttributeType.Toughness => card.Toughness == "*" || card.Toughness == null ? 0 : int.Parse(card.Toughness),
                    _ when Type == NumericCardAttributeType.Loyalty => card.Toughness == null ? 0 : int.Parse(card.Loyalty),
                    _ => throw new NotImplementedException($"Dev forgot to handle {nameof(NumericCardAttributeType)}.{Type}")
                };
            }
            catch (FormatException ex)
            {
                throw new Exception($"Couldn't parse {Type} for card `{card.Name}`", ex);
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

        public List<Highlighter> FetchHighlighters() => [];
    }
}
