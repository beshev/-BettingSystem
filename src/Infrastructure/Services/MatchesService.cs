namespace Infrastructure.Services
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
    using Infrastructure.InputModels;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class MatchesService : IMatchesService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IBetsService _betsService;
        private readonly IMapper _mapper;

        public MatchesService(
            ApplicationDbContext dbContext,
            IBetsService betsService,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _betsService = betsService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TModel>> GetAllMatches<TModel>()
        {
            // TODO: Should that be UtcNow ?
            var targetDate = DateTime.Now.AddHours(24);
            var matches = await _dbContext.Matches
                .Where(m => m.StartDate >= DateTime.Now && m.StartDate <= targetDate)
                .ProjectTo<TModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            // TODO: Filter for groups
            return matches;
        }

        public async Task<TModel> GetById<TModel>(int id)
        {
            var match = await _dbContext.Matches
                .Where(m => m.Id.Equals(id))
                .ProjectTo<TModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            return match;
        }

        public async Task UpdateAsync(int eventId, MatchInputModel model)
        {
            var matches = _dbContext.Matches;
            var currentMatch = matches.FirstOrDefault(m => m.Id.Equals(model.Id));
            if (currentMatch is null)
            {
                var newMatch = _mapper.Map<Match>(model);
                newMatch.EventId = eventId;
                matches.Add(newMatch);
            }
            else
            {
                if (!currentMatch.StartDate.Equals(model.StartDate) || !currentMatch.MatchType.Equals(model.MatchType))
                {
                    currentMatch.MatchType = model.MatchType;
                    currentMatch.StartDate = model.StartDate;

                    await _dbContext.SaveChangesAsync();
                }

                var matchBets = model.Bets;
                foreach (var bet in matchBets)
                {
                    await _betsService.UpdateAsync(model.Id, bet);
                }
            }
        }
    }
}
