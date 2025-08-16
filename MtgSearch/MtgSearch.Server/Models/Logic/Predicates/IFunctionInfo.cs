using MtgSearch.Server.Models.Logic.Parsing;
using System.Text.RegularExpressions;

namespace MtgSearch.Server.Models.Logic.Predicates
{
    public static class ParsibleFunction
    {
        public static Regex ParseRegexOrThrow(string argument, string errorContext, int argNum)
        {
            Regex parsed;
            try
            {
                parsed = new Regex(argument, RegexOptions.IgnoreCase);
            }
            catch(ArgumentException ex)
            {
                throw new QueryParseException($"invalid regex `{argument}` at ...{errorContext}, argument {argNum + 1}", ex);
            }
            return parsed;
        }
    }
    public interface IFunctionInfo
    {
        string ParseAs { get; }
        string[] Signitures { get; }
        string[] Comments { get; }
        string[] Examples { get; }
        public ISearchPredicate Factory(string[] args, string context);
    }
}
