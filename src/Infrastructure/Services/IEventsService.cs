namespace Infrastructure.Services
{
    using Infrastructure.Dto.InputModels;

    public interface IEventsService
    {
        Task UpdateAsync(int sportId, EventInputModel model);
    }
}