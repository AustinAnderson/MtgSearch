using MtgSearch.Server.Models.Data;
using MtgSearch.Server.Models.Logic.Highlighting;

namespace MtgSearch.Server.Models.Logic.Predicates.Functions
{
    public class HasAltFacePredicate : ISearchPredicate, IFunctionInfo
    {
        public static IFunctionInfo FunctionInfo { get; } = new HasAltFacePredicate();
        public string ParseAs => "hasAltFace";
        public string[] Signitures => ["hasAltFace()"];
        public string[] Comments => [
            "matches if the card has an alt face",
            "this includes modals such as double sided cards",
            "but also adventures and cards that rotate for the other textbox"
        ];
        public string[] Examples => ["hasAltFace()"];
        public ISearchPredicate Factory(string[] args, string context) => new HasAltFacePredicate();

        public bool Apply(ServerCardModel card) => card.AltFaceName != null;
        public List<Highlighter> FetchHighlighters() => [];
    }
}
