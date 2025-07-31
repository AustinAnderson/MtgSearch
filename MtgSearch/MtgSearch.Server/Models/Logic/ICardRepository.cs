using MtgSearch.Server.Models.Data;
using MtgSearch.Server.Models.Logic.Predicates;

namespace MtgSearch.Server.Models.Logic
{
    public interface ICardRepository
    {
        Task<List<ServerCardModel>> Search(ColorIdentity colors, ISearchPredicate predicate);
        Task<bool> Update(CancellationToken cancellation);
        Task Initialize(CancellationToken cancellation);
    }
}
