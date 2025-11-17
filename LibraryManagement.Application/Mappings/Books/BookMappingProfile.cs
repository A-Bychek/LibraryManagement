using AutoMapper;
using LibraryManagement.Application.DTOs.Books;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Mappings.Books;

public class BookMappingProfile : Profile
{
    public BookMappingProfile()
    {
        CreateMap<Book, BookDto>()
            .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author != null ? $"{src.Author.FirstName} {src.Author.LastName}" : "Unknown author"))
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : "Unknown category"));
    }
}


