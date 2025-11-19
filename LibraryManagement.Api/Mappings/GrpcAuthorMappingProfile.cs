using AutoMapper;
using LibraryManagement.Application.Commands.Author;
using LibraryManagement.Application.DTOs.Authors;
using LibraryManagement.Contract.Authors;

namespace LibraryManagement.Api.Mappings;


public class GrpcAuthorMappingProfile : Profile
{
    public GrpcAuthorMappingProfile()
    {
        CreateMap<AuthorDto, AuthorResponse>();
        CreateMap<CreateAuthorRequest, CreateAuthorCommand>();
        CreateMap<UpdateAuthorRequest, UpdateAuthorCommand>();
    }
}
