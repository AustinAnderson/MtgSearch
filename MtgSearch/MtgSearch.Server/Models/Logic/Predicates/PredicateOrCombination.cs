using MtgSearch.Server.Models.Data;
using MtgSearch.Server.Models.Logic.Highlighting;

namespace MtgSearch.Server.Models.Logic.Predicates
{
    public class PredicateOrCombination : ISearchPredicate
    {
        public List<ISearchPredicate> Predicates { get; set; }
        public bool Apply(ServerCardModel card)
        {
            foreach (var predicate in Predicates)
            {
                if (predicate.Apply(card)) return true;
            }
            return false;
        }
        public List<Highlighter> FetchHighlighters()
        {
            var highlighters = new List<Highlighter>();
            highlighters.AddRange(Predicates.SelectMany(x => x.FetchHighlighters()));
            return highlighters;
        }
    }
}
