namespace Infrastructure.Services
{
    using Infrastructure.Dto.InputModels;

    public interface ISportsService
    {
        Task CreateAsync(SportInputModel model);

        Task<bool> IsExistsAsync(int sportId);

        Task UpdateAsync(SportInputModel model);
    }
}
