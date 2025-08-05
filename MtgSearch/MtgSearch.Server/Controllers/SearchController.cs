using Microsoft.AspNetCore.Mvc;
using MtgSearch.Server.Models.Api;
using MtgSearch.Server.Models.Data;
using MtgSearch.Server.Models.Logic;
using MtgSearch.Server.Models.Logic.Highlighting;
using MtgSearch.Server.Models.Logic.Parsing;
using MtgSearch.Server.Models.Logic.Parsing.Tokens;
using MtgSearch.Server.Models.Logic.Predicates;

namespace MtgSearch.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly ILogger<SearchController> _logger;
        private readonly ICardRepository repo;
        private readonly ITextMarker textMarker;
        private IActionResult Make503UnavailableResponse()
        {
            var resp = StatusCode(503,
                repo.RepoState switch
                {
                    RepoState.CheckingForUpdate => "Checking for an update from card sources",
                    RepoState.Updating => "Downloading the new list of cards",
                    RepoState.Loading => "Reading the list of cards",
                    RepoState.Ready => "Ready",
                    //TODO: this will mask the 503 with a generic 500, should probably log instead, but would only happen if I make a new state and forget
                    _ => throw new NotImplementedException($"{repo.RepoState} was never handled in {nameof(Make503UnavailableResponse)}")
                }
            );
            Response.Headers.RetryAfter = repo.TimeUntilReadyInSeconds.ToString();
            return resp;
        }
        public SearchController(ILogger<SearchController> logger, ICardRepository repo, ITextMarker textMarker)
        {
            _logger = logger;
            this.repo = repo;
            this.textMarker = textMarker;
        }

        [HttpPost("CheckSearchCount")]
        public async Task<IActionResult> CountSearch([FromBody] SearchRequest request)
        {
            if (repo.RepoState != RepoState.Ready) return Make503UnavailableResponse();
            var res = await TryDoSearch(request);
            if (res.Error != null) return res.Error;
            return Ok(res.Matches.Count);
        }
        [HttpPost("RunSearch")]
        public async Task<IActionResult> DoSearch([FromBody] SearchRequest request)
        {
            if (repo.RepoState != RepoState.Ready) return Make503UnavailableResponse();
            var res = await TryDoSearch(request);
            if (res.Error != null) return res.Error;
            return Ok(res.Matches.Select(x=>new SearchResultCard(x) { TextLines = textMarker.MarkText(x, res.Highlighters)}).ToList());
        }

        private class SearchResult
        {
            public BadRequestObjectResult? Error { get; set; }
            public List<ServerCardModel> Matches { get; set; }
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
