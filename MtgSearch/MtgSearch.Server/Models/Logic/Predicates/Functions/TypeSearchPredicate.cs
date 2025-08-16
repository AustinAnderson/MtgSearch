using MtgSearch.Server.Models.Data;
using MtgSearch.Server.Models.Logic.Parsing;

namespace MtgSearch.Server.Models.Logic.Predicates.Functions
{
    public class HasTypeFunctionInfo : IFunctionInfo
    {
        public string ParseAs => "type";
        public string[] Signitures => ["type(value: string)"];
        public string[] Comments => ["matches if the card's list of types includes this one"];
        public string[] Examples => ["type(\"Creature\")"];
        public ISearchPredicate Factory(string[] args, string context)
        {
            if (args.Length != 1)
            {
                throw new QueryParseException($"{ParseAs} takes exactly one argument, at ...{context}");
            }
            return new TypeSearchPredicate { Includes = args[0] };
        }
    }
    public class AnyTypesFunctionInfo : IFunctionInfo
    {
        public string ParseAs => "types.any";
        public string[] Signitures => ["types.any(...values: string[])"];
        public string[] Comments => [
            "matches if any of the supplied types show up in the card's type list",
            "shorthand for (type(<type1>) or type(<type2>)...)"
        ];
        public string[] Examples => ["types.any(\"Enchantment\",\"Creature\")"];
        public ISearchPredicate Factory(string[] args, string context)
            => new TypeSearchPredicate { Any = args };
    }
    public class AllTypesFunctionInfo: IFunctionInfo
    {
        public string ParseAs => "types.all";
        public string[] Signitures => ["types.all(...values: string[])"];
        public string[] Comments => [
           "matches if all of the supplied types show up in the card's type list",
           "shorthand for (type(<type1>) and type(<type2>)...)"
        ];
        public string[] Examples => ["types.all(\"Enchantment\",\"Creature\")"];
        public ISearchPredicate Factory(string[] args, string context)
            => new TypeSearchPredicate { All = args };
    }
    public class TypeSearchPredicate : AbstractTypeSearchPredicate
    {
        protected override string[] SelectTypeArray(ServerCardModel card) => card.Types.ToArray();
    }
}
