namespace MtgSearch.Server.Models.Logic.Parsing.Tokens
{
    //credit https://github.com/tomc128/boolean-expression-parser
    public class VariableToken : AbstractToken
    {
        public int Key {  get; }
        public VariableToken(int key)
        {
            Key = key;
        }
        public override string ToString() => Key.ToString();
    }
}
