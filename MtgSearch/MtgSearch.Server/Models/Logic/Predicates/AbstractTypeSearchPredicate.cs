using MtgSearch.Server.Models.Data;

namespace MtgSearch.Server.Models.Logic.Predicates
{
    public abstract class AbstractTypeSearchPredicate:ISearchPredicate
    {
        public string? Exact { get; set; }
        public string[]? Any { get; set; }
        public string[]? All { get; set; }
        abstract protected string[] SelectTypeArray(MtgJsonAtomicCard card);
        public bool Apply(MtgJsonAtomicCard card)
        {
            var types=SelectTypeArray(card);
            if(Exact != null)
            {
                return types.Length==1 && types[0] == Exact;
            }
            else if(Any != null) 
            {
                return Any.Any(x => types.Contains(x));
            }
            else if(All != null)
            {
                return All.All(x=> types.Contains(x));
            }
            else
            {
                return false;
            }
        }

    }
}
