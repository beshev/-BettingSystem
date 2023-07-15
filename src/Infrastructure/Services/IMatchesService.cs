namespace Infrastructure.Services
{
    using Infrastructure.Dto.InputModels;

    public interface IMatchesService
    {
        Task<IEnumerable<TModel>> GetAllMatches<TModel>();

        Task<TModel> GetById<TModel>(int id);

        Task UpdateAsync(int eventId, IEnumerable<MatchInputModel> model);
    }
}