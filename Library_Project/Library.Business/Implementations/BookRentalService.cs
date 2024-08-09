using Library.Business.Abstractions;
using Library.Business.Exceptions;
using Library.Core.Entities;
using Library.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Library.Business.Implementations
{
    public class BookRentalService : IBookRentalService
    {
        private Database _database;
        public BookRentalService(Database database)
        {
            _database = database;
        }
        public void Create(Member member, Book book, DateTime borrowdate, DateTime returndate)
        {
            BookRental bookRental = new BookRental(member,book,borrowdate,returndate);
            _database.bookRentals.Add(bookRental);
        }

        public void Delete(int memberid, Guid bookid)
        {
            var bookRental = _database.bookRentals.Find(br => br.Member.Id == memberid && br.Book.Id == bookid);
            if (bookRental is null)
            {
                throw new NotFoundException("This rental record does not exist");
            }
            _database.bookRentals.Remove(bookRental);
        }

        public List<BookRental> ExpiredGetAll()
        {
            var bookrentals = _database.bookRentals.FindAll(br => br.returnDate < DateTime.Now);
            return bookrentals;
        }

        public List<BookRental> GetAll()
        {
            return _database.bookRentals;
        }

        public void Update(int id, int memberid, Guid bookid, DateTime borrowdate, DateTime returndate)
        {
            if (borrowdate > DateTime.Now || returndate > DateTime.Now)
            {
                throw new FutureDateException("Date should not be in the future");
            }
            if (borrowdate > returndate)
            {
                throw new InConsistentDateIntervalException("Borrow date should not be greater than return date");
            }
            var bookrental = _database.bookRentals.Find(br => br.Id == id);
            if(bookrental is null)
            {
                throw new NotFoundException("This rental record does not exist");
            }
            var member = _database.members.Find(m => m.Id == memberid);
            if(member is null)
            {
                throw new NotFoundException("This member does not exist");
            }
            var book = _database.books.Find(b => b.Id == bookid);
            if( book is null)
            {
                throw new NotFoundException("This book does not exist");
            }
            if(member.Id != bookrental.Member.Id)
            {
                bookrental.Member.bookRented = false;
                member.bookRented = true;
            }
            if(book.Id != bookrental.Book.Id)
            {
                bookrental.Book.availableCount++;
                book.availableCount--;
            }
            bookrental.borrowDate = borrowdate;
            bookrental.returnDate = returndate;
            bookrental.Member = member;
            bookrental.Book = book;
        }
        public BookRental GetById(int id)
        {
            var bookRental = _database.bookRentals.Find(br => br.Id == id);
            if (bookRental is null)
            {
                throw new NotFoundException("This rental record does not exist");
            }
            return bookRental;
        }
        public BookRental GetByMember(Member member)
        {
            var bookrental = _database.bookRentals.Find(br => br.Member == member);
            if(bookrental is null)
            {
                throw new ArgumentNullException("This book rental record does not exist");
            }
            return bookrental;
        }
    }
}
