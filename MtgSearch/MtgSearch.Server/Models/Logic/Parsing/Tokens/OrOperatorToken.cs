namespace MtgSearch.Server.Models.Logic.Parsing.Tokens
{
    //credit https://github.com/tomc128/boolean-expression-parser
    public class OrOperatorToken: AbstractOperatorToken
    {
        public OrOperatorToken() : base(1) { }
        public override string ToString() => "||";
    }
}
