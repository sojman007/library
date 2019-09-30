using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Library.BLL.Dto.Extensions;
using Library.BLL.Dto.RequestModel;
using Library.BLL.Dto.ResponseModel;
using Library.BLL.Services.Interfaces;
using Library.DAL.Entities;
using Library.DAL.Repositories.Interface;
using Microsoft.AspNetCore.Http;

namespace Library.BLL.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepo _bookRepo;
        private readonly IBookHistoryRepo _bookHistoryRepo;
        private readonly IUserRepo _userRepo;

        public BookService(IBookRepo bookRepo, IBookHistoryRepo bookHistoryRepo, IUserRepo userRepo)
        {
            this._bookRepo = bookRepo;
            this._bookHistoryRepo = bookHistoryRepo;
            this._userRepo = userRepo;
        }

        public async Task<MiscResponse<Admin_BookResponseModel>> CreateBookAsync(CreateBookRequestModel model)
        {
            var existingBook = this._bookRepo.Find(x => x.Title == model.Title && x.Author == model.Author);
            if (existingBook.Any())
                throw new ArgumentException($"A book exists with the given title '{model.Title}' and author '{model.Author}'");

            existingBook = this._bookRepo.Find(x => x.ISBN == model.ISBN);
            if (existingBook.Any())
                throw new ArgumentException($"A book exists with the given ISBN '{model.ISBN}'");

            var bookId = await this._bookRepo.AddAsync(new Book
            { Author = model.Author, ISBN = model.ISBN, Title = model.Title, IsAvailable = true });

            return new MiscResponse<Admin_BookResponseModel>
            {
                Data = new Admin_BookResponseModel { Author = model.Author, IsAvailable = true, ISBN = model.ISBN, Title = model.Title, Id = bookId },
                Message = "Book created successfully"
            };
        }

        public async Task<MiscResponse<object>> DeleteBookAsync(long bookId)
        {
            var existingBook = this._bookRepo.Find(x => x.Id == bookId).FirstOrDefault();
            if (existingBook == null)
                throw new ArgumentException($"Book with given id '{bookId}' does not exist");

            if (!existingBook.IsAvailable)
                throw new ArgumentException($"A borrowed book cannot be deleted");

            existingBook.IsDeleted = true;
            await this._bookRepo.UpdateAsync(existingBook);
            return new MiscResponse<object>() {Message = "Book deleted successfully"};
        }

        public SearchResponse<User_BookReponseModel> SearchForBooksAsNonAdmin(SearchBookRequestModel model, int page, int size)
        {
            var availableBooks = this._bookRepo.Find(x => x.IsAvailable);
            availableBooks = searchForBook(model, availableBooks);
            var response = new SearchResponse<User_BookReponseModel>{ Page = page, TotalCount = availableBooks.Count() };
            response.Result = availableBooks.ToList().Skip((response.Page - 1) * size).Take(size).Select(x => x.ToUserModel());
            return response;
        }

        private static IQueryable<Book> searchForBook(SearchBookRequestModel model, IQueryable<Book> availableBooks)
        {
            if (!string.IsNullOrWhiteSpace(model.ISBN))
                availableBooks = availableBooks.Where(x => x.ISBN.ToLower() == model.ISBN.ToLower());

            if (!string.IsNullOrWhiteSpace(model.Title))
                availableBooks = availableBooks.Where(x => x.Title.ToLower().Contains(model.Title.ToLower()));

            if (!string.IsNullOrWhiteSpace(model.Author))
                availableBooks = availableBooks.Where(x => x.Author.ToLower().Contains(model.Author.ToLower()));

            return availableBooks;
        }

        public SearchResponse<Admin_BookResponseModel> SearchForBooksAsAdmin(SearchBookRequestModel model, int page, int size)
        {
            var availableBooks = this._bookRepo.GetAll();
            availableBooks = searchForBook(model, availableBooks);
            var response = new SearchResponse<Admin_BookResponseModel> { Page = page, TotalCount = availableBooks.Count() };
            response.Result = availableBooks.ToList().Skip((response.Page - 1) * size).Take(size).Select(x => x.ToAdminModel());
            return response;
        }

        public async Task<MiscResponse<User_BookReponseModel>> BorrowBookAsync(long bookId, string userEmail)
        {
            var book = this._bookRepo.Find(x => x.Id == bookId).FirstOrDefault();
            if (book == null)
                throw new ArgumentException($"No book found with the id '{bookId}'");
            if (!book.IsAvailable)
                throw new ArgumentException($"The book with id '{bookId}' has been borrowed");

            var user = this._userRepo.Find(x => x.Email == userEmail).FirstOrDefault();
            if (user == null)
                throw new ArgumentException("Unable to lend book to non-existent user");

            book.IsAvailable = false;
            await this._bookHistoryRepo.AddAsync(new BookHistory {BookId = bookId, Returned = false, LenderId = user.Id });
            await this._bookRepo.UpdateAsync(book);
            return new MiscResponse<User_BookReponseModel>() {
                Message = "Successfully borrowed a book",
                Data = book.ToUserModel(),
            };
        }

        public async Task<MiscResponse<User_BookReponseModel>> ReturnBookAsync(long bookId, string userEmail)
        {
            var book = this._bookRepo.Find(x => x.Id == bookId).FirstOrDefault();
            if (book == null)
                throw new ArgumentException($"No book found with the id '{bookId}'");

            var bookHistory = this._bookHistoryRepo.Find(x => x.BookId == bookId && !x.Returned).FirstOrDefault();
            if (bookHistory == null)
                throw new ArgumentException($"The book with id '{bookId}' is in the library");

            var user = this._userRepo.Find(x => x.Email == userEmail).FirstOrDefault();
            if (user == null)
                throw new ArgumentException("Unable to receive book to non-existent user");

            if (user.Id != bookHistory.LenderId)
                throw new ArgumentException("Borrowed book can only be returned by lender");

            book.IsAvailable = true;
            bookHistory.Returned = true;
            await this._bookHistoryRepo.UpdateAsync(bookHistory);
            await this._bookRepo.UpdateAsync(book);
            return new MiscResponse<User_BookReponseModel> {
                Message = "Successfully returned a book",
                Data = book.ToUserModel(),
            };
        }
    }
}
