namespace Infrastructure.Mapping
{
    using AutoMapper;
    using Infrastructure.InputModels;
    using Models;

    internal class SportsMappingProfiler : Profile
    {
        public SportsMappingProfiler()
        {
            CreateMap<SportInputModel, Sport>();
            CreateMap<EventInputModel, Event>()
                .ForMember(dest => dest.Matches, 
                    opt => opt.MapFrom(x => x.Matches.Where(m => m.MatchType != Models.Enums.MatchType.OutRight)));
            
            CreateMap<MatchInputModel, Match>();
            CreateMap<BetInputModel, Bet>();
            CreateMap<OddInputModel, Odd>()
                .ForMember(dest => dest.SpecialBetValue,
                    opt => opt.MapFrom(x => string.IsNullOrWhiteSpace(x.SpecialBetValue) 
                        ? default 
                        : double.Parse(x.SpecialBetValue)));
        }
    }
}
