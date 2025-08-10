using MtgSearch.Server.Models.Data;
using MtgSearch.Server.Models.Logic.Highlighting;
using System.Text.RegularExpressions;

namespace MtgSearch.Server.Models.Logic.Predicates
{
    public class NameLikePredicate : ISearchPredicate
    {
        private readonly string fuzName;

        public NameLikePredicate(string fuzName)
        {
            this.fuzName = fuzName;
        }
        private static readonly Regex splitter = new Regex("[ ;:,]");
        public bool Apply(ServerCardModel card)
        {
            var split = splitter.Split(card.Name.ToLower());
            foreach(var piece in split)
            {
                if (string.IsNullOrEmpty(piece)) continue;
                var dist = Fastenshtein.Levenshtein.Distance(piece, fuzName);
                if (dist < 2) return true;
            }
            return false;
        }
        public List<Highlighter> FetchHighlighters() => [];
    }
}
