using AutoMapper;
using LibraryManagement.Application.DTOs.Borrowings;
using LibraryManagement.Contract.Borrowings;
using LibraryManagement.Contract.Commands.Borrowing;
using LibraryManagement.Contract.QueryModels.Borrowings;
using LibraryManagement.Shared;

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
        CreateMap<PagedResult<BorrowingDto>, BorrowingListResponse>()
            .ForMember(dest => dest.Borrowings, opt => opt.Ignore());
        CreateMap<OverdueBooksRequest, BorrowingListResponse>()
            .ForMember(dest => dest.Borrowings, opt => opt.Ignore())
            .ForMember(dest => dest.TotalCount, opt => opt.Ignore());
    }
}
