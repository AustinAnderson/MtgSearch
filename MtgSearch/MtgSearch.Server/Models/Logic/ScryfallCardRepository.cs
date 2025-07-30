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
        private const string BulkDataEndpoint = "/bulk-data";
        private readonly HttpClient scryfallClient;
        private List<ServerCardModel> cardList = [];

        public ScryfallCardRepository(HttpClient scryfallClient) {
            //TODO: could setup http client registered with container with polly and what-not, yagni
            this.scryfallClient = scryfallClient;
            this.scryfallClient.BaseAddress = new Uri("https://api.scryfall.com");
        }

        public Task<List<ServerCardModel>> Search(ColorIdentity colors, ISearchPredicate predicate)
        {
            if (cardList.Count == 0)
            {
                throw new ArgumentException($"RAM card cache empty, {nameof(Initialize)} was never called");
            }
            return Task.FromResult(cardList.Where(c=>c.ColorIdentity.IncludedIn(colors) && predicate.Apply(c)).ToList());
        }

        public async Task Initialize()
        {
            await Task.Delay(0);
            var now = DateTime.UtcNow;
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
            //TODO: make .Read() return IAsyncEnumerable if we ever call Initialize via api instead of background startup
            cardList = jsonParseStream.Read()
                .Where(x => (x.IsLegal || x.IsPreReleaseAsOf(now)) && !x.IsFunny)
                .SelectMany(jCard => ServerCardModel.FromScryfall(jCard, now))
                .ToList();
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
            var bulkListings = await GetResultOrThrow<BulkDataApiResponse>(scryfallClient.GetAsync(BulkDataEndpoint));
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

            //download uri in bulk data response is fully qualified
            scryfallClient.BaseAddress = null;
            //scryfall docs say they would like a delay between calls to manage load
            await Task.Delay(100);
            await using var fileStream = File.OpenWrite(Path.Combine(CacheFilePath,fileName));
            await (await GetStreamJsonOrThrow(scryfallClient.GetAsync(oracleCardsInfo.DownloadUri))).CopyToAsync(fileStream);
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
            var resp = await response;
            if (!resp.IsSuccessStatusCode)
            {
                string context = "(unable to fetch content: ";
                try
                {
                    context = await resp.Content.ReadAsStringAsync();
                }
                catch(Exception ex)
                {
                    context += $"{ex.GetType().Name}: {ex.Message}";
                }
                throw new CardDataFetchException($"failed to fetch card data from `{resp.RequestMessage?.RequestUri}`, {resp.StatusCode}: {context}");
            }
            //TODO: pretty sure this buffers the response into memory anyway, but revisit if it's a problem
            //apparently extension method resp.Content.ReadFromJsonAsAsyncEnumerable exists,
            //so if performance is an issue, we could filter when fetching and only store filtred list
            return await resp.Content.ReadAsStreamAsync();
        }

    }
}
