using MtgSearch.Server.Models.Data;
using MtgSearch.Server.Models.Logic.Highlighting;
using System.Text.RegularExpressions;

namespace MtgSearch.Server.Models.Logic.Predicates
{
    public class ManaSymbolLikeSearch : ISearchPredicate
    {
        private readonly Regex search;
        public ManaSymbolLikeSearch(Regex symbolSearch) 
        {
            search = symbolSearch;
        }
        public bool Apply(MtgJsonAtomicCard card)
        {
            var simplified = card.manaCost?.Replace("{", "").Replace("}", "") ?? "";
            var match = search.Match(simplified,0);
            return match.Success && match.Length == simplified.Length;
        }

        public List<Highlighter> FetchHighlighters() => [];
    }
}
