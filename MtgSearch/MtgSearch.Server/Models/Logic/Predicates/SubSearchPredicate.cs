using MtgSearch.Server.Models.Data;

namespace MtgSearch.Server.Models.Logic.Predicates
{
    public class SubSearchPredicate : ISearchPredicate
    {
        public string SubType { get; set; }
        public bool Apply(MtgJsonAtomicCard card) => card.subtypes.Contains(SubType);
    }
}
