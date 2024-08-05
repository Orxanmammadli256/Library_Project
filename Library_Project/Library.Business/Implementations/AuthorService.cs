using Library.Business.Abstractions;
using Library.Business.Exceptions;
using Library.Core.Entities;
using Library.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Library.Business.Implementations
{
    public class AuthorService : IAuthorService
    {
        private Database _database;
        public AuthorService(Database database)
        {
            _database = database;
        }
        public void Create(string name, string? surname = null)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("Author name should not be null");
            }
            Author author = new Author(name, surname);
            _database.authors.Add(author);
        }

        public void Delete(int id, BookService bookservice)
        {
            var author = _database.authors.Find(a => a.Id == id);
            if (author is null)
            {
                throw new NotFoundException("This author does not exist");
            }
            var book = bookservice.SearchByAuthor(author);
            if(book.Count > 0)
            {
                throw new BookExistException("Book by this author exists");
            }
            _database.authors.Remove(author);
        }

        public List<Author> GetAll()
        {
            return _database.authors;
        }

        public Author GetById(int id)
        {
            var author = _database.authors.Find(a => a.Id == id);
            if (author is null)
            {
                throw new NotFoundException("This author does not exist");
            }
            return author;
        }

        public List<Author> SearchByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("Author name should not be null");
            }
            var authors = _database.authors.FindAll(a => a.Name.ToLower().Contains(name.ToLower()));
            return authors;
        }

        public List<Author> SearchBySurname(string? surname)
        {
            if(surname is null)
            {
               return _database.authors.FindAll(a => a.Surname is null);
            }
            var authors = _database.authors.FindAll(a => a.Surname is not null && a.Surname.ToLower().Contains(surname.ToLower()));
            return authors;
        }

        public void Update(int id, string name, string? surname = null)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("Author name should not be null");
            }
            var author = _database.authors.Find(a => a.Id == id);
            if (author is null)
            {
                throw new NotFoundException("This author does not exist");
            }
            author.Name = name;
            author.Surname = surname;
        }
    }
}
