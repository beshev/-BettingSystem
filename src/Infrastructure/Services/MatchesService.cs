namespace Infrastructure.Services
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
    using Infrastructure.Dto.InputModels;
    using Infrastructure.Dto.UpdateModels;
    using Infrastructure.Events;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class MatchesService : IMatchesService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IBetsService _betsService;
        private readonly IEventPublisher _eventPublisher;
        private readonly IMapper _mapper;

        public MatchesService(
            ApplicationDbContext dbContext,
            IBetsService betsService,
            IEventPublisher eventPublisher,
            IEventSubscriber eventSubscriber,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _betsService = betsService;
            _eventPublisher = eventPublisher;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TModel>> GetAllMatches<TModel>()
        {
            // TODO: Should that be UtcNow ?
            var targetDate = DateTime.Now.AddHours(24);
            var matches = await _dbContext.Matches
                .Where(m => m.StartDate >= DateTime.Now && m.StartDate <= targetDate)
                .Include(m => m.Bets)
                .ThenInclude(b => b.Odds)
                .ToListAsync();

            var result = _mapper.Map<IEnumerable<TModel>>(matches);
            return result;
        }

        public async Task<TModel> GetById<TModel>(int id)
        {
            var match = await _dbContext.Matches
                .Where(m => m.Id.Equals(id))
                .ProjectTo<TModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            return match;
        }

        public async Task UpdateAsync(int eventId, IEnumerable<MatchInputModel> model)
        {
            var newMatchesIds = new HashSet<int>(model.Select(m => m.Id));
            var matches = await _dbContext
                .Matches
                .Where(m => m.EventId.Equals(eventId))
                .ToListAsync();

            var oldMatches = matches
                .Where(m => !newMatchesIds.Contains(m.Id))
                .ToList();

            var hiddenMatches = new List<int>();
            foreach (var oldMatch in oldMatches)
            {
                oldMatch.IsActive = false;
                hiddenMatches.Add(oldMatch.Id);
                await _betsService.UpdateAsync(oldMatch.Id, Enumerable.Empty<BetInputModel>());
            }

            var activeMatches = matches
                .Where(o => newMatchesIds.Contains(o.Id))
                .ToList();

            var changedMatches = new List<MatchUpdateModel>();
            foreach (var newMatch in model.Where(m => !m.MatchType.Equals(Models.Enums.MatchType.OutRight)))
            {
                var currentMatch = activeMatches.FirstOrDefault(m => m.Id.Equals(newMatch.Id));
                if (currentMatch is null)
                {
                    var newMatchEntity = _mapper.Map<Match>(newMatch);
                    newMatchEntity.EventId = eventId;

                    await _dbContext.Matches.AddAsync(newMatchEntity);
                }
                else
                {
                    currentMatch.IsActive = true;
                    if (!currentMatch.StartDate.Equals(newMatch.StartDate) || !currentMatch.MatchType.Equals(newMatch.MatchType))
                    {
                        currentMatch.MatchType = newMatch.MatchType;
                        currentMatch.StartDate = newMatch.StartDate;

                        _dbContext.Matches.Update(currentMatch);

                        var updateModel = _mapper.Map<MatchUpdateModel>(currentMatch);
                        changedMatches.Add(updateModel);
                    }

                    await _betsService.UpdateAsync(newMatch.Id, newMatch.Bets);
                }
            }

            await _dbContext.SaveChangesAsync();

            _eventPublisher.TriggerEventForChanges(changedMatches);
            _eventPublisher.TriggerEventForHide(hiddenMatches);
        }
    }
}
