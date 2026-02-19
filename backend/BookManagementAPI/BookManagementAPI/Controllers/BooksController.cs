using BookManagementAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private static List<Book> books = new List<Book>
        {
            new Book
            {
                Id = 1,
                Title = "Harry Potter",
                Author = "J.K. Rowling",
                Isbn = "12345",
                PublicationDate = DateTime.Now
            }
        };

        [HttpGet]
        public IActionResult GetBooks()
        {
            return Ok(books);
        }

        [HttpPost]
        public IActionResult AddBook(Book book)
        {
            book.Id = books.Count + 1;
            books.Add(book);

            return CreatedAtAction(nameof(GetBooks), new { id = book.Id }, book);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBook(int id, Book updatedBook)
        {
            var existingBook = books.FirstOrDefault(b => b.Id == id);

            if (existingBook == null)
            {
                return NotFound();
            }

            existingBook.Title = updatedBook.Title;
            existingBook.Author = updatedBook.Author;
            existingBook.Isbn = updatedBook.Isbn;
            existingBook.PublicationDate = updatedBook.PublicationDate;

            return Ok(existingBook);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id)
        {
            var book = books.FirstOrDefault(b => b.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            books.Remove(book);

            return NoContent();
        }
    }
}
