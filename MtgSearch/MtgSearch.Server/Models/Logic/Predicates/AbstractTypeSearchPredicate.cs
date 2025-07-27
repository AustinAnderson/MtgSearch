using MtgSearch.Server.Models.Data;
using MtgSearch.Server.Models.Logic.Highlighting;

namespace MtgSearch.Server.Models.Logic.Predicates
{
    public abstract class AbstractTypeSearchPredicate : ISearchPredicate
    {
        public string? Includes { get; set; }
        public string[]? Any { get; set; }
        public string[]? All { get; set; }
        abstract protected string[] SelectTypeArray(MtgJsonAtomicCard card);
        public bool Apply(MtgJsonAtomicCard card)
        {
            var types=SelectTypeArray(card).Select(x=>x.ToLower());
            if(Includes != null)
            {
                return types.Contains(Includes.ToLower());
            }
            else if(Any != null) 
            {
                return Any.Any(x => types.Contains(x.ToLower()));
            }
            else if(All != null)
            {
                return All.All(x=> types.Contains(x.ToLower()));
            }
            else
            {
                return false;
            }
        }

        public List<Highlighter> FetchHighlighters() => [];
    }
}
