namespace MtgSearch.Server.Models.Logic.Parsing.Tokens
{
    //credit https://github.com/tomc128/boolean-expression-parser
    public class NotOperatorToken : AbstractOperatorToken
    {
        public NotOperatorToken() : base(3) { }
        public override string ToString() => "!";
    }
}
