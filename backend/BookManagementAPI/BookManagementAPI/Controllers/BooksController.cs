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
    }
}
