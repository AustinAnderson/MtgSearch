using MtgSearch.Server.Models.Data;
using MtgSearch.Server.Models.Logic.Predicates;
using Newtonsoft.Json;

namespace MtgSearch.Server.Models.Logic
{
    public class FileCardRepository : ICardRepository
    {
        private List<ServerCardModel> cards = [];

        public async Task Initialize(CancellationToken cancellation)
        {
            await Update(cancellation);
        }

        public Task<List<ServerCardModel>> Search(ColorIdentity colors, ISearchPredicate predicate)
        {
            return Task.FromResult(cards.Where(x => x.ColorIdentity.IncludedIn(colors) && predicate.Apply(x)).ToList());
        }

        //eventually repalce with scryfall bulk data api only update if cache older
        public async Task<bool> Update(CancellationToken cancellation)
        {
            var text = await File.ReadAllTextAsync(@"C:\Users\Austi\Downloads\AtomicCards.json\AtomicCards.json");
            var data = JsonConvert.DeserializeObject<ServerCardModel[]>(text);
            data= data.Where(kvp => !kvp.Name.StartsWith("A-")).ToArray();
            return true;
        }
    }
}
