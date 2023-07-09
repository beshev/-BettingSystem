namespace Infrastructure.Services
{
    using Infrastructure.InputModels;

    public interface IMatchesService
    {
        Task<IEnumerable<TModel>> GetAllMatches<TModel>();

        Task<TModel> GetById<TModel>(int id);

        Task UpdateAsync(int eventId, MatchInputModel model);
    }
}