namespace MtgSearch.Server.Models.Api
{
    public class CardTextLineSegment
    {
        public bool IsHighlighted { get; set; }
        public bool IsSymbol { get; set; }
        public bool IsPlaneswalkerPlaque { get; set; }
        public string Text { get; set; }
    }
}
