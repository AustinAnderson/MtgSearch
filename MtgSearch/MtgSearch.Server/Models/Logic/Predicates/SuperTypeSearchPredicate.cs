using MtgSearch.Server.Models.Data;

namespace MtgSearch.Server.Models.Logic.Predicates
{
    public class SuperTypeSearchPredicate : ISearchPredicate
    {
        public string SuperType { get; set; }
        public bool Apply(MtgJsonAtomicCard card) => card.supertypes.Contains(SuperType);

    }
}
