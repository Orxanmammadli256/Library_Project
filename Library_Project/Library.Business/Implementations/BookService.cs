using Library.Business.Abstractions;
using Library.Business.Exceptions;
using Library.Core.Entities;
using Library.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Business.Implementations
{
    public class BookService : IBookService
    {
        private Database _database;
        public BookService(Database database)
        {
            _database = database;
        }
        public void Create(string title, int count, DateTime publicationdate, List<Genre> genres, List<Author> authors)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException("Title should not be null");
            }
            if(count < 1)
            {
                throw new NotGreaterThanZeroException("Count should be greater than 0");
            }
            if(publicationdate > DateTime.Now)
            {
                throw new FutureDateException("Date should not be in the future");
            }
            Book book = new Book(title, count, publicationdate);
            book.Authors = authors;
            book.Genres = genres;
            _database.books.Add(book);
        }

        public void Delete(Guid id)
        {
            var book = _database.books.Find(b => b.Id == id);
            if (book is null)
            {
                throw new NotFoundException("This book does not exist");
            }
            _database.books.Remove(book);
        }

        public List<Book> GetAll()
        {
            return _database.books;
        }

        public Book GetById(Guid id)
        {
            var book = _database.books.Find(b => b.Id == id);
            if (book is null)
            {
                throw new NotFoundException("This book does not exist");
            }
            return book;
        }

        public List<Book> SearchByAuthor(Author author)
        {
            var books = _database.books.Where(b => b.Authors.Contains(author)).ToList();
            return books;
        }

        public List<Book> SearchByGenre(Genre genre)
        {
            var books = _database.books.Where(b => b.Genres.Contains(genre)).ToList();
            return books;
        }

        public List<Book> SearchByPublicationDate(DateTime publicationdate)
        {
            if (publicationdate > DateTime.Now)
            {
                throw new FutureDateException("Date should not be in the future");
            }
            var books = _database.books.FindAll(b => b.publicationDate == publicationdate);
            return books;
        }

        public List<Book> SearchByTitle(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException("Title should not be null");
            }
            var books = _database.books.FindAll(b => b.Title.ToLower().Contains(title.ToLower()));
            return books;
        }

        public void Update(Guid id, string title, int count, DateTime publicationDate, List<Genre> genres, List<Author> authors)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException("Title should not be null");
            }
            if (count < 1)
            {
                throw new NotGreaterThanZeroException("Count should be greater than 0");
            }
            if (publicationDate > DateTime.Now)
            {
                throw new FutureDateException("Date should not be in the future");
            }
            var book = _database.books.Find(b => b.Id == id);
            if (book is null)
            {
                throw new NotFoundException("This book does not exist");
            }
            book.Title = title;
            book.totalCount = count;
            book.publicationDate = publicationDate;
            book.Genres = genres;
            book.Authors = authors;
        }
    }
}
