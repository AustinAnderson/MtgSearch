using Newtonsoft.Json;

namespace MtgSearch.Server.Models.Api.BackEnd
{
    public class BulkDataApiResponse
    {
        public BulkDataApiResult[] Data { get; set; }
    }
    public class BulkDataApiResult
    {
        public bool IsOracleSet => Type == "oracle_cards";
        public string Type { get; set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("download_uri")]
        public string DownloadUri { get; set; }
    }
}
