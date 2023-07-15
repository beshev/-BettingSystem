namespace Infrastructure.Services
{
    using AutoMapper;
    using Data;
    using Infrastructure.Dto.InputModels;
    using Infrastructure.Events;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class BetsService : IBetsService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IOddsService _oddsService;
        private readonly IEventPublisher _eventPublisher;
        private readonly IMapper _mapper;

        public BetsService(
            ApplicationDbContext dbContext,
            IOddsService oddsService,
            IEventPublisher eventPublisher,
            IEventSubscriber eventSubscriber,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _oddsService = oddsService;
            _eventPublisher = eventPublisher;
            _mapper = mapper;
        }

        public async Task UpdateAsync(int matchId, IEnumerable<BetInputModel> model)
        {
            var newBetsIds = new HashSet<int>(model.Select(m => m.Id));
            var bets = await _dbContext
                .Bets
                .Where(b => b.MatchId.Equals(matchId))
                .ToListAsync();

            var oldBets = bets
                .Where(o => !newBetsIds.Contains(o.Id))
                .ToList();

            var hiddenBets = new List<int>();
            foreach (var oldBet in oldBets)
            {
                oldBet.IsActive = false;
                hiddenBets.Add(oldBet.Id);
                await _oddsService.UpdateAsync(oldBet.Id, Enumerable.Empty<OddInputModel>());
            }

            var activeBets = bets
                .Where(o => newBetsIds.Contains(o.Id))
                .ToList();

            foreach (var newBet in model)
            {
                var currentBet = activeBets.FirstOrDefault(b => b.Id.Equals(newBet.Id));
                if (currentBet is null)
                {
                    var newBetEntity = _mapper.Map<Bet>(newBet);
                    newBetEntity.MatchId = matchId;

                    await _dbContext.Bets.AddAsync(newBetEntity);
                }
                else
                {
                    currentBet.IsActive = true;
                    await _oddsService.UpdateAsync(newBet.Id, newBet.Odds);
                }
            }

            await _dbContext.SaveChangesAsync();

            _eventPublisher.TriggerEventForHide(hiddenBets);
        }
    }
}
