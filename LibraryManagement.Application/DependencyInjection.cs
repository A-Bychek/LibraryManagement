using FluentValidation;
using LibraryManagement.Application.Interfaces.Services;
using LibraryManagement.Application.Services.Authors;
using LibraryManagement.Application.Services.Books;
using LibraryManagement.Application.Services.Borrowings;
using LibraryManagement.Application.Services.Categories;
using LibraryManagement.Application.Validation.Authors;
using LibraryManagement.Application.Validation.Books;
using LibraryManagement.Application.Validation.Borrowings;
using LibraryManagement.Application.Validation.Categories;
using LibraryManagement.Contract.Commands.Author;
using LibraryManagement.Contract.Commands.Book;
using LibraryManagement.Contract.Commands.Borrowing;
using LibraryManagement.Contract.Commands.Category;
using LibraryManagement.Contract.QueryModels.Authors;
using LibraryManagement.Contract.QueryModels.Books;
using LibraryManagement.Contract.QueryModels.Borrowings;
using LibraryManagement.Contract.QueryModels.Categories;
using SimpleInjector;

namespace LibraryManagement.Contract;

public static class DependencyInjection
{
    public static void AddApplication(this Container container)
    {
        container.Register<IAuthorService, AuthorService>(Lifestyle.Scoped);
        container.Register<IValidator<CreateAuthorCommand>, CreateAuthorCommandValidator>(Lifestyle.Scoped);
        container.Register<IValidator<UpdateAuthorCommand>, UpdateAuthorCommandValidator>(Lifestyle.Scoped);
        container.Register<IValidator<AuthorSearchArgs>, AuthorSearchArgsValidator>(Lifestyle.Scoped);

        container.Register<IBookService, BookService>(Lifestyle.Scoped);
        container.Register<IValidator<CreateBookCommand>, CreateBookCommandValidator>(Lifestyle.Scoped);
        container.Register<IValidator<UpdateBookCommand>, UpdateBookCommandValidator>(Lifestyle.Scoped);
        container.Register<IValidator<BookSearchArgs>, BookSearchArgsValidator>(Lifestyle.Scoped);

        container.Register<IBorrowingService, BorrowingService>(Lifestyle.Scoped);
        container.Register<IValidator<BorrowBookCommand>, BorrowBookCommandValidator>(Lifestyle.Scoped);
        container.Register<IValidator<ReturnBookCommand>, ReturnBookCommandValidator>(Lifestyle.Scoped);
        container.Register<IValidator<BorrowingSearchArgs>, BorrowingSearchArgsValidator>(Lifestyle.Scoped);

        container.Register<ICategoryService, CategoryService>(Lifestyle.Scoped);
        container.Register<IValidator<CreateCategoryCommand>, CreateCategoryCommandValidator>(Lifestyle.Scoped);
        container.Register<IValidator<CategorySearchArgs>, CategorySearchArgsValidator>(Lifestyle.Scoped);
    }
}
