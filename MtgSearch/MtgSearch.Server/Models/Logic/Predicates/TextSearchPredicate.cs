using MtgSearch.Server.Models.Data;
using System.Text.RegularExpressions;

namespace MtgSearch.Server.Models.Logic.Predicates
{
    public class TextSearchPredicate : ISearchPredicate, IHasHighlighter
    {
        public TextSearchPredicate(Regex reg) => Regex = reg;
        public Regex Regex { get; }
        public Regex[] Highlighters => [Regex];//TODO: what about & !reg("jalsdkfj") ?
        public bool Apply(MtgJsonAtomicCard card) => Regex.IsMatch(card.text);
    }
}
