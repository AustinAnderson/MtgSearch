using MtgSearch.Server.Models.Data;
using System.Text.RegularExpressions;

namespace MtgSearch.Server.Models.Logic
{
    public class TextMarker
    {
        public const string HighlightStartMarker = "{HighlightStart}";
        public const string HighlightEndMarker = "{HighlightEnd}";
        private const char HlStartMarkerInternal = '\u3898';//arbitrary code point outside of what can show up on the card
        private const char HlEndMarkerInternal = '\u3899';
        public string MarkText(MtgJsonAtomicCard card, Regex[] Highlighters)
        {
            //mark start and end of each section matching the regexs,
            //loop through chars with a depth counter to catch nested highlights and erase them
            //replace internal marker with external one
            //return this to the presentation layer to deal with

            //because .* will match our markers, just replace the search text each time
            foreach()

        }
    }
}
