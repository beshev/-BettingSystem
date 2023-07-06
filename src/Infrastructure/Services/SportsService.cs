using AutoMapper;
using Data;
using Infrastructure.InputModels;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Infrastructure.Services
{
    public class SportsService : ISportsService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public SportsService(
            ApplicationDbContext dbContext,
            IMapper mapper)
        {
            _dbContext = dbContext;
            this._mapper = mapper;
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
            // TODO: Finish here!
            //var sport = await _dbContext.Sports.FirstOrDefaultAsync(x => x.Id.Equals(model.Id));
            //foreach (var currentEvent in model.Events)
            //{
            //    var @event = await _dbContext.Events.FirstOrDefaultAsync(x => x.Id.Equals(currentEvent.Id));
            //    if (@event is not null)
            //    {
                   
            //    }
            //    else
            //    {
            //        var newEvent = model.Events.Select
            //        sport.Events.Add()
            //    }
            //}
        }
    }
}
