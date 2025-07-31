using MtgSearch.Server.Models.Data;
using MtgSearch.Server.Models.Logic.Highlighting;

namespace MtgSearch.Server.Models.Logic.Predicates
{
    public class PredicateNegation : ISearchPredicate
    {
        public ISearchPredicate Predicate { get; set; }
        public bool Apply(ServerCardModel card) => !Predicate.Apply(card);
        public List<Highlighter> FetchHighlighters() => [];//tree negated, don't want to highlight matches
    }
}
