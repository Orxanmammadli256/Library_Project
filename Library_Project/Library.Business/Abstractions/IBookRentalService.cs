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
        void Create(Guid bookid, string booktitle, DateTime borrowdate, DateTime returndate);
        void Delete(int id);
        void Update(int id, Guid bookid, string booktitle, DateTime borrowdate, DateTime returndate);
        List<BookRental> GetAll();
        List<BookRental> ExpiredGetAll();
        BookRental GetById(int id);
    }
}
