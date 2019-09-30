using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Library.API.Extensions;
using Library.API.Filters;
using Library.BLL.Dto.RequestModel;
using Library.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            this._bookService = bookService;
        }

        [HttpGet]
        [Route("allbooks")]
        [CustomAuthorization(ClaimTypes.Role, "admin")]
        public IActionResult GetAllBooks([FromQuery]SearchBookRequestModel model, int page = 1, int size = 10)
        {
            try
            {
                page = page > 0 ? page : 1;
                size = size > 0 ? size : 20;
                var books = this._bookService.SearchForBooksAsAdmin(model, page, size);
                return Ok(books);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // log exception
                return StatusCode(500, Constants.ServerErrorMessageText);
            }
        }

        [HttpGet]
        [Route("availablebooks")]
        [CustomAuthorization]
        public IActionResult GetAllAvailableBooks([FromQuery]SearchBookRequestModel model, int page = 1, int size = 10)
        {
            try
            {
                page = page > 0 ? page : 1;
                size = size > 0 ? size : 20;
                var books = this._bookService.SearchForBooksAsNonAdmin(model, page, size);
                return Ok(books);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // log exception
                return StatusCode(500, Constants.ServerErrorMessageText);
            }
        }

        [HttpPost]
        [Route("borrow")]
        [CustomAuthorization]
        public async Task<IActionResult> BorrowBook(long bookId, string userEmail)
        {
            try
            {
                var borrowedBook = await this._bookService.BorrowBookAsync(bookId, userEmail);
                return Ok(borrowedBook);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // log exception
                return StatusCode(500, Constants.ServerErrorMessageText);
            }
        }
        
        [HttpPost]
        [Route("return")]
        [CustomAuthorization]
        public async Task<IActionResult> ReturnBook(long bookId, string userEmail)
        {
            try
            {
                var returnedBook = await this._bookService.ReturnBookAsync(bookId, userEmail);
                return Ok(returnedBook);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // log exception
                return StatusCode(500, Constants.ServerErrorMessageText);
            }
        }

        [HttpPost]
        [Route("add")]
        [CustomAuthorization(ClaimTypes.Role, "admin")]
        public async Task<IActionResult> AddBook([FromBody]CreateBookRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorsAsList());
                var book = await this._bookService.CreateBookAsync(model);
                return Ok(book);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // log exception
                return StatusCode(500, Constants.ServerErrorMessageText);
            }
        }


        [HttpDelete]
        [Route("delete")]
        [CustomAuthorization]
        public async Task<IActionResult> DeleteBook(long bookId)
        {
            try
            {
                var result = await this._bookService.DeleteBookAsync(bookId);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // log exception
                return StatusCode(500, Constants.ServerErrorMessageText);
            }
        }
    }
}
