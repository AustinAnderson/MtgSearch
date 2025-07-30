using Newtonsoft.Json;

namespace MtgSearch.Server.Models.Api.BackEnd
{
    public class JsonParseStream<T> where T:class
    {
        private readonly Stream inputStream;

        public JsonParseStream(Stream inputStream)
        {
            this.inputStream = inputStream;
        }

        public IEnumerable<T> Read()
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
