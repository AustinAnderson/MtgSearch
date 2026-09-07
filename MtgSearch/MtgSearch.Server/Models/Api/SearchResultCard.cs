using MtgSearch.Server.Models.Data;

namespace MtgSearch.Server.Models.Api
{
    public class SearchResultCard
    {
        public SearchResultCard() { }
        public SearchResultCard(ServerCardModel card) 
        {
            Name = card.Name;
            Power = card.Power;
            Toughness = card.Toughness;
            Loyalty = card.Loyalty;
            ManaCost = card.ManaCost;
            SuperTypes = [.. card.Supertypes];
            Types = [.. card.Types];
            SubTypes = card.Subtypes;
            IsPreRelease = card.IsPreRelease;
            ImageUrl = card.CardImageUrl;
            AltImageUrl = card.AltFaceImageUrl;
            AltFaceName = card.AltFaceName;
            ColorId = card.ColorIdentity.Colors;
            SetCode = card.SetCode??"---";
            //expressed from server as a date with no time (utc midnight), so don't localize or let ui localize
            ReleaseDate = card.ReleasedAt.ToString("yyyy-MM-dd");
        }
        public string Name { get; set; }
        public string? Power { get; set; }
        public string? Toughness { get; set; }
        public string? Loyalty { get; set; }
        public string? ManaCost { get; set; }
        public string[] SuperTypes { get; set; }
        public string[] Types { get; set; }
        public string[] SubTypes { get; set; }
        public string[] ColorId { get; set; }
        public bool IsPreRelease { get; set; }
        public string? ImageUrl { get; set; }
        public string? AltFaceName { get; set; }
        public string? AltImageUrl { get; set; }
        public string SetCode { get; set; }
        public string ReleaseDate { get; set; }
        public List<CardTextLine> TextLines { get; set; }
    }
}
