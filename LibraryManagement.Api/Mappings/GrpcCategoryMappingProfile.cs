using AutoMapper;
using LibraryManagement.Application.DTOs.Categories;
using LibraryManagement.Contract.Categories;
using LibraryManagement.Contract.Commands.Category;
using LibraryManagement.Contract.QueryModels.Categories;

namespace LibraryManagement.Api.Mappings;

public class GrpcCategoryMappingProfile : Profile
{
    public GrpcCategoryMappingProfile()
    {
        CreateMap<CategoryDto, CategoryResponse>()
            .ForMember(dest => dest.ParentCategoryName, opt => opt.MapFrom(src => src.ParentCategoryName != null ? src.ParentCategoryName : null))
            .ForMember(dest => dest.BookCount, opt => opt.MapFrom(src => src.BookCount))
            .ForMember(dest => dest.SubCategories, opt => opt.MapFrom(src => src.SubCategories.AsEnumerable()));
        CreateMap<CreateCategoryRequest, CreateCategoryCommand>();
        CreateMap<CategorySearchRequest, CategorySearchArgs>();
    }
}
