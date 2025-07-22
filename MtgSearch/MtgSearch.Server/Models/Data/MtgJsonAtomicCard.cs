using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace MtgSearch.Server.Models.Data
{

    public class MtgJsonAtomicCard
    {
        public string name;
        public bool isFunny;
        [JsonProperty("colorIdentity")]
        public string[] ColorIdentityStrings
        {
            get => ColorIdentity?.Colors ?? [];
            set => ColorIdentity = new(string.Join("",value));
        }
        [JsonIgnore]
        public ColorIdentity ColorIdentity;
        public string loyalty;
        private string _text;
        public string text
        {
            get => _text;
            set
            {
                _text = value;
                textLines = _text.Split('\n');
                activatedAbilities = textLines.Where(x => x.Trim(':').Contains(':')).Select(x => new ActivatedAbility(x, name)).ToArray();
            }
        }
        [JsonProperty("legalities")]
        public Legality LegalityParse;
        [JsonIgnore]
        public bool IsLegal => LegalityParse?.commander == "Legal";
        public string[] supertypes;
        public string[] types;
        public string[] subtypes;
        public double manaValue;
        public string manaCost;
        public string power;
        public string toughness;
        public string[] keywords;
        public string[] textLines;
        public ActivatedAbility[] activatedAbilities = [];
        public static IEqualityComparer<MtgJsonAtomicCard> EqualityComparer { get; } = new NameEqualityComparer();
        private class NameEqualityComparer : IEqualityComparer<MtgJsonAtomicCard>
        {
            public bool Equals(MtgJsonAtomicCard? x, MtgJsonAtomicCard? y) => x?.name == y?.name;
            public int GetHashCode([DisallowNull] MtgJsonAtomicCard obj) => obj.name.GetHashCode();
        }
        public class Legality
        {
            public string commander;
        }
    }
}
