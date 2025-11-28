using AutoMapper;
using LibraryManagement.Api.Mappings;
using LibraryManagement.Application.Mappings.Authors;
using LibraryManagement.Application.Mappings.Books;
using LibraryManagement.Application.Mappings.Borrowings;
using LibraryManagement.Application.Mappings.Categories;
using Microsoft.Extensions.Logging;

namespace LibraryManagement.Integration.Tests.Base;

public abstract class AutoMapperTestBase
{
    protected IMapper _mapper { get; set; }
    protected MapperConfiguration _configuration { get; set; }

    protected AutoMapperTestBase()
    {
        _configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<GrpcAuthorMappingProfile>();
            cfg.AddProfile<GrpcBookMappingProfile>();
            cfg.AddProfile<GrpcBorrowingMappingProfile>();
            cfg.AddProfile<GrpcCategoryMappingProfile>();
            cfg.AddProfile<BookMappingProfile>();
            cfg.AddProfile<AuthorMappingProfile>();
            cfg.AddProfile<CategoryMappingProfile>();
            cfg.AddProfile<BorrowingMappingProfile>();
        }, new LoggerFactory());

        _mapper = _configuration.CreateMapper();
    }
}
