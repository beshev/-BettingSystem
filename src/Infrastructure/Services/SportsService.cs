namespace Infrastructure.Services
{
    using AutoMapper;
    using Data;
    using Infrastructure.InputModels;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using System.Collections.Generic;

    public class SportsService : ISportsService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public SportsService(
            ApplicationDbContext dbContext,
            IEventsService eventsService,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task CreateAsync(SportInputModel model)
        {
            var sport = _mapper.Map<Sport>(model);

            await _dbContext.Sports.AddAsync(sport);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> IsExistsAsync(int sportId)
        {
            return await _dbContext.Sports.AnyAsync(x => x.Id.Equals(sportId));
        }

        public async Task UpdateAsync(SportInputModel model)
        {
            await _dbContext.Database.BeginTransactionAsync();

            var sport = await _dbContext.Sports.FindAsync(model.Id);
            _dbContext.Sports.Remove(sport);
            _dbContext.SaveChanges();

            var newSportRecord = _mapper.Map<Sport>(model);
            _dbContext.Sports.Add(newSportRecord);
            _dbContext.SaveChanges();

            await _dbContext.Database.CommitTransactionAsync();

            CheckForMatchesChanges(sport, newSportRecord);
        }

        private void CheckForMatchesChanges(Sport sport, Sport newSportRecord)
        {
            var oldMatches = sport.Events.SelectMany(e => e.Matches);
            var newMatches = newSportRecord.Events.SelectMany(e => e.Matches);
            foreach (var oldMatch in oldMatches)
            {
                var currentMatch = newMatches.FirstOrDefault(m => m.Id.Equals(oldMatch.Id));

                if (currentMatch is null)
                {
                    // To be hidden
                    ;
                }
                else
                {
                    if (!currentMatch.StartDate.Equals(oldMatch.StartDate)
                     || !currentMatch.MatchType.Equals(oldMatch.MatchType))
                    {
                        // Send notification
                        ;
                    }

                    CheckForBetsChanges(oldMatch.Bets, currentMatch.Bets);
                }
            }
        }

        private void CheckForBetsChanges(IEnumerable<Bet> oldBets, IEnumerable<Bet> newBets)
        {
            foreach (var oldBet in oldBets)
            {
                var currentBet = newBets.FirstOrDefault(m => m.Id.Equals(oldBet.Id));
                if (currentBet is null)
                {
                    // To be hidden
                    ;
                }
                else
                {
                    if (!currentBet.IsLive.Equals(oldBet.IsLive))
                    {
                        // Send notification
                        ;
                    }

                    CheckForOddsChanges(oldBet.Odds, currentBet.Odds);
                }
            }
        }

        private void CheckForOddsChanges(IEnumerable<Odd> oldOdds, IEnumerable<Odd> newOdds)
        {
            foreach (var oldOdd in oldOdds)
            {
                var currentOdd = newOdds.FirstOrDefault(m => m.Id.Equals(oldOdd.Id));
                if (currentOdd is null)
                {
                    // To be hidden
                    ;
                }
                else
                {
                    if (!currentOdd.Value.Equals(oldOdd.Value))
                    {
                        // Send notification
                        ;
                    }
                }
            }
        }
    }
}
