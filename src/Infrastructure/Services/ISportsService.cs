namespace Infrastructure.Services
{
    using Infrastructure.InputModels;

    public interface ISportsService
    {
        Task CreateAsync(SportInputModel model);

        Task<bool> IsExistsAsync(int sportId);

        Task UpdateAsync(SportInputModel model);
    }
}
