using Library.Business.Implementations;
using Library.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Business.Abstractions
{
    public interface IMemberService
    {
        void Create(string name, string pin, string? surname);   
        void Delete(int id);
        void Update(int id, string name, string? surname);
        List<Member> GetAll();
        Member GetById(int id);
        List<Member> SearchByName(string name);
        List<Member> SearchBySurname(string? surname);
        void LoanBook(string pin, Guid bookid, DateTime returnDate);
        void ReturnBook(string pin);
    }
}
