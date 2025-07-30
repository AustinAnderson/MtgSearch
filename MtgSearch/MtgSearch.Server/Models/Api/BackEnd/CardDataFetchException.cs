namespace MtgSearch.Server.Models.Api.BackEnd
{
    public class CardDataFetchException:Exception
    {
        public CardDataFetchException(string message): base(message) { }
        public CardDataFetchException(string message, Exception inner): base(message,inner) { }
    }
}
