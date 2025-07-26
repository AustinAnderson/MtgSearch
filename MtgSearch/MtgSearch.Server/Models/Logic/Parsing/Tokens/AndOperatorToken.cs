namespace MtgSearch.Server.Models.Logic.Parsing.Tokens
{
    //credit https://github.com/tomc128/boolean-expression-parser
    public class AndOperatorToken : AbstractOperatorToken
    {
        public AndOperatorToken() : base(2) { }
        public override string ToString() => "&&";
    }
}
