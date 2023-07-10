namespace Infrastructure.Services
{
    using Infrastructure.Dto.InputModels;

    public interface IOddsService
    {
        Task UpdateAsync(int betId, OddInputModel model);
    }
}