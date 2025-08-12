using System.Runtime.CompilerServices;

namespace MtgSearch.Server.Models.Logic.Predicates
{
    public class CardAttributeType
    {
        public static CardAttributeType ConvertedManaCost = new();
        public static CardAttributeType Power = new();
        public static CardAttributeType Toughness = new();
        public static CardAttributeType Loyalty = new();
        public static readonly IReadOnlyDictionary<string, CardAttributeType> ByString = new Dictionary<string, CardAttributeType>
        {
            { "mv", ConvertedManaCost },
            { "cmc", ConvertedManaCost },
            { "pow", Power },
            { "def", Toughness },
            { "toughness", Toughness },
            { "loyalty", Loyalty }
        };
        public readonly string Name;
        public override string ToString() => Name;
        private CardAttributeType([CallerMemberName] string toStr = "") 
        { 
            Name = toStr;
        }
    }
}
