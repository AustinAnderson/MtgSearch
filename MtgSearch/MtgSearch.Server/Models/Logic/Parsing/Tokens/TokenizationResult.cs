namespace MtgSearch.Server.Models.Logic.Parsing.Tokens
{
    public class TokenizationResult
    {
        public List<AbstractToken> Tokens { get; set; }
        public VariableSubstitutionSet VariableSubstitutionSet { get; set; }
    }
}
