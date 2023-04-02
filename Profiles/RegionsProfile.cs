using AutoMapper;

namespace UdemyProject.Profiles
{
    public class RegionsProfile: Profile
    {
        // transfter data from source to the destination
        public RegionsProfile()
        {
            CreateMap<Models.Domain.RegionDomain, Models.DTO.RegionDTO>()
                .ForMember(destination => destination.RegionId, options => options.MapFrom(source => source.Id))
                .ForMember(destination => destination.RegionCode, options => options.MapFrom(source => source.Code))
                .ForMember(destination => destination.RegionName, options => options.MapFrom(source => source.Name))
                .ForMember(destination => destination.RegionArea, options => options.MapFrom(source => source.Area))
                .ForMember(destination => destination.RegionLat, options => options.MapFrom(source => source.Lat))
                .ForMember(destination => destination.RegionLong, options => options.MapFrom(source => source.Long))
                .ForMember(destination => destination.RegionPopulation, options => options.MapFrom(source => source.Population))
                .ReverseMap();
        }
    }
}