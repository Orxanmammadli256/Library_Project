using Library.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Business.Abstractions
{
    public interface IBookRentalService
    {
        void Create(Member member, Book book, DateTime returndate);
        void Delete(int memberid, Guid bookid);
        void Update(int id, int memberid, Guid bookid, DateTime borrowdate, DateTime returndate);
        List<BookRental> GetAll();
        List<BookRental> ExpiredGetAll();
        BookRental GetById(int id);
        BookRental GetByMember(Member member);
    }
}
