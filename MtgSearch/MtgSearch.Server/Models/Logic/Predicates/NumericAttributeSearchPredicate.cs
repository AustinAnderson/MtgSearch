using MtgSearch.Server.Models.Data;

namespace MtgSearch.Server.Models.Logic.Predicates
{

    public enum NumericCardAttributeType
    {
        CMC, Power, Toughness, Loyalty
    }
    public enum Operator
    {
        Gt, Lt, Gte, Lte, Equals
    }
    //TODO: checkbox to exclude X mana value or * power or * toughness
    public class NumericAttributeSearchPredicate : ISearchPredicate
    {
        public NumericCardAttributeType Type { get; set; }
        public Operator Operator { get; set; }
        public int Value { get; set; }
        public bool Apply(MtgJsonAtomicCard card)
        {
            int compareAgainst = 0;
            try
            {
                compareAgainst = Type switch
                {
                    NumericCardAttributeType.CMC => (int)card.manaValue,
                    NumericCardAttributeType.Power => card.power == "*" || card.power == null ? 0 : int.Parse(card.power),
                    NumericCardAttributeType.Toughness => card.toughness == "*" || card.toughness == null ? 0 : int.Parse(card.toughness),
                    NumericCardAttributeType.Loyalty => card.toughness == null ? 0 : int.Parse(card.loyalty),
                    _ => throw new NotImplementedException($"Dev forgot to handle {nameof(NumericCardAttributeType)}.{Type}")
                };
            }
            catch (FormatException ex)
            {
                throw new Exception($"Couldn't parse {Type} for card `{card.name}`", ex);
            }
            return Operator switch
            {
                Operator.Gt => compareAgainst > Value,
                Operator.Lt => compareAgainst < Value,
                Operator.Gte => compareAgainst >= Value,
                Operator.Lte => compareAgainst <= Value,
                Operator.Equals => compareAgainst == Value,
                _ => throw new NotImplementedException($"Dev forgot to handle {nameof(Operator)}.{Operator}")
            };
        }
    }
}
