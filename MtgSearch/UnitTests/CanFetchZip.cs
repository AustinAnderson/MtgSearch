using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class CanFetchZip
    {
        [TestMethod]
        public async Task TryDownload()
        {
            var client = new HttpClient();
            var resp = await client.GetAsync("https://data.scryfall.io/oracle-cards/oracle-cards-20250730090321.json");
            string outputPath = "testOutput.json.zip";
            if (resp.IsSuccessStatusCode)
            {
                using var fileOutStream = File.OpenWrite(outputPath);
                await (await resp.Content.ReadAsStreamAsync()).CopyToAsync(fileOutStream);
            }
            else
            {
                Assert.Fail("didn't work");
            }
            FileInfo fileInfo = new FileInfo(outputPath);
        }
    }
}
