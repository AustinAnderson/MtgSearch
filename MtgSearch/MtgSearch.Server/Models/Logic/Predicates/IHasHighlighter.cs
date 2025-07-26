using System.Text.RegularExpressions;

namespace MtgSearch.Server.Models.Logic.Predicates
{
    public interface IHasHighlighter
    {
        Regex[] Highlighters { get; }
    }
}
