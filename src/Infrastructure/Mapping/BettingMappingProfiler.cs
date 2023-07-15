namespace Infrastructure.Mapping
{
    using AutoMapper;
    using Infrastructure.Constants;
    using Infrastructure.Dto.InputModels;
    using Infrastructure.Dto.UpdateModels;
    using Infrastructure.Dto.ViewModels;
    using Models;

    internal class BettingMappingProfiler : Profile
    {
        public BettingMappingProfiler()
        {
            // Input models
            CreateMap<SportInputModel, Sport>();
            CreateMap<EventInputModel, Event>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.Matches,
                    opt => opt.MapFrom(x => x.Matches.Where(m => m.MatchType != Models.Enums.MatchType.OutRight)));

            CreateMap<MatchInputModel, Match>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));

            CreateMap<BetInputModel, Bet>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));

            CreateMap<OddInputModel, Odd>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));

            // View models
            CreateMap<Match, MatchViewModel>()
                .ForMember(dest => dest.Bets,
                                   opt => opt.MapFrom(
                                         src => src.Bets.Where(b => b.Name.Equals(BettingSystemCommonConstants.NameOfMatchWinner)
                                                            || b.Name.Equals(BettingSystemCommonConstants.NameOfMapAdvantage)
                                                            || b.Name.Equals(BettingSystemCommonConstants.NameOfTotalMapsPlayed))));

            CreateMap<Odd, OddViewModel>();
            CreateMap<Bet, BetViewModel>()
                .AfterMap((src, dest) =>
                {
                    dest.Odds = dest.Odds.Any(o => o.SpecialBetValue != null)
                        ? dest.Odds.GroupBy(o => o.SpecialBetValue).FirstOrDefault().ToList()
                        : dest.Odds;
                });

            CreateMap<Match, MatchDetailsModel>()
                .ForMember(dest => dest.ActiveBets, opt => opt.MapFrom(src => src.Bets.Where(b => b.IsLive)))
                .ForMember(dest => dest.InActiveBets, opt => opt.MapFrom(src => src.Bets.Where(b => !b.IsLive)));

            // Update models
            CreateMap<Match, MatchUpdateModel>();
            CreateMap<Bet, BetUpdateModel>();
            CreateMap<Odd, OddUpdateModel>();
        }
    }
}
