using MtgSearch.Server.Models.Data;
using MtgSearch.Server.Models.Logic.Highlighting;

namespace MtgSearch.Server.Models.Logic.Predicates.Functions
{
    public class CanBeCommanderPredicate : ISearchPredicate, IFunctionInfo
    {
        public static IFunctionInfo FunctionInfo { get; } = new CanBeCommanderPredicate();
        public string ParseAs => "canBeCommander";
        public string[] Signitures => ["canBeCommander()"];
        public string[] Comments => ["matches if a card is eligible to be your commander"];
        public string[] Examples => ["canBeCommander()"];
        public ISearchPredicate Factory(string[] args, string context) => new CanBeCommanderPredicate();

        public bool Apply(ServerCardModel card)
        {
            return card.Supertypes.Contains("Legendary") && (
                card.Types.Contains("Creature") ||
                card.Power != null &&
                    (card.Subtypes.Contains("Spacecraft") ||
                     card.Subtypes.Contains("Vehicle"))
                
            ) || card.Text != null && card.Text.ToLower().Contains("can be your commander");
        }

        public List<Highlighter> FetchHighlighters() => [];
    }
}
