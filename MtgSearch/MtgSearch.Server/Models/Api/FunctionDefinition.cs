namespace MtgSearch.Server.Models.Api
{
    public class FunctionDefinition
    {
        public string Name { get; set; }
        public string[] Description { get; set; }
        public string[] Signitures { get; set; }
        public string[] Examples { get; set; }
    }
}
