namespace Infrastructure.Services
{
    using Infrastructure.Dto.InputModels;

    public interface IBetsService
    {
        Task UpdateAsync(int matchId, BetInputModel model);
    }
}