using System.Text.RegularExpressions;

namespace MtgSearch.Server.Models.Logic.Parsing.Tokens
{
    public class Tokenizer
    {
        //adapted from https://github.com/tomc128/boolean-expression-parser/blob/main/BooleanExpressionParser/Tokeniser.cs,
        //the idea being we can swap out the functions and comparisons for a variable key,
        //and parse those as we tokenize, and the rest of the parse is just a boolean expression
        //with the variables being the keys to our preparsed dictionary
        public static TokenizationResult Tokenize(string query)
        {
            //pass one and two, pull out the basic expresssions
            var variables = new VariableSubstitutionSet();
            var attributeExpressionParser = new AttributeExpressionParser(variables);
            query = AttributeExpressionParser.AttributeOperatorExpression.Replace(
                query, attributeExpressionParser.ParseAndSubstituteOperatorBased
            );
            query = AttributeExpressionParser.AttributeIsExpression.Replace(
                query, attributeExpressionParser.ParseAndSubstituteStarBased
            );
            //pass three, tokenize with function substitutions
            var tokenizer = new Regex(@"([([{]|[)\]}]|[\w\.]+|&&|\|\||[&!|])\s*");
            var gobbleWhiteSpace = new Regex(@"\s*");
            int i = 0;
            var tokens = new List<AbstractToken>();
            while(i<query.Length)
            {
                var match = tokenizer.Match(query, i);
                if (!match.Success) throw new QueryParseException($"Unrecognized symbol at char {i}, at `...{FunctionParser.TryToGetContext(i, query)}`");
                if (match.Index != i) throw new QueryParseException($"Invalid token (match found at wrong position) at char {i}, at `...{FunctionParser.TryToGetContext(i, query)}`, match `{match.Value}` found at char {match.Index}");
                var token = match.Groups[1].Value;
                int matchNdx = i;
                //if our "variable" is a reserved function name, try parsing it and substituting in
                if (FunctionList.ByName.ContainsKey(token))
                {
                    var pred = FunctionParser.Parse(ref i, query, token);
                    token = variables.AddNext(pred).ToString();
                    var wspace = gobbleWhiteSpace.Match(query, i);
                    if(wspace.Success)
                    {
                        i += wspace.Length;
                    }

                }
                else
                {
                    i += match.Length;
                }
                tokens.Add(token.ToLower() switch
                {
                    "(" or "[" or "{" => new OpenParenToken(),
                    ")" or "]" or "}" => new CloseParenToken(),
                    "and" or "&" or "&&" => new AndOperatorToken(),
                    "or" or "|" or "||" => new OrOperatorToken(),
                    "not" or "!" => new NotOperatorToken(),
                    _ when int.TryParse(token, out var parsed) => new VariableToken(parsed),
                    _ => throw new QueryParseException(GetUnmatchedTokenError(token, matchNdx, query))
                });
            }
            return new TokenizationResult
            {
                Tokens = tokens,
                VariableSubstitutionSet = variables,
            };
        }
        private static string GetUnmatchedTokenError(string token, int index, string query)
        {
            string message = $"invalid token `{token}` at char {index} at `...{FunctionParser.TryToGetContext(index, query)}`";
            string? didYouMean = null;
            try
            {
                int distThreshold = 2;
                foreach (var functionName in FunctionList.ByName.Keys)
                {
                    if (Fastenshtein.Levenshtein.Distance(token, functionName) <= distThreshold)
                    {
                        didYouMean = functionName;
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine($"unable to compute did you mean distance for error; {message}: {e.GetType().Name}, {e.Message}");
            }
            if(didYouMean != null)
            {
                message += $" (did you mean `{didYouMean}`?)";
            }
            return message;
        }

    }
}
