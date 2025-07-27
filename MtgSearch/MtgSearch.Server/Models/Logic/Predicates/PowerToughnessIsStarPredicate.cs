using MtgSearch.Server.Models.Data;
using MtgSearch.Server.Models.Logic.Highlighting;
using System.Runtime.CompilerServices;

namespace MtgSearch.Server.Models.Logic.Predicates
{
    public class PowerOrToughness
    {        
        public static PowerOrToughness Power = new();
        public static PowerOrToughness Toughness = new();
        public static readonly IReadOnlyDictionary<string, PowerOrToughness> ByString = new Dictionary<string, PowerOrToughness>
        {
            { "pow", Power },
            { "def", Toughness },
            { "toughness", Toughness }
        };
        public readonly string Name;
        public override string ToString() => Name;
        private PowerOrToughness([CallerMemberName] string toStr = "") 
        { 
            Name = toStr;
        }
    }
    public class PowerToughnessIsStarPredicate:ISearchPredicate
    {
        public PowerOrToughness PowerOrToughness { get; set; }

        public bool Apply(MtgJsonAtomicCard card)
        {
            if (PowerOrToughness == PowerOrToughness.Power)
            {
                return card.power == "*";
            }
            else if(PowerOrToughness == PowerOrToughness.Toughness)
            {
                return card.toughness == "*";
            }
            else
            {
                throw new ArgumentException($"powerOrToughness must be either power or toughness, was '{PowerOrToughness}'");
            }
        }

        public List<Highlighter> FetchHighlighters() => [];
    }
}
