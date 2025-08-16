using MtgSearch.Server.Models.Data;
using MtgSearch.Server.Models.Logic.Highlighting;
using MtgSearch.Server.Models.Logic.Parsing;
using System.Text.RegularExpressions;

namespace MtgSearch.Server.Models.Logic.Predicates.Functions
{
    public class TextSearchPredicate : ISearchPredicate, IFunctionInfo
    {
        public static IFunctionInfo FunctionInfo { get; } = new TextSearchPredicate(null!);
        public string ParseAs => "text";
        public string[] Signitures => ["text(filter: regex)"];
        public string[] Comments => ["search the text box with the regex, ignoring case"];
        public string[] Examples => ["text(\"gain life.*draw\")"];
        public ISearchPredicate Factory(string[] args, string context)
        {
            if (args.Length != 1)
            {
                throw new QueryParseException($"{ParseAs} takes exactly one argument, at ...{context}");
            }
            return new TextSearchPredicate(ParsibleFunction.ParseRegexOrThrow(args[0], context, 0));
        }


        public TextSearchPredicate(Regex reg) => Regex = reg;
        public Regex Regex { get; }
        public bool Apply(ServerCardModel card)
        {
            if(card.Text == null) return false;
            return Regex.IsMatch(card.Text);
        }

        public List<Highlighter> FetchHighlighters() 
            => [new Highlighter { Regex =  Regex, Target = Highlighter.HlTarget.FullText }];
    }
}
