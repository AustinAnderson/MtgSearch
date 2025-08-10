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
        public string[] Examples { get; }
        public string Name { get; }
        private Function(string name, string[] signitures, string[] comments, string[] examples, Func<string[],string, ISearchPredicate> factory)
        {
            Name = name;
            _functionsByName[name] = this;
            Comments = comments;
            Examples = examples;
            Signitures = signitures;
            Factory = factory;
        }
        public static readonly Function ManaSymbolsLike = new("manaCostLike",
            ["manaCostLike(filter: regex)"],
            ["matches the mana cost symbols based on simple 'w' 'u' 'b' 'r' 'g' text",
            "for instance, '5w*' would match Avacyn, Angle of Hope's cost=5{W}{W}{W} and The Eternity Elevator's cost=5"],
            ["manaCostLike(\"5w*\")"],
            (args, ctx) => {
                if(args.Length != 1)
                {
                    throw new QueryParseException("manaCostLike requires exactly 1 argument");
                }
                return new ManaSymbolLikeSearch(ParseRegexOrThrow(args[0], ctx, 0));
            }
        );
        public static readonly Function TextRegex = new("text",
            ["text(filter: regex)"],
            ["search the text box with the regex, ignoring case"],
            ["text(\"gain life.*draw\")"],
            (args, ctx) =>
            {
                if (args.Length != 1)
                {
                    throw new QueryParseException($"text takes exactly one argument, at ...{ctx}");
                }
                return new TextSearchPredicate(ParseRegexOrThrow(args[0], ctx, 0));
            }
        );
        public static readonly Function Activated = new("activated",
            ["activated(costFilter: regex, costAntiFilter: regex, abilityFilter: regex, abilityAntiFilter: regex)",
             "activated(costFilter: regex, abilityFilter: regex)"
            ],
            ["matches if any of it's abilities match where the costReg matches a cost and the abilityReg matches the ability",
             "costAntiReg and abilityAntiReg will return false if they match on that same ability line",
             "example: {T}, sac a creature: draw a card; could be matched by activated(\"sac\",\"draw\") but not activated(\"sac\",\"{T}\",\"draw\",\"\")"
            ],
            ["activated(\"sacrifice\", \"{T}\", \"put.*counter\", \"\\+1/\\+1\")",
             "activated(\"{T}\", \"draw\")"],
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
            ["superType(value: string)"],
            ["matches if the card's list of super types includes this one"],
            ["superType(\"Legendary\")"],
            (args, ctx) => {
                if (args.Length != 1)
                {
                    throw new QueryParseException($"superType takes exactly one argument, at ...{ctx}");
                }
                return new SuperTypeSearchPredicate { Includes = args[0] };
            }
        );
        public static readonly Function SuperType_Any = new("superTypes.any",
            ["superTypes.any(...values: string[])"],
            ["matches if any of the supplied super types show up in the card's super type list",
             "shorthand for (superType(<type1>) or superType(<type2>)...)"
            ],
            ["superTypes.any(\"Legendary\",\"Snow\")"],
            (args, ctx) => new SuperTypeSearchPredicate { Any = args }
        );
        public static readonly Function SuperType_All = new("superTypes.all",
            ["superTypes.all(...values: string[])"],
            ["matches if all of the supplied super types show up in the card's super type list",
             "shorthand for (superType(<type1>) and superType(<type2>)...)"
            ],
            ["superTypes.all(\"Legendary\",\"Snow\")"],
            (args, ctx) => new SuperTypeSearchPredicate { All = args }
        );
        public static readonly Function Type = new("type",
            ["type(value: string)"],
            ["matches if the card's list of types includes this one"],
            ["type(\"Creature\")"],
            (args, ctx) => {
                if (args.Length != 1)
                {
                    throw new QueryParseException($"type takes exactly one argument, at ...{ctx}");
                }
                return new TypeSearchPredicate { Includes = args[0] };
            }
        );
        public static readonly Function Type_Any = new("types.any",
            ["types.any(...values: string[])"],
            ["matches if any of the supplied types show up in the card's type list",
             "shorthand for (type(<type1>) or type(<type2>)...)"
            ],
            ["types.any(\"Enchantment\",\"Creature\")"],
            (args, ctx) => new TypeSearchPredicate { Any = args }
        );
        public static readonly Function Type_All = new("types.all",
            ["types.all(...values: string[])"],
            ["matches if all of the supplied types show up in the card's type list",
             "shorthand for (type(<type1>) and type(<type2>)...)"
            ],
            ["types.all(\"Enchantment\",\"Creature\")"],
            (args, ctx) => new TypeSearchPredicate { All = args }
        );
        public static readonly Function SubType = new("subType",
            ["subType(value: string)"],
            ["matches if the card's list of subtypes includes this one"],
            ["subType(\"Eldrazi\")"],
            (args, ctx) => {
                if (args.Length != 1)
                {
                    throw new QueryParseException($"subType takes exactly one argument, at ...{ctx}");
                }
                return new SubTypeSearchPredicate { Includes = args[0] };
            }
        );
        public static readonly Function SubType_Any = new("subTypes.any",
            ["subTypes.any(...values: string[])"],
            ["matches if any of the supplied sub types show up in the card's sub type list",
             "shorthand for (subType(<type1>) or subType(<type2>)...)"
            ],
            ["subTypes.any(\"Phyrexian\",\"Dog\")"],
            (args, ctx) => new SubTypeSearchPredicate { Any = args }
        );
        public static readonly Function SubType_All = new("subTypes.all",
            ["subTypes.all(...values: string[])"],
            ["matches if all of the supplied sub types show up in the card's sub type list",
             "shorthand for (subType(<type1>) and subType(<type2>)...)"
            ],
            ["subTypes.all(\"Phyrexian\",\"Dog\")"],
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
