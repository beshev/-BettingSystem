namespace Infrastructure.Services
{
    using Infrastructure.InputModels;

    public interface IBetsService
    {
        Task UpdateAsync(int matchId, BetInputModel model);
    }
}