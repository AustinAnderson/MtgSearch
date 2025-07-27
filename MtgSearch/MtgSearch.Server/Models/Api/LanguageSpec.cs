namespace MtgSearch.Server.Models.Api
{
    public class LanguageSpec
    {
        public List<FunctionDefinition> FunctionDefinitions { get; set; }
        public ExpressionSpec Expressions { get; set; }
        public ExpressionSpec IsExpressions { get; set; }
    }
}
