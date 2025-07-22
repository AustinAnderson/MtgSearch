using MtgSearch.Server.Models.Data;
using System.Text.RegularExpressions;

namespace MtgSearch.Server.Models.Logic.Predicates
{
    public class TextSearchPredicate : ISearchPredicate
    {
        public Regex? TextMatch1 { get; set; }
        public Regex? TextMatch2 { get; set; }
        public Regex? DontMatch { get; set; }

        public bool Apply(MtgJsonAtomicCard card)
        {
            return (TextMatch1 == null || TextMatch1.IsMatch(card.text)) &&
                   (TextMatch2 == null || TextMatch2.IsMatch(card.text)) &&
                   (DontMatch == null || !DontMatch.IsMatch(card.text));
        }
    }
}
