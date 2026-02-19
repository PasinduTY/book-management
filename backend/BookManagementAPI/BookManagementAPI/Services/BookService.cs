using BookManagementAPI.Models;

namespace BookManagementAPI.Services
{
    public class BookService
    {
        private readonly List<Book> books = new List<Book>();
        private int nextId = 1;

        public List<Book> GetAll()
        {
            return books;
        }

        public Book Add(Book book)
        {
            book.Id = nextId++;
            books.Add(book);
            return book;
        }

        public Book? Update(int id, Book updatedBook)
        {
            var existing = books.FirstOrDefault(b => b.Id == id);
            if (existing == null) { 
                return null; 
            }

            existing.Title = updatedBook.Title;
            existing.Author = updatedBook.Author;
            existing.Isbn = updatedBook.Isbn;
            existing.PublicationDate = updatedBook.PublicationDate;

            return existing;
        }

        public bool Delete(int id)
        {
            var book = books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                return false;
            }

            books.Remove(book);
            return true;
        }
    }
}
