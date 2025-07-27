using Microsoft.AspNetCore.Mvc;
using MtgSearch.Server.Models.Api;
using MtgSearch.Server.Models.Data;
using MtgSearch.Server.Models.Logic;
using MtgSearch.Server.Models.Logic.Highlighting;
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
        public async Task<IActionResult> CountSearch([FromBody] SearchRequest request)
        {
            var res = await TryDoSearch(request);
            if (res.Error != null) return res.Error;
            return Ok(res.Matches.Count);
        }
        [HttpPost("RunSearch")]
        public async Task<IActionResult> DoSearch([FromBody] SearchRequest request)
        {
            var res = await TryDoSearch(request);
            if (res.Error != null) return res.Error;
            return Ok(res.Matches.Select(x=>new SearchResultCard(x) { TextLines = textMarker.MarkText(x, res.Highlighters)}).ToList());
        }

        private class SearchResult
        {
            public BadRequestObjectResult? Error { get; set; }
            public List<MtgJsonAtomicCard> Matches { get; set; }
            public List<Highlighter> Highlighters { get; set; }
        }
        private async Task<SearchResult> TryDoSearch(SearchRequest request)
        {
            ColorIdentity colorId;
            try
            {
                colorId = new ColorIdentity(request.ColorIdentity);
            }
            catch(InvalidColorIdentityException ex)
            {
                return new SearchResult { Error = BadRequest(ex.Message) };
            }
            ISearchPredicate predicate;
            try
            {
                predicate = QueryParser.Parse(Tokenizer.Tokenize(request.Query));
            }
            catch(QueryParseException ex)
            {
                return new SearchResult { Error = BadRequest(ex.Message) };
            }
            return new SearchResult
            {
                Highlighters = predicate.FetchHighlighters(),
                Matches = await repo.Search(colorId, predicate)
            };
        }
    }
}
