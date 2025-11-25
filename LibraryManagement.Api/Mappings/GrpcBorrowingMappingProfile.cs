using AutoMapper;
using LibraryManagement.Application.Commands.Borrowing;
using LibraryManagement.Application.DTOs.Borrowings;
using LibraryManagement.Application.QueryModels.Borrowings;
using LibraryManagement.Contract.Borrowings;

namespace LibraryManagement.Api.Mappings;

public class GrpcBorrowingMappingProfile : Profile
{
    public GrpcBorrowingMappingProfile()
    {
        CreateMap<BorrowingDto, BorrowingResponse>();
        CreateMap<BorrowBookRequest, BorrowBookCommand>();
        CreateMap<ReturnBookRequest, ReturnBookCommand>();
        CreateMap<UserBorrowingsRequest, BorrowingSearchArgs>()
            .ForMember(dest => dest.PageNumber, opt => opt.MapFrom(src => 1))
            .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => 15));
    }
}
