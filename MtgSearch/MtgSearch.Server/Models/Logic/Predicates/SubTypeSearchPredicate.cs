using MtgSearch.Server.Models.Data;

namespace MtgSearch.Server.Models.Logic.Predicates
{
    public class SubTypeSearchPredicate : AbstractTypeSearchPredicate
    {
        protected override string[] SelectTypeArray(ServerCardModel card) => card.Subtypes;
    }
}
