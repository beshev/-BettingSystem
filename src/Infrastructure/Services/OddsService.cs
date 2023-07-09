namespace Infrastructure.Services
{
    using AutoMapper;
    using Data;
    using Infrastructure.InputModels;
    using Models;

    public class OddsService : IOddsService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public OddsService(
            ApplicationDbContext dbContext,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task UpdateAsync(int betId, OddInputModel model)
        {
            var odds = _dbContext.Odds;

            var currentOdd = odds.FirstOrDefault(o => o.Id.Equals(model.Id));
            if (currentOdd is null)
            {
                var newOdd = _mapper.Map<Odd>(model);
                newOdd.BetId = betId;
                odds.Add(newOdd);
            }
            else
            {
                currentOdd.Value = currentOdd.Value.Equals(model.Value)
                    ? currentOdd.Value
                    : model.Value;
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
