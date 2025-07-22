using MtgSearch.Server.Models.Data;
using System.Text.RegularExpressions;

namespace MtgSearch.Server.Models.Logic.Predicates
{
    public class ActivatedAbilitySearchPredicate : ISearchPredicate
    {
        public Regex? CostText { get; set; }
        public Regex? AbilityText { get; set; }
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
    }
}
