using System.Text.RegularExpressions;

namespace MtgSearch.Server.Models.Logic.Highlighting
{
    public class Highlighter
    {
        public enum HlTarget
        {
            FullText,
            Trigger,
            TriggeredAbility,
            ActivationCost,
            ActivatedAbility
        }
        public HlTarget Target { get; set; } = HlTarget.FullText;
        public Regex Regex { get; set; } = new("");
    }
}
