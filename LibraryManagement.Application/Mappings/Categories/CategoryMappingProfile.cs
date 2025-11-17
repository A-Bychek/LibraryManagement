using AutoMapper;
using LibraryManagement.Application.DTOs.Categories;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Mappings.Categories;

public class CategoryMappingProfile : Profile
{
    public CategoryMappingProfile()
    {
        CreateMap<Category, CategoryDto>()
            .ForMember(dest => dest.ParentCategoryName, opt => opt.MapFrom(src => src.ParentCategory != null ? src.ParentCategory.Name : null))
            .ForMember(dest => dest.BookCount, opt => opt.MapFrom(src => src.Books.Count));
    }
}


