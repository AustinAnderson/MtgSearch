using Microsoft.AspNetCore.Mvc;
using MtgSearch.Server.Models.Api;
using MtgSearch.Server.Models.Logic.Parsing;
using MtgSearch.Server.Models.Logic.Predicates;

namespace MtgSearch.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DocumentationController:ControllerBase
    {
        public DocumentationController() { }
        [HttpGet]
        public ActionResult GetLanguageSpec()
        {
            var functions = new List<FunctionDefinition>();
            foreach(var func in Function.ByName.Values)
            {
                functions.Add(new FunctionDefinition
                {
                    Name = func.Name,
                    Description = func.Comments,
                    Signitures = func.Signitures,
                    Examples = func.Examples
                });
            }
            List<string> expressions = [
                "where key can be",
            ];
            expressions.AddRange(CollapseDict(NumericCardAttributeType.ByString));
            expressions.Add("and operator can be");
            expressions.AddRange(CollapseDict(Operator.ByString));
            expressions.Add("and number is a 1-4 digit integer");
            List<string> isExpressions = [
                "where key can be"
            ];
            isExpressions.AddRange(CollapseDict(PowerOrToughness.ByString));
            return Ok(new LanguageSpec
            {
                FunctionDefinitions = functions,
                Expressions = new ExpressionSpec
                {
                    Template = "{key} {operator} {number}",
                    ExplainationText = expressions
                },
                IsExpressions = new ExpressionSpec
                {
                    Template = "{key} is *",
                    ExplainationText = isExpressions
                }
            });
        }
        private List<string> CollapseDict<T>(IReadOnlyDictionary<string, T> dict)
        {
            //byString is Map(symbolKey -> Value Name), multiple keys can map to a single name, like [def or toughness -> Toughness]
            //assume enum's to string gives the correct property name, collapse to 'key or key2: Name'
            return dict.Select(kvp => new { Name = kvp.Value?.ToString(), Symbol = kvp.Key }).GroupBy(x => x.Name).Select(g => string.Join(" or ", g.Select(x=>$"`{x.Symbol}`")) + ": " + g.Key).ToList();
        }
    }
}
