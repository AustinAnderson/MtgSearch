namespace MtgSearch.Server.Models.Api
{
    public class ExpressionSpec
    {
        public string Name { get; set; }
        public string Template { get; set; }
        public List<string> ExplainationText { get; set; }
        public List<string> Examples { get; set; }
    }
}
