namespace Infrastructure.Services
{
    using Infrastructure.InputModels;

    public interface IOddsService
    {
        Task UpdateAsync(int betId, OddInputModel model);
    }
}