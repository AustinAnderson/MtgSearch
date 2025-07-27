using MtgSearch.Server.Models.Data;
using MtgSearch.Server.Models.Logic.Highlighting;

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

        public List<Highlighter> FetchHighlighters()
        {
            var highlighters = new List<Highlighter>();
            highlighters.AddRange(Predicates.SelectMany(x => x.FetchHighlighters()));
            return highlighters;
        }
    }
}
