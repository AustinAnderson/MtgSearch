using Newtonsoft.Json;

namespace MtgSearch.Server.Models.Api.BackEnd
{
    public class JsonLParseStream<T> where T:class
    {
        private readonly Stream inputStream;

        public JsonLParseStream(Stream inputStream)
        {
            this.inputStream = inputStream;
        }

        public IEnumerable<T> Read(CancellationToken cancellation)
        {
            using var sr = new StreamReader(inputStream);
            int i = 0;
            string? line = null;
            do
            {
                line = sr.ReadLine();
                if (line == null) break;
                if (cancellation.IsCancellationRequested) break;
                var item = JsonConvert.DeserializeObject<T>(line);
                if (item == null)
                {
                    throw new CardDataFetchException($"deserialized json to null for {i}th item in cache stream");
                }
                i++;
                yield return item;
            } while (line != null);
        }
    }

    public class JsonArrayParseStream<T> where T:class
    {
        private readonly Stream inputStream;

        public JsonArrayParseStream(Stream inputStream)
        {
            this.inputStream = inputStream;
        }

        public IEnumerable<T> Read(CancellationToken cancellation)
        {
            using var sr = new StreamReader(inputStream);
            using var reader = new JsonTextReader(sr);
            var ser = new JsonSerializer();
            int i = 0;
            if (!reader.Read() || reader.TokenType != JsonToken.StartArray)
            {
                throw new CardDataFetchException("expected cache to be an array");
            }
            while (reader.Read())
            {
                if (cancellation.IsCancellationRequested) break;
                if (reader.TokenType == JsonToken.EndArray) break;
                var item = ser.Deserialize<T>(reader);
                if(item == null)
                {
                    throw new CardDataFetchException($"deserialized json to null for {i}th item in cache stream");
                }
                i++;
                yield return item;
            }
        }
    }
}
