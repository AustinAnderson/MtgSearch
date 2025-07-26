using MtgSearch.Server.Models.Data;

namespace MtgSearch.Server.Models.Logic.Predicates
{
    public class SuperTypeSearchPredicate: AbstractTypeSearchPredicate
    {
        protected override string[] SelectTypeArray(MtgJsonAtomicCard card) => card.supertypes;
    }
}
