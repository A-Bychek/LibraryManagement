using AutoMapper;
using LibraryManagement.Application.DTOs.Authors;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Mappings.Authors
{
    public class AuthorMappingProfile : Profile
    {
        public AuthorMappingProfile()
        {
            CreateMap<Author, AuthorDto>()
                .ForMember(dest => dest.BookCount, opt => opt.MapFrom(src => src.Books.Count));
        }
    }
}
