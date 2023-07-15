namespace Infrastructure.Services
{
    using Infrastructure.Dto.InputModels;

    public interface IBetsService
    {
        Task UpdateAsync(int matchId, IEnumerable<BetInputModel> model);
    }
}