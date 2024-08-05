using Library.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Core.Entities
{
    public class BookRental : IEntity<int>
    {
        public int Id { get; set; }
        public string bookTitle { get; set; }
        public Guid bookId { get; set; }
        public DateTime borrowDate { get; set; }
        public DateTime returnDate { get; set; }
        private static int _id;
        public BookRental(Guid bookid, string booktitle, DateTime borrowdate, DateTime returndate)
        {
            bookId = bookid;
            bookTitle = booktitle;
            borrowDate = borrowdate;
            returnDate = returndate;
            Id = ++_id;
        }
    }
}
