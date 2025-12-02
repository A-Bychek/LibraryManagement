using AutoMapper;
using LibraryManagement.Application.DTOs.Borrowings;
using LibraryManagement.Application.Interfaces.Services;
using LibraryManagement.Contract.Commands.Borrowing;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;

namespace LibraryManagement.Application.Mappings.Borrowings;

public class BorrowingMappingProfile : Profile
{
    public BorrowingMappingProfile()
    {
        CreateMap<Borrowing, BorrowingDto>()
            .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book != null ? src.Book.Title : "Unknown book")) // raise Exception
            .ForMember(dest => dest.ReturnDate,
                opt => opt.MapFrom(src => src.ReturnDate.HasValue
                    ? src.ReturnDate.Value.ToString("yyyy-MM-dd")
                    : string.Empty));
        CreateMap<BorrowBookCommand, Borrowing>()
            .ForMember(dest => dest.BorrowDate,
                opt => opt.MapFrom(src => DateTime.Today.ToString("yyyy-MM-dd")))
            .ForMember(dest => dest.DueDate,
                opt => opt.MapFrom(src => DateTime.Today + TimeSpan.FromDays(src.DaysToReturn)))
            .ForMember(dest => dest.Status,
                opt => opt.MapFrom(src => BorrowingStatus.Active));
    }
}
