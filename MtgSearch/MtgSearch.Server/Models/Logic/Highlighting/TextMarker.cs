using MtgSearch.Server.Models.Api;
using MtgSearch.Server.Models.Data;
using System.Text.RegularExpressions;

namespace MtgSearch.Server.Models.Logic.Highlighting
{
    public interface ITextMarker
    {
        List<CardTextLine> MarkText(MtgJsonAtomicCard card, List<Highlighter> highlighters);
    }
    public class TextMarker : ITextMarker
    {

        private const char HlStart = '\u3898';//arbitrary codes point outside of what can show up on the card
        private const char HlEnd = '\u3899';//used to mark spots
        private static readonly Regex ManaSymbol = new Regex(@"({(?:[TQ0-9WUBRGCSPXE])(?:/?(?:[TQ0-9WUBRGCSPXE]?))?})", RegexOptions.Compiled);
        private static readonly Regex HlOrSymbol = new Regex(@"([" + HlStart + HlEnd + "])|" + ManaSymbol, RegexOptions.Compiled);
        /// <summary>
        /// tokenizes the string by highlights start stops and symbols
        /// </summary>
        public List<CardTextLine> MarkText(MtgJsonAtomicCard card, List<Highlighter> highlighters)
        {
            if (string.IsNullOrEmpty(card.Text)) return [];
            var text = card.Text;
            //TODO: implement other targets
            var copOut = highlighters.Where(x => x.Target == Highlighter.HlTarget.FullText).Select(x => x.Regex);
            //mark start and end of each hl section matching the regexs,

            //because .* will match our hl markers, just replace the search text each time
            foreach (var highlighter in copOut)
            {
                text = highlighter.Replace(text, x => HlStart.ToString() + x + HlEnd);
            }


            var lines = new List<CardTextLine>();
            int depth = 0;
            //split the string on newline
            //loop through list of lists of strings setting highlight flag based on start marker,
            //removing the markers and ignoring nested while building the new list per line
            foreach (var line in text.Split((char[])['\r', '\n'], StringSplitOptions.RemoveEmptyEntries))
            {
                var segments = new List<CardTextLineSegment>();
                //foreach line, split on symbol keeping it because of the capture group in the regex
                var splits = HlOrSymbol.Split(line);
                foreach (var token in splits)
                {
                    if (token == HlStart.ToString())
                    {
                        depth++;
                    }
                    else if (token == HlEnd.ToString())
                    {
                        depth--;
                    }
                    else if (ManaSymbol.IsMatch(token))
                    {
                        segments.Add(new CardTextLineSegment { Text = token, IsSymbol = true });
                    }
                    else
                    {
                        segments.Add(new CardTextLineSegment { Text = token, IsHighlighted = depth > 0 });
                    }
                }
                //because we could have "text hlStart text hlStart text hlEnd hlEnd",
                //we would get two segments that should be merged
                for (int i = segments.Count-1; i > 0; i--)
                {
                    if (!segments[i - 1].IsSymbol && !segments[i].IsSymbol &&
                        segments[i - 1].IsHighlighted && segments[i].IsHighlighted)
                    {
                        segments[i - 1].Text += segments[i].Text;
                        segments.RemoveAt(i);
                    }
                }
                lines.Add(new CardTextLine { Segments = segments });
            }
            return lines;
        }
    }
}
