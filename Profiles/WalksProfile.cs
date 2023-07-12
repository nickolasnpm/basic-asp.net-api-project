//using AutoMapper;

//namespace UdemyProject.Profiles
//{
//    public class WalksProfile: Profile
//    {
//        public WalksProfile()
//        {
//            CreateMap<Models.Domain.WalkDomain, Models.DTO.WalkDTO>()
//                .ForMember(destination => destination.WalkId, options => options.MapFrom(source => source.Id))
//                .ForMember(destination => destination.WalkName, options => options.MapFrom(source => source.Name))
//                .ForMember(destination => destination.WalkLength, options => options.MapFrom(source => source.Length))
//                .ForMember(destination => destination.WalkRegionId, options => options.MapFrom(source => source.RegionId))
//                .ForMember(destination => destination.WalkDifficultyId, options => options.MapFrom(source => source.DifficultyId))
//                .ForMember(destination => destination.WalkRegion, options => options.MapFrom(source => source.Region))
//                .ForMember(destination => destination.WalkDifficulty, options => options.MapFrom(source => source.Difficulty))
//                .ReverseMap();
//        }
//    }
//}