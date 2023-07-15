namespace Infrastructure.Services
{
    using AutoMapper;
    using Data;
    using Infrastructure.Dto.InputModels;
    using Microsoft.EntityFrameworkCore;
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

        public async Task UpdateAsync(int sportId, IEnumerable<EventInputModel> model)
        {
            //TODO: The most of the services have almost same logic!
            //Maybe there is a way to reduce the duplication of the code

            var newEventsIds = new HashSet<int>(model.Select(m => m.Id));
            var events = await _dbContext
                .Events
                .Where(e => e.SportId.Equals(sportId))
                .ToListAsync();

            var oldEvents = events
                .Where(m => !newEventsIds.Contains(m.Id))
                .ToList();

            foreach (var oldEvent in oldEvents)
            {
                oldEvent.IsActive = false;
                await _matchesService.UpdateAsync(oldEvent.Id, Enumerable.Empty<MatchInputModel>());
            }

            var activeEvents = events
                .Where(m => newEventsIds.Contains(m.Id))
                .ToList();

            foreach (var newEvent in model)
            {
                var currentEvent = activeEvents.FirstOrDefault(e => e.Id.Equals(newEvent.Id));
                if (currentEvent is null)
                {
                    var newEventEntity = _mapper.Map<Event>(newEvent);
                    newEventEntity.SportId = sportId;

                    await _dbContext.Events.AddAsync(newEventEntity);
                }
                else
                {
                    currentEvent.IsActive = true;
                    await _matchesService.UpdateAsync(newEvent.Id, newEvent.Matches);
                }
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
