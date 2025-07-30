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

        /// <summary>
        /// yyyy-MM-dd, assuming PT timezone since wotc is in cali?
        /// </summary>
        [JsonProperty("released_at")] 
        public string releasedAt { get; set; }
        /// <summary>
        /// 'legal' or 'not_legal' or 'restricted' or 'banned'
        /// </summary>
        public Legalities Legalities { get; set; }
    }
    public class Legalities
    {
        public string Commander { get; set; }
    }
}
