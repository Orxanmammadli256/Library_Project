using Library.Business.Implementations;
using Library.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Business.Abstractions
{
    public interface IAuthorService
    {
        void Create(string name, string? surname = null);
        void Update(int id, string name, string? surname = null);
        void Delete(int id);
        List<Author> GetAll();
        Author GetById(int id);
        List<Author> SearchByName(string name);
        List<Author> SearchBySurname(string? surname);
    }
}
