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
        public Member Member { get; set; }
        public Book Book { get; set; }
        public DateTime borrowDate { get; set; }
        public DateTime returnDate { get; set; }
        private static int _id;
        public BookRental(Member member, Book book, DateTime borrowdate, DateTime returndate)
        {
            Book = book;
            Member = member;
            borrowDate = borrowdate;
            returnDate = returndate;
            Id = ++_id;
        }
        public override string ToString()
        {
            return $"Id: {Id}\tMemberName: {Member.Name}\tMemberSurname: {Member.Surname}\tBookTitle: {Book.Title}\t  BorrowDate: {borrowDate.ToString("dd.MM.yyyy")}\tReturnDate: {returnDate.ToString("dd.MM.yyyy")}";
        }
    }
}
