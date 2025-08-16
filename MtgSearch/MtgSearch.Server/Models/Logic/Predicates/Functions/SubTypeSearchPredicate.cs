using MtgSearch.Server.Models.Data;
using MtgSearch.Server.Models.Logic.Parsing;

namespace MtgSearch.Server.Models.Logic.Predicates.Functions
{
    public class HasSubTypeFunctionInfo: IFunctionInfo
    {
        public string ParseAs => "subType";
        public string[] Signitures => ["subType(value: string)"];
        public string[] Comments => ["matches if the card's list of subtypes includes this one"];
        public string[] Examples => ["subType(\"Eldrazi\")"];
        public ISearchPredicate Factory(string[] args, string context)
        {
            if (args.Length != 1)
            {
                throw new QueryParseException($"{ParseAs} takes exactly one argument, at ...{context}");
            }
            return new SubTypeSearchPredicate { Includes = args[0] };
        }
    }
    public class AnySubTypesFunctionInfo: IFunctionInfo
    {
        public string ParseAs => "subTypes.any";
        public string[] Signitures => ["subTypes.any(...values: string[])"];
        public string[] Comments => [
            "matches if any of the supplied sub types show up in the card's sub type list",
            "shorthand for (subType(<type1>) or subType(<type2>)...)"
        ];
        public string[] Examples => ["subTypes.any(\"Phyrexian\",\"Dog\")"];
        public ISearchPredicate Factory(string[] args, string context)
            => new SubTypeSearchPredicate { Any = args };
    }
    public class AllSubTypesFunctionInfo: IFunctionInfo
    {
        public string ParseAs => "subTypes.all";
        public string[] Signitures => ["subTypes.all(...values: string[])"];
        public string[] Comments => [
            "matches if all of the supplied sub types show up in the card's sub type list",
            "shorthand for (subType(<type1>) and subType(<type2>)...)"
        ];
        public string[] Examples => ["subTypes.all(\"Phyrexian\",\"Dog\")"];
        public ISearchPredicate Factory(string[] args, string context)
            => new SubTypeSearchPredicate { All = args };
    }
    public class SubTypeSearchPredicate : AbstractTypeSearchPredicate
    {
        protected override string[] SelectTypeArray(ServerCardModel card) => card.Subtypes;
    }
}
