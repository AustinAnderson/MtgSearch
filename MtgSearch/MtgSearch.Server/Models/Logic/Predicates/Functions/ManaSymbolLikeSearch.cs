using MtgSearch.Server.Models.Data;
using MtgSearch.Server.Models.Logic.Highlighting;
using MtgSearch.Server.Models.Logic.Parsing;
using System.Text.RegularExpressions;

namespace MtgSearch.Server.Models.Logic.Predicates.Functions
{
    public class ManaSymbolLikeSearch : ISearchPredicate, IFunctionInfo
    {
        public static IFunctionInfo FunctionInfo { get; } = new ManaSymbolLikeSearch(null!);
        public string ParseAs => "manaCostLike";
        public string[] Signitures => ["manaCostLike(filter: regex)"];
        public string[] Comments => [
            "matches the mana cost symbols based on simple 'w' 'u' 'b' 'r' 'g' text",
            "for instance, '5w*' would match Avacyn, Angle of Hope's cost=5{W}{W}{W} and The Eternity Elevator's cost=5"
        ];
        public string[] Examples => ["manaCostLike(\"5w*\")"];
        public ISearchPredicate Factory(string[] args, string context)
        {
            if (args.Length != 1)
            {
                throw new QueryParseException($"{ParseAs} requires exactly 1 argument at ...{context}");
            }
            return new ManaSymbolLikeSearch(ParsibleFunction.ParseRegexOrThrow(args[0], context, 0));
        }

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
