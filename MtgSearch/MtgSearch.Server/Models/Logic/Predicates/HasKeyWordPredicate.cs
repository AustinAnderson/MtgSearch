using MtgSearch.Server.Models.Data;
using MtgSearch.Server.Models.Logic.Highlighting;
using MtgSearch.Server.Models.Logic.Parsing;

namespace MtgSearch.Server.Models.Logic.Predicates
{
    public class HasKeyWordPredicate : IFunctionInfo, ISearchPredicate
    {
        public static IFunctionInfo FunctionInfo { get; } = new HasKeyWordPredicate("");
        public HasKeyWordPredicate(string keyword) 
        {
            this.keyWord = keyword;
        }
        private string keyWord;
        public string ParseAs => "hasKeyWord";

        public string[] Signitures => ["hasKeyWord(keyWord: string)"];
        public string[] Examples => [
            "hasKeyWord(\"trample\")",
            "hasKeyWord(\"Flying\")",
        ];
        public string[] Comments => [
            "matches if the card is a creature with the given keyword",
            "doesn't match things like 'gains flying', only if it's intrinsic to the card"
        ];


        //TODO: rather than parse, should just change datamodel to deserialize keyword list

        public bool Apply(ServerCardModel card)
        {
            foreach(var line in card.Text?.Split("\n") ?? [])
            {
                var csvs=line.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                if(csvs.Any(x=>x.Contains(' ')))
                {
                    continue;
                }
                if (csvs.Any(x => x.Equals(keyWord, StringComparison.OrdinalIgnoreCase))) 
                {
                    return true;
                }
            }
            return false;
        }

        public ISearchPredicate Factory(string[] args, string context)
        {
            if(args.Length != 1)
            {
                throw new QueryParseException($"{ParseAs} requires exactly 1 argument at ...{context}");
            }
            //TODO: enforce valid keyword list?
            return new HasKeyWordPredicate(args[0]);
        }

        public List<Highlighter> FetchHighlighters() 
        {
            return new List<Highlighter>
            {
                new Highlighter
                {
                    //TODO: will match the 'gains flying'
                    Regex = new System.Text.RegularExpressions.Regex(keyWord)
                }
            };
        }
    }
}
