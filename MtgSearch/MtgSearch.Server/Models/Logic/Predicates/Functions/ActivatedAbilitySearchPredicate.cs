using MtgSearch.Server.Models.Data;
using MtgSearch.Server.Models.Logic.Highlighting;
using MtgSearch.Server.Models.Logic.Parsing;
using System.Text.RegularExpressions;

namespace MtgSearch.Server.Models.Logic.Predicates.Functions
{
    public class ActivatedAbilitySearchPredicate : ISearchPredicate, IFunctionInfo
    {
        public static IFunctionInfo FunctionInfo { get; } = new ActivatedAbilitySearchPredicate();
        public string ParseAs => "activated";
        public string[] Signitures => [
            "activated(\"sacrifice\", \"{T}\", \"put.*counter\", \"\\+1/\\+1\")",
             "activated(\"{T}\", \"draw\")"];
        public string[] Comments => [
            "matches if any of it's abilities match where the costReg matches a cost and the abilityReg matches the ability",
            "costAntiReg and abilityAntiReg will return false if they match on that same ability line",
            "example: {T}, sac a creature: draw a card; could be matched by activated(\"sac\",\"draw\") but not activated(\"sac\",\"{T}\",\"draw\",\"\")"
         ];
        public string[] Examples => [
            "activated(costFilter: regex, costAntiFilter: regex, abilityFilter: regex, abilityAntiFilter: regex)",
            "activated(costFilter: regex, abilityFilter: regex)"
        ];
        public ISearchPredicate Factory(string[] args, string context)
        {
            if (args.Length != 2 && args.Length != 4)
            {
                throw new QueryParseException($"activated takes exactly either two or four arguments, at ...{context}");
            }
            var res = new ActivatedAbilitySearchPredicate();
            if (args.Length == 2)
            {
                res.CostText = string.IsNullOrEmpty(args[0]) ? null : ParsibleFunction.ParseRegexOrThrow(args[0], context, 0);
                res.AbilityText = string.IsNullOrEmpty(args[1]) ? null : ParsibleFunction.ParseRegexOrThrow(args[1], context, 1);
            }
            else
            {
                res.CostText = string.IsNullOrEmpty(args[0]) ? null : ParsibleFunction.ParseRegexOrThrow(args[0], context, 0);
                res.CostAntiText = string.IsNullOrEmpty(args[1]) ? null : ParsibleFunction.ParseRegexOrThrow(args[1], context, 1);
                res.AbilityText = string.IsNullOrEmpty(args[2]) ? null : ParsibleFunction.ParseRegexOrThrow(args[2], context, 2);
                res.AbilityAntiText = string.IsNullOrEmpty(args[3]) ? null : ParsibleFunction.ParseRegexOrThrow(args[3], context, 3);
            }
            return res;

        }


        public Regex? CostText { get; set; }
        public Regex? CostAntiText { get; set; }
        public Regex? AbilityText { get; set; }
        public Regex? AbilityAntiText { get; set; }

        public bool Apply(ServerCardModel card)
        {
            if(card.Name.ToLower() == "liliana of the veil")
            {
                int i = 0;
            }
            if (!card.ActivatedAbilities.Any()) return false;
            bool isMatch = false;
            foreach (var ability in card.ActivatedAbilities)
            {
                if (AbilityAntiText != null && AbilityAntiText.IsMatch(ability.ability)) return false;
                if (CostAntiText != null && ability.costs.Any(CostAntiText.IsMatch)) return false;

                bool abilityMatch = AbilityText != null && AbilityText.IsMatch(ability.ability);
                bool costMatch = CostText != null && ability.costs.Any(CostText.IsMatch);
                isMatch = abilityMatch || costMatch;
                if (CostText != null && AbilityText != null)
                {
                    isMatch = abilityMatch && costMatch;
                }
            }
            return isMatch;
        }

        public List<Highlighter> FetchHighlighters()
        {
            var highlighters = new List<Highlighter>();
            if(CostText != null && CostAntiText == null) 
            { 
                highlighters.Add(new Highlighter { Regex = CostText, Target = Highlighter.HlTarget.ActivationCost });
            }
            if (AbilityText != null && AbilityAntiText == null)
            {
                highlighters.Add(new Highlighter { Regex = AbilityText , Target = Highlighter.HlTarget.ActivatedAbility });
            }
            return highlighters;
        }
    }
}
