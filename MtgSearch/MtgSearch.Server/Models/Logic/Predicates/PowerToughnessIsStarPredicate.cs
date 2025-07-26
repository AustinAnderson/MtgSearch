using MtgSearch.Server.Models.Data;

namespace MtgSearch.Server.Models.Logic.Predicates
{
    public enum PowerOrToughness
    {
        Power,
        Toughness
    }
    public class PowerToughnessIsStarPredicate:ISearchPredicate
    {
        public PowerOrToughness PowerOrToughness { get; set; }

        public bool Apply(MtgJsonAtomicCard card)
        {
            if (PowerOrToughness == PowerOrToughness.Power)
            {
                return card.power == "*";
            }
            else if(PowerOrToughness == PowerOrToughness.Toughness)
            {
                return card.toughness == "*";
            }
            else
            {
                throw new ArgumentException($"powerOrToughness must be either power or toughness, was '{PowerOrToughness}'");
            }
        }
    }
}
