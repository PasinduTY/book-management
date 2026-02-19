using BookManagementAPI.Models;
using BookManagementAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookService _bookService;

        public BooksController(BookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public IActionResult GetBooks()
        {
            return Ok(new ApiResponse<List<Book>>
            {
                Success = true,
                Message = "Books retrieved successfully",
                Data = _bookService.GetAll()
            });
        }

        [HttpPost]
        public IActionResult AddBook(Book book)
        {
            return CreatedAtAction(nameof(GetBooks), new { id = book.Id }, new ApiResponse<Book>
            {
                Success = true,
                Message = "Book added successfully",
                Data = _bookService.Add(book)
            });
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBook(int id, Book book)
        {
            var updated = _bookService.Update(id, book);
            if (updated == null)
            {
                return NotFound(new ApiResponse<Book> 
                { 
                    Success = false, 
                    Message = $"Book {id} not found", 
                    Data = null 
                });
            }

            return Ok(new ApiResponse<Book> 
            { Success = true, 
                Message = "Book updated successfully", 
                Data = updated 
            });
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id)
        {
            if (!_bookService.Delete(id))
            {
                return NotFound(new ApiResponse<Book>
                {
                    Success = false,
                    Message = $"Book with ID {id} not found",
                    Data = null
                });
            }

            return Ok(new ApiResponse<Book>
            {
                Success = true,
                Message = "Book deleted successfully",
                Data = null
            });
        }
    }
}
