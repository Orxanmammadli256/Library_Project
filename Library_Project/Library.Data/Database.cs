using Library.Core.Entities;

namespace Library.Data
{
    public class Database
    {
        public List<Book> books;
        public List<Author> authors;
        public List<Genre> genres;
        public List<BookRental> bookRentals;
        public Database() 
        {
            books = new List<Book>();
            authors = new List<Author>();
            genres = new List<Genre>();
            bookRentals = new List<BookRental>();
        }
    }
}
