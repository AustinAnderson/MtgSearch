using MtgSearch.Server.Models.Data;
using MtgSearch.Server.Models.Logic.Highlighting;

namespace MtgSearch.Server.Models.Logic.Predicates.Functions
{
    public class IsMultiColoredPredicate : ISearchPredicate
    {
        public bool Apply(ServerCardModel card) => card.ColorIdentity.Colors.Length > 1;
        public List<Highlighter> FetchHighlighters() => [];
    }
}
