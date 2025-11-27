using AutoMapper;
using LibraryManagement.Application.DTOs.Borrowings;
using LibraryManagement.Application.Interfaces.Services;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Mappings.Borrowings;

public class BorrowingMappingProfile : Profile
{
    public IBorrowingService _borrowingService { get; set; }

    BorrowingMappingProfile(IBorrowingService borrowingService)
    {
        _borrowingService = borrowingService;
    }

    public BorrowingMappingProfile()
    {
        CreateMap<Borrowing, BorrowingDto>()
            .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book != null ? src.Book.Title : "Unknown book")) // raise Exception
            .ForMember(dest => dest.ReturnDate,
                opt => opt.MapFrom(src => src.ReturnDate.HasValue
                    ? src.ReturnDate.Value.ToString("yyyy-MM-dd")
                    : string.Empty));
    }
}
