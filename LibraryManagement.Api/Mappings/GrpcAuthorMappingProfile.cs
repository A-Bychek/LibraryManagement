using AutoMapper;
using LibraryManagement.Application.DTOs.Authors;
using LibraryManagement.Contract.Authors;
using LibraryManagement.Contract.Commands.Author;
using LibraryManagement.Contract.QueryModels.Authors;
using LibraryManagement.Shared;

namespace LibraryManagement.Api.Mappings;

public class GrpcAuthorMappingProfile : Profile
{
    public GrpcAuthorMappingProfile()
    {
        CreateMap<AuthorDto, AuthorResponse>();
        CreateMap<CreateAuthorRequest, CreateAuthorCommand>();
        CreateMap<UpdateAuthorRequest, UpdateAuthorCommand>();
        CreateMap<AuthorSearchRequest, AuthorSearchArgs>()
            .ForMember(dest => dest.PageNumber, opt => opt.MapFrom(src => src.PageNumber > 0 ? src.PageNumber : 1))
            .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.PageSize > 0 ? src.PageSize : 15));
        CreateMap<PagedResult<AuthorDto>, AuthorListResponse>()
            .ForMember(dest => dest.Authors, opt => opt.Ignore());
        CreateMap<DeleteAuthorDto, DeleteResponse>();
    }
}
