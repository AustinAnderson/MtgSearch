using MtgSearch.Server.Models.Logic.Predicates;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace MtgSearch.Server.Models.Logic.Parsing
{
    //grammar for this part is regular, just use simple loop
    public class FunctionParser
    {
        //loop fullInput, starting at readPos end at eof, at end of everything add pred to dict and return key
        //lookaheadMatch should be the function name and the open paren right after
        public static ISearchPredicate Parse(ref int readPos, string fullInput, string lookaheadMatch)
        {
            var startPos = readPos;
            var functionName = lookaheadMatch;
            if (!Function.ByName.ContainsKey(functionName))
            {
                throw new QueryParseException($"unknown function `{functionName}`, at `...{TryToGetContext(readPos,fullInput)}`");
            }
            //should be at the space before the function start, calling should ignore and tokenize so
            readPos+=lookaheadMatch.Length+1; //to consume function name and (
            if (readPos >=fullInput.Length || fullInput[readPos - 1] != '(')
            {
                throw new QueryParseException($"function call must start with open paren after the name, for function `{functionName}` at `...{TryToGetContext(readPos,fullInput)}`");
            }
            //                                                        ,--------------_____________
            //                                                        v             v              \
            //matching function name + open paren (skipped) -> gobble string --> comma/whitespace -'
            //                                                               \_-> close paren

            var args = new List<string>();
            bool done= false;
            for (; readPos < fullInput.Length && !done; readPos++)
            {
                char c = fullInput[readPos];
                if (c == ',' || c == ' ')
                {
                    continue;
                }
                else if (c == '"')
                {
                    args.Add(ParseString(ref readPos, fullInput).TrimStart('"').TrimEnd('"'));
                }
                else if (c == ')')
                {
                    done = true;
                }
                else
                {
                    throw new QueryParseException($"unexpected symbol `{fullInput[readPos]}` at char {readPos} `...{TryToGetContext(readPos,fullInput)}`");
                }
            }

            return Function.ByName[functionName].Factory(args.ToArray(), TryToGetContext(startPos, fullInput));
        }


        //should enter if lookahead(1)=='"'
        private static string ParseString(ref int readPos, string fullInput)
        {
            StringBuilder charAppend = new StringBuilder();
            char previous = '\0';
            for(; readPos<fullInput.Length; readPos++)
            {
                charAppend.Append(fullInput[readPos]);
                //not the opening " (length>1)
                //not escaped (prev != \ )
                //must be closing "
                if (fullInput[readPos] == '"' && charAppend.Length > 1 && previous != '\\')
                {
                    return charAppend.ToString();
                }
                previous = fullInput[readPos];
            }

            //got to end of string, no closing ", 
            throw new QueryParseException($"missing closing '\"' at `...{TryToGetContext(readPos,fullInput)}`");
            //look ahead 1 for \"
        }
        public static string TryToGetContext(int position, string fullText)
        {
            try
            {
                var start = position - 20;
                if(start < 0) start = 0;
                var subLength = Math.Min(fullText.Length-1, 21);
                return fullText.Substring(start, subLength);
            }
            catch(Exception ex) 
            {
                Console.WriteLine("error fetching context for error: " + ex.Message);
                return "(couldn't fetch context)";
            }
        }
    }
}
