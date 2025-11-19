using FluentValidation;
using LibraryManagement.Application.Commands.Author;
using LibraryManagement.Application.Commands.Book;
using LibraryManagement.Application.Commands.Borrowing;
using LibraryManagement.Application.Commands.Category;
using LibraryManagement.Application.Interfaces.Services;
using LibraryManagement.Application.Services.Authors;
using LibraryManagement.Application.Services.Books;
using LibraryManagement.Application.Services.Borrowings;
using LibraryManagement.Application.Services.Categories;
using LibraryManagement.Application.Validation.Authors;
using LibraryManagement.Application.Validation.Books;
using LibraryManagement.Application.Validation.Borrowings;
using LibraryManagement.Application.Validation.Categories;
using SimpleInjector;

namespace LibraryManagement.Application
{
    public static class DependencyInjection
    {
        public static void AddApplication(this Container container)
        {
            container.Register<IAuthorService, AuthorService>(Lifestyle.Scoped);
            container.Register<IValidator<CreateAuthorCommand>, CreateAuthorCommandValidator>(Lifestyle.Scoped);
            container.Register<IValidator<UpdateAuthorCommand>, UpdateAuthorCommandValidator>(Lifestyle.Scoped);

            container.Register<IBookService, BookService>(Lifestyle.Scoped);
            container.Register<IValidator<CreateBookCommand>, CreateBookCommandValidator>(Lifestyle.Scoped);
            container.Register<IValidator<UpdateBookCommand>, UpdateBookCommandValidator>(Lifestyle.Scoped);

            container.Register<IBorrowingService, BorrowingService>(Lifestyle.Scoped);
            container.Register<IValidator<BorrowBookCommand>, BorrowBookCommandValidator>(Lifestyle.Scoped);
            container.Register<IValidator<ReturnBookCommand>, ReturnBookCommandValidator>(Lifestyle.Scoped);

            container.Register<ICategoryService, CategoryService>(Lifestyle.Scoped);
            container.Register<IValidator<CreateCategoryCommand>, CreateCategoryCommandValidator>(Lifestyle.Scoped);
        }
    }
}
