namespace Infrastructure.Services
{
    using AutoMapper;
    using Data;
    using Infrastructure.Dto.InputModels;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class SportsService : ISportsService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IEventsService _eventsService;
        private readonly IMapper _mapper;

        public SportsService(
            ApplicationDbContext dbContext,
            IEventsService eventsService,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _eventsService = eventsService;
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
            await _eventsService.UpdateAsync(model.Id, model.Events);
        }
    }
}
