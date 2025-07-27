using MtgSearch.Server.Models.Data;
using MtgSearch.Server.Models.Logic.Predicates;
using Newtonsoft.Json;

namespace MtgSearch.Server.Models.Logic
{
    public class FileCardRepository : ICardRepository
    {
        private List<MtgJsonAtomicCard> cards = [];
        public Task<List<MtgJsonAtomicCard>> Search(ColorIdentity colors, ISearchPredicate predicate)
        {
            return Task.FromResult(cards.Where(x => x.ColorIdentity.IncludedIn(colors) && predicate.Apply(x)).ToList());
        }

        //eventually repalce with scryfall bulk data api only update if cache older
        public async Task<bool> Update()
        {
            var text = await File.ReadAllTextAsync(@"C:\Users\Austi\Downloads\AtomicCards.json\AtomicCards.json");
            var data = JsonConvert.DeserializeObject<MtgJsonCardData>(text);
            data.data = new(data.data.Where(kvp => !kvp.Key.StartsWith("A-")));
            cards = data.data.Values.SelectMany(x=>x).Where(x=>x.IsLegal && !x.isFunny).ToList();
            return true;
        }
    }
}
