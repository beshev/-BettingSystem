namespace Infrastructure.Services
{
    using AutoMapper;
    using Data;
    using Infrastructure.InputModels;
    using Models;

    public class EventsService : IEventsService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMatchesService _matchesService;
        private readonly IMapper _mapper;

        public EventsService(
            ApplicationDbContext dbContext,
            IMatchesService matchesService,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _matchesService = matchesService;
            _mapper = mapper;
        }

        public async Task UpdateAsync(int sportId, EventInputModel model)
        {
            var currentEvent = await _dbContext
                .Events
                .FindAsync(model.Id);

            if (currentEvent is null)
            {
                var newEvent = _mapper.Map<Event>(model);
                newEvent.SportId = sportId;
                await _dbContext.Events.AddAsync(newEvent);

                await _dbContext.SaveChangesAsync();
            }
            else
            {
                foreach (var eventMatch in model.Matches)
                {
                    await _matchesService.UpdateAsync(model.Id, eventMatch);
                }
            }
        }
    }
}
