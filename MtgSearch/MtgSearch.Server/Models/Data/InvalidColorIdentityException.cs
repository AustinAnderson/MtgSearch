namespace MtgSearch.Server.Models.Data
{
    public class InvalidColorIdentityException : Exception
    {
        public InvalidColorIdentityException(string message): base(message) { }
    }
}
