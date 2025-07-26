using MtgSearch.Server.Models.Logic.Parsing;
using MtgSearch.Server.Models.Logic.Parsing.Tokens;

namespace UnitTests
{
    [TestClass]
    public class CanParse
    {
        [TestMethod]
        public void ParsesNoParens()
        {
            var query = "type('Crea\\'ture') && (mv > 3 || pow is *) && subType('Kavu') || subType('Human')";//.Replace('\'', '"');
            var tokRes = Tokenizer.Tokenize(query);
            var res = QueryParser.Parse(tokRes);
        }
    }
}