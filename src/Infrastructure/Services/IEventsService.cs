namespace Infrastructure.Services
{
    using Infrastructure.InputModels;

    public interface IEventsService
    {
        Task UpdateAsync(int sportId, EventInputModel model);
    }
}