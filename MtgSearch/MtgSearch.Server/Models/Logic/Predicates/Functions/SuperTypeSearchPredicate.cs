using MtgSearch.Server.Models.Data;

namespace MtgSearch.Server.Models.Logic.Predicates.Functions
{
    public class SuperTypeSearchPredicate: AbstractTypeSearchPredicate
    {
        protected override string[] SelectTypeArray(ServerCardModel card) => card.Supertypes.ToArray();
    }
}
