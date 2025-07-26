using MtgSearch.Server.Models.Logic.Predicates;
using System.Diagnostics.Contracts;

namespace MtgSearch.Server.Models.Logic.Parsing
{
    public class VariableSubstitutionSet
    {
        public int AddNext(ISearchPredicate predicate)
        {
            var next = Latest + 1;
            Substitutions.Add(next, predicate);
            return next;
        }
        public Dictionary<int, ISearchPredicate> Substitutions { get; } = new Dictionary<int,ISearchPredicate>();
        public int Latest
        {
            get
            {
                if (!Substitutions.Any()) return 0;
                return Substitutions.Keys.Max();
            }
        }
    }
}
