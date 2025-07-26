using MtgSearch.Server.Models.Logic.Predicates;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MtgSearch.Server.Models.Logic.Parsing
{
    public class Function
    {
        public static IReadOnlyDictionary<string, Function> ByName => _functionsByName;
        private readonly static Dictionary<string,Function> _functionsByName = new Dictionary<string,Function>();
        public Func<string[],string, ISearchPredicate> Factory { get; }
        public string[] Signitures { get; }
        public string[] Comments { get; }
        public string Name { get; }
        private Function(string name, string[] comments, string[] signitures, Func<string[],string, ISearchPredicate> factory)
        {
            Name = name;
            _functionsByName[name] = this;
            Comments = comments;
            Signitures = signitures;
            Factory = factory;
        }
        public static readonly Function Reg = new("reg",
            ["search the text box with the regex, ignoring case"],
            ["reg(regex: string)"],
            (args, ctx) =>
            {
                if (args.Length != 1)
                {
                    throw new QueryParseException($"reg takes exactly one argument, at ...{ctx}");
                }
                return new TextSearchPredicate(ParseRegexOrThrow(args[0], ctx, 0));
            }
        );
        public static readonly Function Activated = new("activated",
            ["matches if any of it's abilities match where the costReg matches a cost and the abilityReg matches the ability",
             "costAntiReg and abilityAntiReg will return false if they match on that same ability line",
             "example: {T}, sac a creature: draw a card; could be matched by activated(\"sac\",\"draw\") but not activated(\"sac\",\"{T}\",\"draw\",\"\")"
            ],
            ["activated(costReg:string, costAntiReg: string, abilityReg: string, abilityAntiReg: string)",
             "activated(costReg:string, abilityReg: string)"
            ],
            (args, ctx) =>
            {
                if(args.Length != 2 && args.Length != 4) 
                {
                    throw new QueryParseException($"activated takes exactly either two or four arguments, at ...{ctx}");
                }
                var res = new ActivatedAbilitySearchPredicate();
                if (args.Length == 2)
                {
                    res.CostText = ParseRegexOrThrow(args[0], ctx, 0);
                    res.AbilityText = ParseRegexOrThrow(args[1], ctx, 1);
                }
                else
                {
                    res.CostText = ParseRegexOrThrow(args[0], ctx, 0);
                    res.CostAntiText = ParseRegexOrThrow(args[1], ctx, 1);
                    res.AbilityText = ParseRegexOrThrow(args[2], ctx, 2);
                    res.AbilityAntiText = ParseRegexOrThrow(args[3], ctx, 3);
                }
                return res;
            }
        );
        /* //TODO: not implemented yet as pred
        public static readonly Function Triggered = new("triggered",
            ["matches if any of the triggered abilities match the criteria"],
            ["triggered(triggerReg: string, abilityReg: string)",
             "triggered(triggerReg: string, triggerAntiReg: string, abilityReg: string, abilityAntiReg: string)"
            ],
            (args, ctx) => {
                if(args.Length != 2 && args.Length != 4) 
                {
                    throw new QueryParseException($"triggered takes exactly either two or four arguments, at ...{ctx}");
                }
                .......
            }
        );
        */
        public static readonly Function SuperType = new("superType",
            ["matches if the card's list of super types is just this one",
             "you can use 'or' with two of these for exactly this or the other and no others"],
            ["superType(value: string)"],
            (args, ctx) => {
                if (args.Length != 1)
                {
                    throw new QueryParseException($"superType takes exactly one argument, at ...{ctx}");
                }
                return new SuperTypeSearchPredicate { Exact = args[0] };
            }
        );
        public static readonly Function SuperType_Any = new("superTypes.any",
            ["matches if any of the supplied super types show up in the card's super type list"],
            ["superTypes.any(...values: string[])"],
            (args, ctx) => new SuperTypeSearchPredicate { Any = args }
        );
        public static readonly Function SuperType_All = new("superTypes.all",
            ["matches if all of the supplied super types show up in the card's super type list"],
            ["superTypes.all(...values: string[])"],
            (args, ctx) => new SuperTypeSearchPredicate { All = args }
        );
        public static readonly Function Type = new("type",
            ["matches if the card's list of types is just this one"],
            ["type(value: string)"],
            (args, ctx) => {
                if (args.Length != 1)
                {
                    throw new QueryParseException($"type takes exactly one argument, at ...{ctx}");
                }
                return new TypeSearchPredicate { Exact = args[0] };
            }
        );
        public static readonly Function Type_Any = new("types.any",
            ["matches if any of the supplied types show up in the card's type list"],
            ["types.any(...values: string[])"],
            (args, ctx) => new TypeSearchPredicate { Any = args }
        );
        public static readonly Function Type_All = new("types.all",
            ["matches if all of the supplied types show up in the card's type list"],
            ["types.all(...values: string[])"],
            (args, ctx) => new TypeSearchPredicate { All = args }
        );
        public static readonly Function SubType = new("subType",
            ["matches if the card's list of subtypes is just this one"],
            ["subType(value: string)"],
            (args, ctx) => {
                if (args.Length != 1)
                {
                    throw new QueryParseException($"subType takes exactly one argument, at ...{ctx}");
                }
                return new SubTypeSearchPredicate { Exact = args[0] };
            }
        );
        public static readonly Function SubType_Any = new("subTypes.any",
            ["matches if any of the supplied sub types show up in the card's sub type list"],
            ["subTypes.any(...values: string[])"],
            (args, ctx) => new SubTypeSearchPredicate { Any = args }
        );
        public static readonly Function SubType_All = new("subTypes.all",
            ["matches if all of the supplied sub types show up in the card's sub type list"],
            ["subTypes.all(...values: string[])"],
            (args, ctx) => new SubTypeSearchPredicate { All = args }
        );


        private static Regex ParseRegexOrThrow(string argument, string errorContext, int argNum)
        {
            Regex parsed;
            try
            {
                parsed = new Regex(argument, RegexOptions.IgnoreCase);
            }
            catch(ArgumentException ex)
            {
                throw new QueryParseException($"invalid regex `{argument}` at ...{errorContext}, argument {argNum + 1}", ex);
            }
            return parsed;
        }
    }
}
