using MtgSearch.Server.Models.Data;
using MtgSearch.Server.Models.Logic.Highlighting;
using System.Runtime.CompilerServices;

namespace MtgSearch.Server.Models.Logic.Predicates
{
    public class HasOrIs
    {        
        public static HasOrIs Has = new();
        public static HasOrIs Is = new();
        public static readonly IReadOnlyDictionary<string, HasOrIs> ByString = new Dictionary<string, HasOrIs>
        {
            { "is", Is},
            { "has", Has },
        };
        public readonly string Name;
        public override string ToString() => Name;
        private HasOrIs([CallerMemberName] string toStr = "") 
        { 
            Name = toStr;
        }
    }
    public class XorStar
    {
        public static XorStar X = new("X");
        public static XorStar XX = new("XX");
        public static XorStar XXX = new("XXX");
        public static XorStar Star = new("*");
        public static readonly IReadOnlyDictionary<string, XorStar> ByString = new Dictionary<string, XorStar>
        {
            { "X", X},
            { "XX", XX},
            { "XXX", XXX},
            { "*", Star },
        };
        public readonly string Name;
        public readonly string Symbol;
        public override string ToString() => Name;
        private XorStar(string symbol, [CallerMemberName] string toStr = "") 
        { 
            Name = toStr;
            Symbol = symbol;
        }
    }
    public class AttributeAssertionPredicate:ISearchPredicate
    {
        public AttributeAssertionPredicate(CardAttributeType attributeType, HasOrIs hasOrIs, XorStar xorStar)
        {
            CardAttribute = attributeType;
            HasOrIs = hasOrIs;
            XorStar = xorStar;
        }

        public CardAttributeType CardAttribute { get; }
        public HasOrIs HasOrIs { get; }
        public XorStar XorStar { get; }

        public bool Apply(ServerCardModel card)
        {
            var attribute = CardAttribute switch
            {
                _ when CardAttribute == CardAttributeType.Power => card.Power,
                _ when CardAttribute == CardAttributeType.Toughness => card.Toughness,
                _ when CardAttribute == CardAttributeType.Loyalty => card.Loyalty,
                _ when CardAttribute == CardAttributeType.ConvertedManaCost => card.ManaCost?.Replace("{","").Replace("}",""),
                _ => throw new NotImplementedException($"Dev forgot to handle {nameof(CardAttribute)}.{CardAttribute}")
            };
            return HasOrIs switch
            {
                _ when HasOrIs == HasOrIs.Has => attribute != null && attribute.Contains(XorStar.Symbol),
                _ when HasOrIs == HasOrIs.Is => attribute == XorStar.Symbol,
                _ => throw new NotImplementedException($"Dev forgot to handle {nameof(HasOrIs)}.{HasOrIs}")
            };
        }

        public List<Highlighter> FetchHighlighters() => [];
    }
}
