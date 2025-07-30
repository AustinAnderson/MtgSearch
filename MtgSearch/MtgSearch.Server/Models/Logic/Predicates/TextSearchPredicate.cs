using MtgSearch.Server.Models.Data;
using MtgSearch.Server.Models.Logic.Highlighting;
using System.Text.RegularExpressions;

namespace MtgSearch.Server.Models.Logic.Predicates
{
    public class TextSearchPredicate : ISearchPredicate 
    {
        public TextSearchPredicate(Regex reg) => Regex = reg;
        public Regex Regex { get; }
        public bool Apply(ServerCardModel card)
        {
            if(card.Text == null) return false;
            return Regex.IsMatch(card.Text);
        }
        
        //TODO: what about & !reg("jalsdkfj") ?
        public List<Highlighter> FetchHighlighters() 
            => [new Highlighter { Regex =  Regex, Target = Highlighter.HlTarget.FullText }];
    }
}
