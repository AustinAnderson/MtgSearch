using MtgSearch.Server.Models.Data;

namespace MtgSearch.Server.Models.Logic.Predicates
{
    public class PredicateOrCombination : ISearchPredicate
    {
        public List<ISearchPredicate> Predicates { get; set; }
        public bool Apply(MtgJsonAtomicCard card)
        {
            foreach (var predicate in Predicates)
            {
                if (predicate.Apply(card)) return true;
            }
            return false;
        }
    }
}
