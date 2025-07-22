using MtgSearch.Server.Models.Logic.Predicates;

namespace MtgSearch.Server.Models.Logic.Parsing
{
    public class QueryParser
    {
        public ISearchPredicate Parse(string query)
        {
            int depth = 0;
            (char prev, char prev2, char prev3, char prev4, char prev5) = ('\0', '\0', '\0', '\0', '\0');
            foreach(var c in query)
            {
                if (c == '(')
                {
                    if ((prev3, prev2, prev) == ('r', 'e', 'g'))
                    {

                    }
                    depth++;
                }

                prev5 = prev4;
                prev4 = prev3;
                prev3 = prev2;
                prev2 = prev;
                prev = c;
            }
        }
    }
}
