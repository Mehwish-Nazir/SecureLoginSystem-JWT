using AutoMapper;
using BackeEndAuthentication.Data;
using BackeEndAuthentication.DTO;
using BackeEndAuthentication.Models;
namespace BackeEndAuthentication.AuotoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Users, RegisterUserDTO>().ReverseMap();
            CreateMap<Users, LoginDTO>().ReverseMap();
            CreateMap<Users, RegisterResponseDTO>().ReverseMap();
            CreateMap<Users, LoginResponseDTO>()
    .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.RoleName));
        }
    }
}
