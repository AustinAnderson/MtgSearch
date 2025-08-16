using MtgSearch.Server.Models.Data;
using MtgSearch.Server.Models.Logic.Highlighting;

namespace MtgSearch.Server.Models.Logic.Predicates.Functions
{
    public class IsPreReleasePredicate : IFunctionInfo, ISearchPredicate
    {
        public static IFunctionInfo FunctionInfo { get; } = new IsPreReleasePredicate();
        public string ParseAs => "isPreRelease";
        public string[] Signitures => ["isPreRelease()"];
        public string[] Comments => [
            "matches if the card has not officially released yet"
        ];
        public string[] Examples => ["isPreRelease()"];
        public ISearchPredicate Factory(string[] args, string context) => new IsPreReleasePredicate();

        public bool Apply(ServerCardModel card) => card.IsPreRelease;
        public List<Highlighter> FetchHighlighters() => [];
    }
}
