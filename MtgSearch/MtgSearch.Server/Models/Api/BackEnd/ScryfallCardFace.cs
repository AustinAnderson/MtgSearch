using Newtonsoft.Json;

namespace MtgSearch.Server.Models.Api.BackEnd
{
    public class ScryfallCardFace
    {
        public string Name { get; set; }
        [JsonProperty("oracle_text")]
        public string? Text { get; set; }
        public double Cmc { get; set; }
        [JsonProperty("mana_cost")]
        public string? ManaCost { get; set; }
        public string Power { get; set; }
        public string Toughness { get; set; }
        [JsonProperty("type_line")]
        public string TypeLine { get; set; }
        public string Loyalty { get; set; }
        ///<summary>could be null if it's on parent</summary>
        [JsonProperty("image_uris")]
        public ScryfallCardImageUrls? ImageUrls { get; set; }
    }
}
