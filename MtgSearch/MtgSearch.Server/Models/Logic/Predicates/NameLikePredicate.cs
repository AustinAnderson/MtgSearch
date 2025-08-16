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
        private static readonly Regex splitter = new Regex("[ ;:,']");
        private const int TargetDist = 2;
        public bool Apply(ServerCardModel card)
        {
            var currentNameSplit = splitter.Split(card.Name.ToLower());
            if (fuzName.Contains(' '))
            {
                //if input has multiple terms, loop over current card's name's terms until we match the first input term,
                //then loop both to make sure all terms of the input are fuzzy matched
                var fuzSplit = fuzName.Split(' ');
                int found = 0;
                for(int i=0;i<currentNameSplit.Length;i++)
                {
                    var piece = currentNameSplit[i];
                    if (string.IsNullOrEmpty(piece)) continue;
                    var dist = Fastenshtein.Levenshtein.Distance(piece, fuzSplit[found]);
                    if (dist < TargetDist)
                    {
                        found++;
                        i++;
                        bool match = true;
                        while(found<fuzSplit.Length && i < currentNameSplit.Length)
                        {
                            if (Fastenshtein.Levenshtein.Distance(fuzSplit[found], currentNameSplit[i]) > TargetDist)
                            {
                                match = false; break;
                            }
                        }
                        if (match)
                        {
                            return true;
                        }
                    }
                }
            }
            else
            {
                foreach (var piece in currentNameSplit)
                {
                    if (string.IsNullOrEmpty(piece)) continue;
                    var dist = Fastenshtein.Levenshtein.Distance(piece, fuzName);
                    if (dist < TargetDist) return true;
                }
            }
            return false;
        }
        public List<Highlighter> FetchHighlighters() => [];
    }
}
