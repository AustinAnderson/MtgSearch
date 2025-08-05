using MtgSearch.Server.Models.Api.BackEnd;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;

namespace MtgSearch.Server.Models.Data
{

    public class ServerCardModel
    {
        public static IEnumerable<ServerCardModel> FromScryfall(ScryfallCard jsonCard, DateTime utcNow)
        {
            var card = new ServerCardModel(jsonCard.Name)
            {
                ColorIdentity = new(string.Join("", jsonCard.ColorId)),
                IsLegal = jsonCard.IsLegal,
                IsFunny = jsonCard.IsFunny,
                IsPreRelease = jsonCard.IsPreReleaseAsOf(utcNow),
                SetCode = jsonCard.SetCode
            };
            if(jsonCard.ScryfallCardFaces!=null && jsonCard.ScryfallCardFaces.Length > 0)
            {
                FillData(card, jsonCard, jsonCard.ScryfallCardFaces[0]);
                card.Name = jsonCard.ScryfallCardFaces[0].Name ?? card.Name;
                if (jsonCard.ScryfallCardFaces.Length > 1)
                {
                    card.AltFaceName = jsonCard.ScryfallCardFaces[1].Name;
                    card.AltFaceImageUrl = GetImageUrl(jsonCard.ScryfallCardFaces[1].ImageUrls);
                }
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
                        AltFaceName = jsonCard.ScryfallCardFaces[0].Name,
                        AltFaceImageUrl = GetImageUrl(jsonCard.ScryfallCardFaces[0].ImageUrls)
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
        private static void FillData(ServerCardModel card, ScryfallCard cardData, ScryfallCardFace? faceData)
        {
            FillText(card, faceData?.Text ?? cardData?.Text);
            FillTypes(card, faceData?.TypeLine ?? cardData?.TypeLine);
            card.Loyalty = faceData?.Loyalty ?? cardData?.Loyalty;
            card.Power = faceData?.Power ?? cardData?.Power;
            card.Toughness = faceData?.Toughness ?? cardData?.Toughness;
            card.ManaValue = faceData?.Cmc ?? cardData?.Cmc ?? 0;
            card.ManaCost = faceData?.ManaCost ?? cardData?.ManaCost;
            card.CardImageUrl = GetImageUrl(faceData?.ImageUrls?? cardData?.ImageUrls);
        }
        private static string? GetImageUrl(ScryfallCardImageUrls? urlsObj)
        {
            if (urlsObj == null) return null;
            return urlsObj.Png ?? urlsObj.Large ?? urlsObj.Normal ?? urlsObj.Small;
        }
        private static void FillText(ServerCardModel card, string? text)
        {
            if (text == null) return;
            card.Text = text;
            card.ActivatedAbilities = card.Text.Split(new char[] { '\n', '\r' },StringSplitOptions.RemoveEmptyEntries)
                   .Where(x => x.Trim(':').Contains(':')).Select(x => new ActivatedAbility(x, card.Name)).ToArray();
        }
        const string SubTypesSplitOn = "—";
        private static void FillTypes(ServerCardModel card, string? typeLine)
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
            if (typeSplits.Length > 1)
            {
                card.Subtypes = typeSplits[1].Split(' ');
            }
        }



        public ServerCardModel(string name) 
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
        public string? CardImageUrl { get; private set; }
        public string? AltFaceImageUrl { get; private set; }
        public string? SetCode { get; private set; }
        public ActivatedAbility[] ActivatedAbilities { get; private set; } = [];
        public bool IsPreRelease { get; private set; }
        public static IEqualityComparer<ServerCardModel> EqualityComparer { get; } = new NameEqualityComparer();
        private class NameEqualityComparer : IEqualityComparer<ServerCardModel>
        {
            public bool Equals(ServerCardModel? x, ServerCardModel? y) => x?.Name == y?.Name;
            public int GetHashCode([DisallowNull] ServerCardModel obj) => obj.Name.GetHashCode();
        }
    }
}
