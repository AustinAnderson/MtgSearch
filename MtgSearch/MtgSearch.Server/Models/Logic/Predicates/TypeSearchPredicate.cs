using MtgSearch.Server.Models.Data;

namespace MtgSearch.Server.Models.Logic.Predicates
{
    public class TypeSearchPredicate : ISearchPredicate
    {
        public string Type { get; set; }
        public bool Apply(MtgJsonAtomicCard card) => card.types.Contains(Type);
    }
}
