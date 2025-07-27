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
        public bool Apply(MtgJsonAtomicCard card)
        {
            int compareAgainst = 0;
            try
            {
                compareAgainst = Type switch
                {
                    _ when Type == NumericCardAttributeType.ConvertedManaCost => (int)card.manaValue,
                    _ when Type == NumericCardAttributeType.Power => card.power == "*" || card.power == null ? 0 : int.Parse(card.power),
                    _ when Type == NumericCardAttributeType.Toughness => card.toughness == "*" || card.toughness == null ? 0 : int.Parse(card.toughness),
                    _ when Type == NumericCardAttributeType.Loyalty => card.toughness == null ? 0 : int.Parse(card.loyalty),
                    _ => throw new NotImplementedException($"Dev forgot to handle {nameof(NumericCardAttributeType)}.{Type}")
                };
            }
            catch (FormatException ex)
            {
                throw new Exception($"Couldn't parse {Type} for card `{card.name}`", ex);
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
