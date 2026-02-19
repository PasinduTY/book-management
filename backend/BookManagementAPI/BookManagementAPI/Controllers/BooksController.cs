using BookManagementAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private static List<Book> books = new List<Book>();
        private static int nextId = 1;

        [HttpGet]
        public IActionResult GetBooks()
        {
            return Ok(new ApiResponse<List<Book>>
            {
                Success = true,
                Message = "Books retrieved successfully",
                Data = books
            });
        }

        [HttpPost]
        public IActionResult AddBook(Book book)
        {
            book.Id = nextId++;
            books.Add(book);

            return CreatedAtAction(nameof(GetBooks), new { id = book.Id }, new ApiResponse<Book>
            {
                Success = true,
                Message = "Book added successfully",
                Data = book
            });
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBook(int id, Book updatedBook)
        {
            var existingBook = books.FirstOrDefault(b => b.Id == id);

            if (existingBook == null)
            {
                return NotFound(new ApiResponse<Book>
                {
                    Success = false,
                    Message = $"Book with ID {id} not found",
                    Data = null
                });
            }

            existingBook.Title = updatedBook.Title;
            existingBook.Author = updatedBook.Author;
            existingBook.Isbn = updatedBook.Isbn;
            existingBook.PublicationDate = updatedBook.PublicationDate;

            return Ok(new ApiResponse<Book>
            {
                Success = true,
                Message = "Book updated successfully",
                Data = existingBook
            });
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id)
        {
            var book = books.FirstOrDefault(b => b.Id == id);

            if (book == null)
            {
                return NotFound(new ApiResponse<Book>
                {
                    Success = false,
                    Message = $"Book with ID {id} not found",
                    Data = null
                });
            }

            books.Remove(book);

            return Ok(new ApiResponse<Book>
            {
                Success = true,
                Message = "Book deleted successfully",
                Data = null
            });
        }
    }
}
