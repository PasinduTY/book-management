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
    }
}
