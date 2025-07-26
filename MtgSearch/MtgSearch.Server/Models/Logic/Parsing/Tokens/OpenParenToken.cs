namespace MtgSearch.Server.Models.Logic.Parsing.Tokens
{
    //credit https://github.com/tomc128/boolean-expression-parser
    public class OpenParenToken : AbstractToken
    {
        public override string ToString() => "(";
    }
}
