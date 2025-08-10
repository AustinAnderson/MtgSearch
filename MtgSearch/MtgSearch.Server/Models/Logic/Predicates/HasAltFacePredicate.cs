using MtgSearch.Server.Models.Data;
using MtgSearch.Server.Models.Logic.Highlighting;

namespace MtgSearch.Server.Models.Logic.Predicates
{
    public class HasAltFacePredicate : ISearchPredicate
    {
        public bool Apply(ServerCardModel card) => card.AltFaceName != null;
        public List<Highlighter> FetchHighlighters() => [];
    }
}
