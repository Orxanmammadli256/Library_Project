using Library.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Business.Abstractions
{
    public interface IBookService
    {
        void Create(string title, int count, DateTime publicationDate, List<Genre> genres, List<Author> authors);
        void Delete(Guid id);
        void Update(Guid id, string title, int count, DateTime publicationDate, List<Genre> genres, List<Author> authors);
        List<Book> GetAll();
        Book GetById(Guid id);
        List<Book> SearchByTitle(string title);
        List<Book> SearchByAuthor(Author author);
        List<Book> SearchByGenre(Genre genre);
        List<Book> SearchByPublicationDate(DateTime publicationDate);
        
    }
}
