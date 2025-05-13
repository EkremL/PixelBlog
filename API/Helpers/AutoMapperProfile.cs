using AutoMapper;
using API.DTOs;
using API.Entities;

namespace API.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<BlogCreateDto, Blog>(); // Maps incoming Create requests (DTO) to the Blog entity.
            CreateMap<BlogUpdateDto, Blog>(); // Maps incoming Update requests (DTO) to the Blog entity.
            CreateMap<Blog, BlogCreateDto>(); // Maps a Blog entity to BlogCreateDto (useful for returning data if needed).
            CreateMap<Blog, BlogUpdateDto>(); // Maps a Blog entity to BlogUpdateDto (also useful for returning editable data).
            CreateMap<Blog, BlogDto>().ReverseMap();
            CreateMap<UserRegisterDto, User>();
            CreateMap<User, UserDto>();
            CreateMap<Blog, BlogDto>()
            .ForMember(dest => dest.AuthorUsername, opt => opt.MapFrom(src => src.User.Username))
            .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.User.Name))
            .ForMember(dest => dest.AuthorSurname, opt => opt.MapFrom(src => src.User.Surname))
            .ForMember(dest => dest.AuthorAvatar, opt => opt.MapFrom(src => src.User.AvatarUrl));
        }
    }
}
