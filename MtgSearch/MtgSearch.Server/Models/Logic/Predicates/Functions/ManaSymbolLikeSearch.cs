using MtgSearch.Server.Models.Data;
using MtgSearch.Server.Models.Logic.Highlighting;
using System.Text.RegularExpressions;

namespace MtgSearch.Server.Models.Logic.Predicates.Functions
{
    public class ManaSymbolLikeSearch : ISearchPredicate
    {
        private readonly Regex search;
        public ManaSymbolLikeSearch(Regex symbolSearch) 
        {
            search = symbolSearch;
        }
        public bool Apply(ServerCardModel card)
        {
            var simplified = card.ManaCost?.Replace("{", "").Replace("}", "") ?? "";
            var match = search.Match(simplified,0);
            return match.Success && match.Length == simplified.Length;
        }

        public List<Highlighter> FetchHighlighters() => [];
    }
}
