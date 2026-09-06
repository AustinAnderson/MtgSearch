using MtgSearch.Server.Models.Api.BackEnd;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class TestGzipJsonStream
    {
        [TestMethod]
        public void Debug()
        {
            var path = @"C:\Users\Austi\Downloads\scryfall\default-cards-20260828210538.jsonl.gz";
            //var path = @"C:\Users\Austi\Downloads\scryfall\default-cards-20260828210538.jsonl";
            using var fStream = new FileStream(path, FileMode.Open);
            using var gzStream = new GZipStream(fStream, CompressionMode.Decompress);
            var jsonStream = new JsonLParseStream<ScryfallCard>(gzStream);
            var cts = new CancellationTokenSource();
            var f = jsonStream.Read(cts.Token).Take(2).ToList();
            cts.Cancel();
            int i = 0;
        }
    }
}
