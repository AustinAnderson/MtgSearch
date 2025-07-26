namespace MtgSearch.Server.Models.Logic.Parsing
{
    public class QueryParseException:Exception
    {
        public QueryParseException(string message):base(message) { }
        public QueryParseException(string message,Exception inner):base(message,inner) { }
    }
}
