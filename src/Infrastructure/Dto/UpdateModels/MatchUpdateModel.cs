namespace Infrastructure.Dto.UpdateModels
{
    public class MatchUpdateModel : BaseUpdateModel
    {
        public Models.Enums.MatchType MatchType { get; set; }

        public DateTime StartDate { get; set; }
    }
}
