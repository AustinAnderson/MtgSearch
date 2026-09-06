using MtgSearch.Server.Models.Api.BackEnd;
using MtgSearch.Server.Models.Data;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace MtgSearch.Server.Models.Api
{
    public class SortableValue
    {
        private static Dictionary<string, SortableValue> lookUp = new();
        private static Regex PascalCaseSplitter = new Regex("([A-Z][^A-Z]*)",RegexOptions.Compiled);
        private SortableValue(IComparer<ServerCardModel> cardSortComparer, [CallerMemberName] string name = "")
        {
            Name = string.Join(" ", PascalCaseSplitter.Split(name).Where(x => !string.IsNullOrWhiteSpace(x)));
            SortComparer = cardSortComparer;
            lookUp.Add(Name, this);
        }
        public static bool TryParse(string name, out SortableValue? value)
        {
            value = null;
            return lookUp.TryGetValue(name, out value);
        }
        public static List<string> ValidValue => lookUp.Keys.ToList();

        public string Name { get; private set; }
        public IComparer<ServerCardModel> SortComparer { get; private set; }

        public static SortableValue ManaValue = new(Comparer<ServerCardModel>.Create((a, b) =>
            Math.Sign(a.ManaValue - b.ManaValue)
        ));

        public static SortableValue Power = new(Comparer<ServerCardModel>.Create((a, b) =>
            Math.Sign(a.GetNumericPower() - b.GetNumericPower())
        ));

        public static SortableValue Toughness = new(Comparer<ServerCardModel>.Create((a, b) =>
            Math.Sign(a.GetNumericToughness() - b.GetNumericToughness())
        ));

        public static SortableValue Loyalty = new(Comparer<ServerCardModel>.Create((a, b) =>
            Math.Sign(a.GetNumericLoyalty() - b.GetNumericLoyalty())
        ));

        public static SortableValue ReleaseDate = new(Comparer<ServerCardModel>.Create((a, b) =>
            Math.Sign(a.ReleasedAt.Ticks - b.ReleasedAt.Ticks)
        ));
    }
}
