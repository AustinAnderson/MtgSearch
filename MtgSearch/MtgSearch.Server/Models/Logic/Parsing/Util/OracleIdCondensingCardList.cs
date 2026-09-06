using MtgSearch.Server.Models.Api.BackEnd;
using System.Collections;
using System.Collections.Generic;

namespace MtgSearch.Server.Models.Logic.Parsing.Util
{
    //set the SetCode and Release Date to the smallest one if we encounter another object with the same name,
    //which corresponds to another printing
    public class OracleIdCondensingCardList
    {
        private Dictionary<string, ScryfallCard> cardsByName = [];
        private static string GetId(ScryfallCard card) => card.OracleId ?? card.ScryfallCardFaces.FirstOrDefault(x => x.OracleId != null)?.OracleId ?? "";

        public void Add(ScryfallCard newInstance)
        {
            if(cardsByName.TryGetValue(GetId(newInstance), out ScryfallCard? cardWeHave))
            {
                if(cardWeHave != null && newInstance.ReleasedAtDate < cardWeHave.ReleasedAtDate)
                {
                    cardWeHave.ReleasedAt = newInstance.ReleasedAt;
                    cardWeHave.ReleasedAtDate = newInstance.ReleasedAtDate;
                    cardWeHave.SetCode = newInstance.SetCode;
                }
            }
            else
            {
                cardsByName.Add(GetId(newInstance), newInstance);
            }
        }
        public IEnumerable<ScryfallCard> Results => cardsByName.Values;
    }
}
