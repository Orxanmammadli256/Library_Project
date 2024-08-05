using Library.Business.Implementations;
using Library.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Business.Abstractions
{
    public interface IGenreService
    {
        void Create(string name);
        void Update(int id, string name);
        void Delete(int id, BookService bookservice);
        List<Genre> GetAll();
        Genre GetById(int id);
        List<Genre> SearchByName(string name);
    }
}
