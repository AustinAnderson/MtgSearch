namespace MtgSearch.Server.Models.Api
{
    public class SearchRequest
    {
        public SortCriterion[] SortCriteria { get; set; }
        public string ColorIdentity { get; set; }
        public string Query { get; set; }

    }
}
