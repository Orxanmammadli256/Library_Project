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
    public class GenreService : IGenreService
    {
        private Database _database;
        public GenreService(Database database)
        {
            _database = database;
        }
        public void Create(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("Genre name should not be null");
            }
            var checkGenre = _database.genres.Find(g => g.Name == name);
            if(checkGenre is not null)
            {
                throw new AlreadyExistException("This name exists in the genres");
            }
            Genre genre = new Genre(name);
            _database.genres.Add(genre);
        }

        public void Delete(int id, BookService bookservice)
        {
            var genre = _database.genres.Find(g => g.Id == id);
            if(genre is null)
            {
                throw new NotFoundException("This genre does not exist");
            }
            var book = bookservice.SearchByGenre(genre);
            if (book.Count > 0)
            {
                throw new BookExistException("Book with this genre exists");
            }
            _database.genres.Remove(genre);
        }

        public List<Genre> GetAll()
        {
            return _database.genres;
        }

        public Genre GetById(int id)
        {
            var genre = _database.genres.Find(g => g.Id == id);
            if (genre is null)
            {
                throw new NotFoundException("This genre does not exist");
            }
            return genre;
        }

        public List<Genre> SearchByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("Genre name should not be null");
            }
            var genres = _database.genres.FindAll(genres => genres.Name.ToLower().Contains(name.ToLower()));
            return genres;
        }

        public void Update(int id, string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("Genre name should not be null");
            }
            var checkGenre = _database.genres.Find(g => g.Name == name);
            if (checkGenre is not null)
            {
                throw new AlreadyExistException("This name exists in the genres");
            }
            var genre = _database.genres.Find(g => g.Id == id);
            if (genre is null)
            {
                throw new NotFoundException("This genre does not exist");
            }
            genre.Name = name;
        }
    }
}
