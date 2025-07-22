using MtgSearch.Server.Models.Data;

namespace MtgSearch.Server.Models.Logic.Predicates
{
    public class PredicateAndCombination : ISearchPredicate
    {
        public List<ISearchPredicate> Predicates { get; set; }
        public bool Apply(MtgJsonAtomicCard card)
        {
            bool isMatch = true;
            foreach (var pred in Predicates)
            {
                isMatch &= pred.Apply(card);
            }
            return isMatch;
        }
    }
}
