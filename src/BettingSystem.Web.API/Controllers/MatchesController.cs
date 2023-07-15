namespace BettingSystem.Web.API.Controllers
{
    using Infrastructure.Dto.DetailModels;
    using Infrastructure.Dto.ViewModels;
    using Infrastructure.Services;
    using Microsoft.AspNetCore.Mvc;
    using System.Net;

    [ApiController]
    [Route("api/[controller]")]
    public class MatchesController : ControllerBase
    {
        private readonly IMatchesService _matchesService;

        public MatchesController(IMatchesService matchesService)
        {
            _matchesService = matchesService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<MatchViewModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var result = await _matchesService.GetAllMatches<MatchViewModel>();
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MatchDetailsModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _matchesService.GetById<MatchDetailsModel>(id);
            return Ok(result);
        }
    }
}