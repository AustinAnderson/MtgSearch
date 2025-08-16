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
        [HttpGet("Lang")]
        public ActionResult GetLanguageSpec()
        {
            var functions = new List<FunctionDefinition>();
            foreach(var func in FunctionList.ByName.Values)
            {
                functions.Add(new FunctionDefinition
                {
                    Name = func.ParseAs,
                    Description = func.Comments,
                    Signitures = func.Signitures,
                    Examples = func.Examples
                });
            }
            List<string> expressions = [
                "where key can be",
            ];
            expressions.AddRange(CollapseDict(CardAttributeType.ByString));
            expressions.Add("and operator can be");
            expressions.AddRange(CollapseDict(Operator.ByString));
            expressions.Add("and number is a 1-4 digit integer");

            List<string> isExpressions = [
                "where key can be"
            ];
            isExpressions.AddRange(CollapseDict(CardAttributeType.ByString));
            isExpressions.Add("and assertion can be");
            isExpressions.Add(CollapseKeys(HasOrIs.ByString));
            isExpressions.Add("and symbol is one of ");
            isExpressions.Add(CollapseKeys(XorStar.ByString));
            return Ok(new LanguageSpec
            {
                FunctionDefinitions = functions,
                Expressions = new ExpressionSpec
                {
                    Name = "Numeric Comparisons",
                    Template = "{key} {operator} {number}",
                    ExplainationText = expressions,
                    Examples = ["mv > 3", "pow == 2", "def > 4"]

                },
                IsExpressions = new ExpressionSpec
                {
                    Name = "Assertions",
                    Template = "{key} {assertion} {symbol}",
                    ExplainationText = isExpressions,
                    Examples = ["pow is *", "def is *", "def has *", "mv has X"]
                }
            });
        }
        private string CollapseKeys<T>(IReadOnlyDictionary<string, T> dict)
        {
            var allKeys = dict.Keys.Select(x => $"`{x}`").ToArray();
            if (allKeys.Length > 1) {
                allKeys[allKeys.Length - 1] = " or " + allKeys[allKeys.Length - 1];
            }
            return string.Join(",",allKeys);
        }
        private List<string> CollapseDict<T>(IReadOnlyDictionary<string, T> dict)
        {
            //byString is Map(symbolKey -> Value Name), multiple keys can map to a single name, like [def or toughness -> Toughness]
            //assume enum's to string gives the correct property name, collapse to 'key or key2: Name'
            return dict.Select(kvp => new { Name = kvp.Value?.ToString(), Symbol = kvp.Key }).GroupBy(x => x.Name).Select(g => string.Join(" or ", g.Select(x=>$"`{x.Symbol}`")) + ": " + g.Key).ToList();
        }
    }
}
