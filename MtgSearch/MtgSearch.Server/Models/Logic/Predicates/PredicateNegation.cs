using MtgSearch.Server.Models.Data;

namespace MtgSearch.Server.Models.Logic.Predicates
{
    public class PredicateNegation : ISearchPredicate
    {
        public ISearchPredicate Predicate { get; set; }
        public bool Apply(MtgJsonAtomicCard card) => !Predicate.Apply(card);
    }
}
