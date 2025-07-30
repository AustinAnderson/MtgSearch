using MtgSearch.Server.Models.Api.BackEnd;
using MtgSearch.Server.Models.Data;
using MtgSearch.Server.Models.Logic.Predicates;
using Newtonsoft.Json;

namespace MtgSearch.Server.Models.Logic
{
    public class ScryfallCardRepository : ICardRepository
    {
        private readonly static string CacheFilePath = Environment.ExpandEnvironmentVariables("%APPDATA%/MtgSearch/Data/");
        private const string TargetBulkDataType = "oracle_cards";
        private readonly HttpClient scryfallClient;
        private List<MtgJsonAtomicCard> cardList;

        public ScryfallCardRepository(HttpClient scryfallClient) {
            this.scryfallClient = scryfallClient;
        }

        public Task<List<MtgJsonAtomicCard>> Search(ColorIdentity colors, ISearchPredicate predicate)
        {
            throw new NotImplementedException();
        }

        public async Task Initialize()
        {
            if(!Directory.Exists(CacheFilePath))
            {
                throw new CardDataFetchException($"no cache folder found at `{CacheFilePath}`");
            }
            var current = Directory.EnumerateFiles(CacheFilePath, "*.json").FirstOrDefault();
            if (current == null)
            {
                throw new CardDataFetchException($"no card data in cache folder found at `{CacheFilePath}`");
            }

            using var fileStream = File.OpenRead(current);
            var jsonParseStream = new JsonParseStream<ScryfallCard>(fileStream);
            jsonParseStream.Read().Where(x=>x.Legalities.Commander.ToLower()=="legal" || x.isPr)
        }

        public async Task<bool> Update()
        {
            if(!Directory.Exists(CacheFilePath)) Directory.CreateDirectory(CacheFilePath);
            var current = Directory.EnumerateFiles(CacheFilePath, "*.json").FirstOrDefault();
            DateTime? lastUpdate = null;
            if(current != null)
            {
                lastUpdate = DateTime.Parse(Path.GetFileNameWithoutExtension(current).Replace(";",":"));
            }
            var bulkListings = await GetResultOrThrow<BulkDataApiResponse>(scryfallClient.GetAsync(""));
            var oracleCardsInfo = bulkListings.Data.FirstOrDefault(x => x.Type.ToLower() == TargetBulkDataType.ToLower());
            if (oracleCardsInfo == null) 
            {
                string context = "unable to serialize bulkData response ";
                try
                {
                    context = JsonConvert.SerializeObject(bulkListings);
                }
                catch(Exception ex)
                {
                    context += $"{ex.GetType().Name}: {ex.Message}";
                }
                //TODO: log.Info(bulkData response) instead of in exception?
                throw new CardDataFetchException($"couldn't find {TargetBulkDataType} in bulk data response {context}");
            }
            if(lastUpdate > oracleCardsInfo.UpdatedAt)
            {
                return false;
            }
            string fileName = DateTime.UtcNow.ToString("O").Replace(":",";")+".json";

            await using var fileStream = File.OpenWrite(Path.Combine(CacheFilePath,fileName));
            await (await GetStreamJsonOrThrow(scryfallClient.GetAsync())).CopyToAsync(fileStream);
            return true;
        }
        private async Task<T> GetResultOrThrow<T>(Task<HttpResponseMessage> response) where T:class
        {
            var resp = await response;
            string content = "(unable to fetch content: ";
            try
            {
                content = await resp.Content.ReadAsStringAsync();
            }
            catch(Exception ex)
            {
                content += $"{ex.GetType().Name} {ex.Message})";
                throw new CardDataFetchException($"got response {resp.StatusCode}: {content}");
            }
            if(!resp.IsSuccessStatusCode)
            {
                throw new CardDataFetchException($"got response {resp.StatusCode}: `{content}`");
            }
            else if (string.IsNullOrEmpty(content))
            {
                throw new CardDataFetchException($"got response {resp.StatusCode} with no content");
            }

            T? data;
            try
            {
                data = JsonConvert.DeserializeObject<T>(content);
                if(data == null)
                {
                    throw new CardDataFetchException($"parsing returned null for `{content}`");
                }
            }
            catch (JsonException ex)
            {
                throw new CardDataFetchException($"parsing returned had exception for `{content}`", ex);
            }
            return data;
        }
        private async Task<Stream> GetStreamJsonOrThrow(Task<HttpResponseMessage> response)
        {
            
        }

    }
}
