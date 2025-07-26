using MtgSearch.Server.Models.Logic.Parsing.Tokens;
using MtgSearch.Server.Models.Logic.Predicates;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace MtgSearch.Server.Models.Logic.Parsing
{
    //credit https://github.com/tomc128/boolean-expression-parser/blob/main/BooleanExpressionParser/Parser.cs
    public class QueryParser
    {
        public static ISearchPredicate Parse(TokenizationResult tokenization)
        {
            //TODO: could make this be Expression<> and compile it for potential speed up instead
            //of executing ISearchPredicate on each card
            return GrowAst(InfixToPostfix(tokenization.Tokens), tokenization.VariableSubstitutionSet);
            
        }
        /// <summary>
        /// Converts a list of tokens in infix notation to a list of tokens in postfix notation.
        /// </summary>
        /// <param name="tokens">The tokens to parse.</param>
        /// <returns>The root node of the expression tree.</returns>
        public static IEnumerable<AbstractToken> InfixToPostfix(IEnumerable<AbstractToken> tokens)
        {
            var output = new Queue<AbstractToken>();
            var stack = new Stack<AbstractToken>();

            foreach (var token in tokens)
            {
                switch (token)
                {
                    case VariableToken:
                        output.Enqueue(token);
                        break;

                    case AbstractOperatorToken op:
                        while ((stack.Count > 0 && stack.Peek() is AbstractOperatorToken && stack.Peek() is not OpenParenToken) &&
                               ((stack.Peek() as AbstractOperatorToken)!.Precedence >= op!.Precedence))
                        {
                            output.Enqueue(stack.Pop());
                        }
                        stack.Push(token);
                        break;

                    case OpenParenToken:
                        stack.Push(token);
                        break;

                    case CloseParenToken:
                        while (stack.Count > 0 && stack.Peek() is not OpenParenToken)
                        {
                            output.Enqueue(stack.Pop());
                        }

                        if (stack.Peek() is OpenParenToken)
                        {
                            stack.Pop();
                            if (stack.Count > 0 && (stack.Peek() is AbstractOperatorToken))
                            {
                                output.Enqueue(stack.Pop());
                            }
                        }
                        break;
                }
            }

            while (stack.Count > 0)
            {
                if (stack.Peek() is OpenParenToken)
                {
                    throw new Exception("OPEN_PAREN on operator stack.");
                }
                output.Enqueue(stack.Pop());
            }
            return output;
        }

        private static ISearchPredicate GrowAst(IEnumerable<AbstractToken> tokens, VariableSubstitutionSet variables)
        {
            var stack = new Stack<ISearchPredicate>();

            foreach (var token in tokens)
            {
                switch (token)
                {
                    case VariableToken var:
                        if (!variables.Substitutions.TryGetValue(var.Key, out var pred))
                        {
                            throw new KeyNotFoundException($"unregistered key for variable token `{var.Key}`");
                        }
                        stack.Push(pred);
                        break;

                    case NotOperatorToken:
                        if (stack.Count < 1) throw new ArgumentException($"1 parameter needed for operator ${token}");
                        stack.Push(new PredicateNegation { Predicate = stack.Pop() });
                        break;

                    // All other operators
                    case AbstractOperatorToken:
                        if (stack.Count < 2) throw new ArgumentException($"2 parameters needed for operator ${token}");

                        ISearchPredicate node = token switch
                        {
                            AndOperatorToken => new PredicateAndCombination { Predicates = [stack.Pop(), stack.Pop()] },
                            OrOperatorToken => new PredicateOrCombination { Predicates = [stack.Pop(), stack.Pop()] },
                            _ => throw new NotImplementedException($"Unknown operator `{token}`")
                        };

                        stack.Push(node);
                        break;
                }
            }

            if (stack.Count != 1)
            {
                throw new QueryParseException($"Expression invalid - stack not empty. Stack: {String.Join(", ", stack)}");
            }

            var root = stack.Pop();

            return root;
        }
    }
}
