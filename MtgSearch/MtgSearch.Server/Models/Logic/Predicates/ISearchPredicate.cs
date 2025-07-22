using MtgSearch.Server.Models.Data;

namespace MtgSearch.Server.Models.Logic.Predicates
{
    public interface ISearchPredicate
    {
        /// <summary>
        /// returns whether or not this search predicate matches the given card
        /// </summary>
        bool Apply(MtgJsonAtomicCard card);
    }
}
