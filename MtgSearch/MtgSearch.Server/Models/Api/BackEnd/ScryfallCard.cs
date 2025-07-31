using Newtonsoft.Json;

namespace MtgSearch.Server.Models.Api.BackEnd
{
    public class ScryfallCard
    {
        [JsonProperty("card_faces")] 
        public ScryfallCardFace[] ScryfallCardFaces { get; set; }
        [JsonProperty("cmc")] 
        public double Cmc { get; set; }
        [JsonProperty("color_identity")] 
        public string[] ColorId { get; set; }
        public string Loyalty { get; set; }
        [JsonProperty("mana_cost")] 
        public string? ManaCost { get; set; }
        public string Name { get; set; }
        [JsonProperty("oracle_text")] 
        public string? Text { get; set; }

        public string Power { get; set; }
        public string Toughness { get; set; }
        [JsonProperty("type_line")] 
        public string TypeLine { get; set; }
        ///<summary>could be null if it's on parent</summary>
        [JsonProperty("image_uris")] 
        public ScryfallCardImageUrls? ImageUrls { get; set; }

        [JsonProperty("set")]
        public string? SetCode { get; set; }

        /// <summary>
        /// yyyy-MM-dd, Assume UTC? not specified in their docs
        /// </summary>
        [JsonProperty("released_at")] 
        public string ReleasedAt { get; set; }
        /// <summary>
        /// 'legal' or 'not_legal' or 'restricted' or 'banned'
        /// </summary>
        public Legalities Legalities { get; set; }
        [JsonIgnore]
        public bool IsFunny
        {
            get {
                bool isFunny = TypeLine.ToLower().Contains("attraction") ||
                    (ScryfallCardFaces != null 
                    && 
                     ScryfallCardFaces.Any(x => x.TypeLine.ToLower().Contains("attraction")));
                if (!isFunny)
                {
                    isFunny |= ManaCost != null && ManaCost.Contains("{TK}");
                    isFunny |= Text != null && Text.Contains("{TK}");
                    isFunny |= ScryfallCardFaces != null && ScryfallCardFaces.Length > 0
                               && ScryfallCardFaces.Any(x =>
                                   x.ManaCost != null && x.ManaCost.Contains("{TK}")
                                   ||x.Text != null && x.Text.Contains("{TK}")
                               );
                }
                return isFunny;
            }
        }
        [JsonIgnore]
        public bool IsLegal => Legalities.Commander.ToLower() == "legal";
        public bool IsPreReleaseAsOf(DateTime utcDate) =>
                DateTime.SpecifyKind(DateTime.Parse(ReleasedAt), DateTimeKind.Utc) > utcDate;
    }
    public class Legalities
    {
        public string Commander { get; set; }
    }
}
