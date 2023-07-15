namespace Infrastructure.Services
{
    using AutoMapper;
    using Data;
    using Infrastructure.Dto.InputModels;
    using Infrastructure.Dto.UpdateModels;
    using Infrastructure.Events;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class OddsService : IOddsService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IEventPublisher _eventPublisher;
        private readonly IMapper _mapper;

        public OddsService(
            ApplicationDbContext dbContext,
            IEventPublisher eventPublisher,
            IEventSubscriber eventSubscriber,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _eventPublisher = eventPublisher;
            _mapper = mapper;
        }

        public async Task UpdateAsync(int betId, IEnumerable<OddInputModel> model)
        {
            var newOddsIds = new HashSet<int>(model.Select(m => m.Id));
            var odds = await _dbContext
                .Odds
                .Where(o => o.BetId.Equals(betId))
                .ToListAsync();

            var oldOdds = odds
                .Where(o => !newOddsIds.Contains(o.Id))
                .ToList();

            var hiddenOdds = new List<int>();
            foreach (var oldOdd in oldOdds)
            {
                oldOdd.IsActive = false;
                hiddenOdds.Add(oldOdd.Id);
            }

            var activeOdds = odds
                .Where(o => newOddsIds.Contains(o.Id))
                .ToList();

            var changedOdds = new List<OddUpdateModel>();
            foreach (var newOdd in model)
            {
                var currentOdd = activeOdds.FirstOrDefault(o => o.Id.Equals(newOdd.Id));
                if (currentOdd is null)
                {
                    var newOddEntity = _mapper.Map<Odd>(newOdd);
                    newOddEntity.BetId = betId;

                    await _dbContext.Odds.AddAsync(newOddEntity);
                }
                else
                {
                    currentOdd.IsActive = true;
                    if (!currentOdd.Value.Equals(newOdd.Value))
                    {
                        currentOdd.Value = newOdd.Value;
                        _dbContext.Odds.Update(currentOdd);

                        var updateModel = _mapper.Map<OddUpdateModel>(currentOdd);
                        changedOdds.Add(updateModel);
                    }
                }
            }

            await _dbContext.SaveChangesAsync();

            _eventPublisher.TriggerEventForChanges(changedOdds);
            _eventPublisher.TriggerEventForHide(hiddenOdds); 
        }
    }
}
