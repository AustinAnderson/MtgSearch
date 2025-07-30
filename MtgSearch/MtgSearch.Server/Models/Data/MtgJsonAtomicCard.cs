using MtgSearch.Server.Models.Api.BackEnd;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;

namespace MtgSearch.Server.Models.Data
{

    public class MtgJsonAtomicCard
    {
        public static IEnumerable<MtgJsonAtomicCard> FromScryfall(ScryfallCard jsonCard)
        {
            bool isFunny = jsonCard.TypeLine.ToLower().Contains("attraction") ||
                (jsonCard.ScryfallCardFaces != null 
                && 
                 jsonCard.ScryfallCardFaces.Any(x => x.TypeLine.ToLower().Contains("attraction")));
            if (!isFunny)
            {
                isFunny &= jsonCard.ManaCost != null && jsonCard.ManaCost.Contains("{TK}");
                isFunny &= jsonCard.Text != null && jsonCard.Text.Contains("{TK}");
                isFunny &= jsonCard.ScryfallCardFaces != null && jsonCard.ScryfallCardFaces.Length > 0
                           && jsonCard.ScryfallCardFaces.Any(x =>
                               x.ManaCost != null && x.ManaCost.Contains("{TK}")
                               ||x.Text != null && x.Text.Contains("{TK}")
                           );
            }

            var card = new MtgJsonAtomicCard(jsonCard.Name)
            {
                ColorIdentity = new(string.Join("", jsonCard.ColorId)),
                IsLegal = jsonCard.Legalities.Commander.ToLower() == "legal",
                IsFunny = isFunny,
                //Assume UTC? not specified in their docs other than yyyy-MM-dd
                IsPreRelease = DateTime.SpecifyKind(DateTime.Parse(jsonCard.releasedAt), DateTimeKind.Utc) > DateTime.UtcNow
            };
            if(jsonCard.ScryfallCardFaces!=null && jsonCard.ScryfallCardFaces.Length > 0)
            {
                FillData(card, jsonCard, jsonCard.ScryfallCardFaces[0]);
                card.Name = jsonCard.ScryfallCardFaces[0].Name ?? card.Name;
                if(jsonCard.ScryfallCardFaces.Length > 1) card.AltFaceName = jsonCard.ScryfallCardFaces[1].Name;
                yield return card;
                if (jsonCard.ScryfallCardFaces.Length > 1)
                {
                    //clone to avoid multiple reference equals obj in enumeration
                    card = new(jsonCard.ScryfallCardFaces[1].Name ?? card.Name)
                    {
                        ColorIdentity = card.ColorIdentity,
                        IsFunny = card.IsFunny,
                        IsLegal = card.IsLegal,
                        IsPreRelease = card.IsPreRelease,
                        AltFaceName = jsonCard.ScryfallCardFaces[0].Name
                    };
                    FillData(card, jsonCard, jsonCard.ScryfallCardFaces[1]);
                    yield return card;
                }
            }
            else
            {
                FillData(card, jsonCard, null);
                yield return card;
            }
        }
        private static void FillData(MtgJsonAtomicCard card, ScryfallCard cardData, ScryfallCardFace? faceData)
        {
            FillText(card, faceData?.Text ?? cardData?.Text);
            FillTypes(card, faceData?.TypeLine ?? cardData?.TypeLine);
            card.Loyalty = faceData?.Loyalty ?? cardData?.Loyalty;
            card.Power = faceData?.Power ?? cardData?.Power;
            card.Toughness = faceData?.Toughness ?? cardData?.Toughness;
            card.ManaValue = faceData?.Cmc ?? cardData?.Cmc ?? 0;
            card.ManaCost = faceData?.ManaCost ?? cardData?.ManaCost;
        }
        private static void FillText(MtgJsonAtomicCard card, string? text)
        {
            if (text == null) return;
            card.Text = text;
            card.ActivatedAbilities = card.Text.Split(new char[] { '\n', '\r' },StringSplitOptions.RemoveEmptyEntries)
                   .Where(x => x.Trim(':').Contains(':')).Select(x => new ActivatedAbility(x, card.Name)).ToArray();
        }
        const string SubTypesSplitOn = "—";
        private static void FillTypes(MtgJsonAtomicCard card, string? typeLine)
        {
            if (typeLine == null) return;
            var typeSplits = typeLine.Split(SubTypesSplitOn);
            card.Supertypes = [];
            card.Subtypes = [];
            foreach (var type in typeSplits[0].Split(' '))
            {
                if (new[] { "basic", "legendary", "snow", "host", "ongoing", "world" }.Contains(type.ToLower()))
                {
                    card.Supertypes.Add(type);
                }
                else
                {
                    card.Types.Add(type);
                }
            }
            if (typeSplits.Length > 0)
            {
                card.Subtypes = typeSplits[1].Split(' ');
            }
        }



        public MtgJsonAtomicCard(string name) 
        { 
            Name = name; 
        }
        public string Name { get; private set; }
        public string? AltFaceName { get; private set; }
        public bool IsFunny {  get; private set; }
        public ColorIdentity ColorIdentity { get; private set; } = new("");
        public bool IsLegal { get; private set; }
        public string? Loyalty { get; private set; }
        public List<string> Supertypes { get; private set; } = [];
        public List<string> Types { get; private set; } = [];
        public string[] Subtypes { get; private set; } = [];
        public string TypeLine => string.Join(
            SubTypesSplitOn, 
            [string.Join(
                " ",
                [string.Join(" ", Supertypes), string.Join(" ",Types)]
            )]
        );
        public double ManaValue { get; private set; }
        public string? ManaCost { get; private set; }
        public string? Power { get; private set; }
        public string? Toughness { get; private set; }
        public string? Text { get; private set; }
        public ActivatedAbility[] ActivatedAbilities { get; private set; } = [];
        public bool IsPreRelease { get; private set; }
        public static IEqualityComparer<MtgJsonAtomicCard> EqualityComparer { get; } = new NameEqualityComparer();
        private class NameEqualityComparer : IEqualityComparer<MtgJsonAtomicCard>
        {
            public bool Equals(MtgJsonAtomicCard? x, MtgJsonAtomicCard? y) => x?.Name == y?.Name;
            public int GetHashCode([DisallowNull] MtgJsonAtomicCard obj) => obj.Name.GetHashCode();
        }
    }
}
