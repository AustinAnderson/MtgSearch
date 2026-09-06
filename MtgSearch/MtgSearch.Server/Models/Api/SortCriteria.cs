using MtgSearch.Server.Models.Data;
using MtgSearch.Server.Models.Logic.Parsing;
using System.Security.Cryptography.X509Certificates;

namespace MtgSearch.Server.Models.Api
{
    public class SortCriteria: IComparer<ServerCardModel>
    {
        public SortCriteria(SortCriterion[] sortCriteria)
        {
            var parsedCriteria = new List<(SortableValue, bool)>();
            foreach(var aspect in sortCriteria)
            {
                if(!SortableValue.TryParse(aspect.Name,out var val) || val==null)
                {
                    throw new QueryParseException($"Invalid sort key name '{aspect.Name}'");
                }
                parsedCriteria.Add((val!, aspect.IsAscending));
            }
            this.comparer = Comparer<ServerCardModel>.Create((a, b) =>
            {
                foreach (var criterion in parsedCriteria)
                {
                    var res = criterion.Item1.SortComparer.Compare(a, b);
                    if (criterion.Item2) res = res * -1;
                    if (res != 0) return res;
                }
                return 0;
            });
        }
        private IComparer<ServerCardModel> comparer;
        public int Compare(ServerCardModel? x, ServerCardModel? y) => comparer.Compare(x, y);
    }
}
