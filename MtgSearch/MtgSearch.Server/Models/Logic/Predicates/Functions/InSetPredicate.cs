using MtgSearch.Server.Models.Data;
using MtgSearch.Server.Models.Logic.Highlighting;
using MtgSearch.Server.Models.Logic.Parsing;

namespace MtgSearch.Server.Models.Logic.Predicates.Functions
{
    public class InSetPredicate : IFunctionInfo, ISearchPredicate
    {
        public static IFunctionInfo FunctionInfo { get; } = new InSetPredicate("");

        private readonly string setCode;
        public InSetPredicate(string setCode)
        {
            this.setCode = setCode;
        }
        public string ParseAs => "inSet";
        public string[] Signitures => ["inSet(setCode: string)"];
        public string[] Comments => ["matches if the card has the given set code"];
        public string[] Examples => ["inSet(\"TLA\")"];

        public bool Apply(ServerCardModel card) => string.Equals(card.SetCode,setCode, StringComparison.OrdinalIgnoreCase);
        public ISearchPredicate Factory(string[] args, string context)
        {
            if (args.Length != 1)
            {
                throw new QueryParseException($"{ParseAs} requires exactly 1 argument at ...{context}");
            }
            return new InSetPredicate(args[0]);
        }
        public List<Highlighter> FetchHighlighters() => [];
    }
}
