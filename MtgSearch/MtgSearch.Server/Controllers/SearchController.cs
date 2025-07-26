using Microsoft.AspNetCore.Mvc;
using MtgSearch.Server.Models.Api;
using MtgSearch.Server.Models.Data;
using MtgSearch.Server.Models.Logic;
using MtgSearch.Server.Models.Logic.Parsing;
using MtgSearch.Server.Models.Logic.Parsing.Tokens;
using MtgSearch.Server.Models.Logic.Predicates;
using System.Text.RegularExpressions;

namespace MtgSearch.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly ILogger<SearchController> _logger;
        private readonly ICardRepository repo;
        private readonly ITextMarker textMarker;

        public SearchController(ILogger<SearchController> logger, ICardRepository repo, ITextMarker textMarker)
        {
            _logger = logger;
            this.repo = repo;
            this.textMarker = textMarker;
        }

        [HttpPost("CheckSearchCount")]
        public IActionResult CountSearch([FromBody] SearchRequest request)
        {
            var error = TryDoSearch(request, out var result);
            if (error != null) return error;
            return Ok(result.Count);
        }
        [HttpPost("RunSearch")]
        public IActionResult DoSearch([FromBody] SearchRequest request)
        {
            var error = TryDoSearch(request, out var result);
            if (error != null) return error;
            List<Regex> highlighters = new List<Regex>();
            return Ok(result.Select(x=>new SearchResultCard(x) { TextLines = textMarker.MarkText(x,highlighters)}).ToList());
        }
        private BadRequestObjectResult? TryDoSearch(SearchRequest request, out List<MtgJsonAtomicCard> cards)
        {
            cards = [];
            ColorIdentity colorId;
            try
            {
                colorId = new ColorIdentity(request.ColorIdentity);
            }
            catch(InvalidColorIdentityException ex)
            {
                return BadRequest(ex.Message);
            }
            ISearchPredicate predicate;
            try
            {
                predicate = QueryParser.Parse(Tokenizer.Tokenize(request.Query));
            }
            catch(QueryParseException ex)
            {
                return BadRequest(ex.Message);
            }
            cards = repo.Search(colorId, predicate);
            return null;
        }
    }
}
