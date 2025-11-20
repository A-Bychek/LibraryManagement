using AutoMapper;
using LibraryManagement.Application.Commands.Author;
using LibraryManagement.Application.DTOs.Authors;
using LibraryManagement.Application.QueryModels.Authors;
using LibraryManagement.Contract.Authors;

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
    }
}
