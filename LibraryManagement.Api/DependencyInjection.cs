using AutoMapper;
using LibraryManagement.Api.Mappings;
using LibraryManagement.Application.Mappings.Authors;
using LibraryManagement.Application.Mappings.Books;
using SimpleInjector;

namespace LibraryManagement.Api;
public static class DependencyInjection
{
    public static void AddAutoMapper(this Container container)
    {
        container.RegisterSingleton<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AuthorMappingProfile>();
                cfg.AddProfile<GrpcAuthorMappingProfile>();
                cfg.AddProfile<BookMappingProfile>();
                cfg.AddProfile<GrpcBookMappingProfile>();
            }, new LoggerFactory());

            return config.CreateMapper();
        });
    }
}