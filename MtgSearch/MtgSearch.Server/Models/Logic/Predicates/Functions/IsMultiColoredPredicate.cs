using MtgSearch.Server.Models.Data;
using MtgSearch.Server.Models.Logic.Highlighting;

namespace MtgSearch.Server.Models.Logic.Predicates.Functions
{
    public class IsMultiColoredPredicate : ISearchPredicate, IFunctionInfo
    {
        public static IFunctionInfo FunctionInfo { get; } = new IsMultiColoredPredicate();
        public string ParseAs => "isMultiColored";
        public string[] Signitures => ["isMultiColored()"];
        public string[] Comments => ["matches if a card has two or more colors"];
        public string[] Examples => ["isMultiColored()"];
        public ISearchPredicate Factory(string[] args, string context) => new IsMultiColoredPredicate();

        public bool Apply(ServerCardModel card) => card.ColorIdentity.Colors.Length > 1;
        public List<Highlighter> FetchHighlighters() => [];
    }
}
