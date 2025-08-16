using MtgSearch.Server.Models.Data;
using MtgSearch.Server.Models.Logic.Parsing;

namespace MtgSearch.Server.Models.Logic.Predicates.Functions
{
    public class HasSuperTypeFunctionInfo : IFunctionInfo
    {
        public string ParseAs => "superType";
        public string[] Signitures => ["superType(value: string)"];
        public string[] Comments => ["matches if the card's list of super types includes this one"];
        public string[] Examples => ["superType(\"Legendary\")"];
        public ISearchPredicate Factory(string[] args, string context)
        {
            if (args.Length != 1)
            {
                throw new QueryParseException($"{ParseAs} takes exactly one argument, at ...{context}");
            }
            return new SuperTypeSearchPredicate { Includes = args[0] };
        }
    }
    public class AllSuperTypesFunctionInfo : IFunctionInfo
    {
        public string ParseAs => "superTypes.all";
        public string[] Signitures => ["superTypes.all(...values: string[])"];
        public string[] Comments => [
            "matches if all of the supplied super types show up in the card's super type list",
            "shorthand for (superType(<type1>) and superType(<type2>)...)"
        ];
        public string[] Examples => ["superTypes.all(\"Legendary\",\"Snow\")"];
        public ISearchPredicate Factory(string[] args, string context)
            => new SuperTypeSearchPredicate { All = args };
    }
    public class AnySuperTypesFunctionInfo : IFunctionInfo
    {
        public string ParseAs => "superTypes.any";
        public string[] Signitures => ["superTypes.any(...values: string[])"];
        public string[] Comments => [
            "matches if any of the supplied super types show up in the card's super type list",
            "shorthand for (superType(<type1>) or superType(<type2>)...)"
        ];
        public string[] Examples => ["superTypes.any(\"Legendary\",\"Snow\")"];
        public ISearchPredicate Factory(string[] args, string context)
            => new SuperTypeSearchPredicate { Any = args };
    }
    public class SuperTypeSearchPredicate: AbstractTypeSearchPredicate
    {
        protected override string[] SelectTypeArray(ServerCardModel card) => card.Supertypes.ToArray();
    }
}
