using MtgSearch.Server.Models.Data;

namespace MtgSearch.Server.Models.Api
{
    public class SearchResultCard
    {
        public SearchResultCard() { }
        public SearchResultCard(MtgJsonAtomicCard card) 
        {
            Name = card.name;
            Power = card.power;
            Toughness = card.toughness;
            Loyalty = card.loyalty;
            ManaCost = card.manaCost;
            SuperTypes = card.supertypes;
            Types = card.types;
            SubTypes = card.subtypes;
        }
        public string Name { get; set; }
        public string? Power { get; set; }
        public string? Toughness { get; set; }
        public string? Loyalty { get; set; }
        public string? ManaCost { get; set; }
        public string[] SuperTypes { get; set; }
        public string[] Types { get; set; }
        public string[] SubTypes { get; set; }
        public List<CardTextLine> TextLines { get; set; }
    }
}
