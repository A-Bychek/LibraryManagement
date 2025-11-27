using AutoMapper;
using LibraryManagement.Application.DTOs.Books;
using LibraryManagement.Contract.Books;
using LibraryManagement.Contract.Commands.Book;
using LibraryManagement.Contract.QueryModels.Books;
using LibraryManagement.Shared;

namespace LibraryManagement.Api.Mappings;

public class GrpcBookMappingProfile : Profile
{
    public GrpcBookMappingProfile()
    {
        CreateMap<BookDto, BookResponse>();
        CreateMap<CreateBookRequest, CreateBookCommand>();
        CreateMap<UpdateBookRequest, UpdateBookCommand>();
        CreateMap<BookSearchRequest, BookSearchArgs>()
            .ForMember(dest => dest.PageNumber, opt => opt.MapFrom(src => src.PageNumber > 0 ? src.PageNumber : 1))
            .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.PageSize > 0 ? src.PageSize : 15));
        CreateMap<DeleteBookDto, DeleteResponse>();
        CreateMap<PagedResult<BookDto>, BookListResponse>()
            .ForMember(dest => dest.Books, opt => opt.Ignore());
    }
}
