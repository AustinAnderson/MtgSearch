using MtgSearch.Server.Models.Data;
using MtgSearch.Server.Models.Logic.Highlighting;
using System.Text.RegularExpressions;

namespace MtgSearch.Server.Models.Logic.Predicates
{
    public class ActivatedAbilitySearchPredicate : ISearchPredicate
    {
        public Regex? CostText { get; set; }
        public Regex? CostAntiText { get; set; }
        public Regex? AbilityText { get; set; }
        public Regex? AbilityAntiText { get; set; }

        public bool Apply(MtgJsonAtomicCard card)
        {
            if (!card.activatedAbilities.Any()) return false;
            bool isMatch = false;
            foreach (var ability in card.activatedAbilities)
            {
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
