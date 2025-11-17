using AutoMapper;
using LibraryManagement.Application.DTOs.Borrowings;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Mappings.Borrowings;

public class BorrowingMappingProfile : Profile
{
    public BorrowingMappingProfile()
    {
        CreateMap<Borrowing, BorrowingDto>()
            .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book != null ? src.Book.Title : "Unknown book")) // raise Exception
            .ForMember(dest => dest.FineAmount, opt => opt.MapFrom(src => 0));
    }
}


