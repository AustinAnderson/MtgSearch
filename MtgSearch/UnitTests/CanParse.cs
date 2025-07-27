using MtgSearch.Server.Models.Logic.Parsing;
using MtgSearch.Server.Models.Logic.Parsing.Tokens;

namespace UnitTests
{
    [TestClass]
    public class CanParse
    {
        [TestMethod]
        public void ParsesBasicFuncs()
        {
            var query = "type('Crea\\'ture') && (mv > 3 || pow is *) && subType('Kavu') || subType('Human')";//.Replace('\'', '"');
            var tokRes = Tokenizer.Tokenize(query);
            var res = QueryParser.Parse(tokRes);
        }
        public void ParsesDotFuncs()
        {
            var query = "mv > 3 && type.any('Crea\\'ture')";
            var tokRes = Tokenizer.Tokenize(query);
            var res = QueryParser.Parse(tokRes);
        }

    }
}