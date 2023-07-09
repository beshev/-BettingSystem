namespace Infrastructure.Services
{
    using AutoMapper;
    using Data;
    using Infrastructure.InputModels;
    using Models;

    public class BetsService : IBetsService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IOddsService _oddsService;
        private readonly IMapper _mapper;

        public BetsService(
            ApplicationDbContext dbContext,
            IOddsService oddsService,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _oddsService = oddsService;
            _mapper = mapper;
        }

        public async Task UpdateAsync(int matchId, BetInputModel model)
        {
            var currentBet = await _dbContext
                .Bets
                .FindAsync(model.Id);

            if (currentBet is null)
            {
                var newBet = _mapper.Map<Bet>(model);
                newBet.MatchId = matchId;
                await _dbContext.Bets.AddAsync(newBet);

                await _dbContext.SaveChangesAsync();
            }
            else
            {
                foreach (var odd in model.Odds)
                {
                    await _oddsService.UpdateAsync(model.Id, odd);
                }
            }
        }
    }
}
