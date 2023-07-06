namespace Infrastructure.Services
{
    using Infrastructure.InputModels;

    public interface ISportsService
    {
        public Task CreateAsync(SportInputModel model);

        public Task<bool> IsExistsAsync(int sportId);

        public Task UpdateAsync(SportInputModel model);
    }
}
