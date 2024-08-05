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
        public void Create(Guid bookid, string booktitle, DateTime borrowdate, DateTime returndate)
        {
            if (string.IsNullOrEmpty(booktitle))
            {
                throw new ArgumentNullException("Title should not be null");
            }
            if (borrowdate > DateTime.Now || returndate > DateTime.Now)
            {
                throw new FutureDateException("Date should not be in the future");
            }
            if(borrowdate > returndate)
            {
                throw new InConsistentDateIntervalException("Borrow date should not be greater than return date");
            }
            var book = _database.books.Find(b => b.Id == bookid);
            if (book is null)
            {
                throw new NotFoundException("This book does not exist");
            }
            if(book.availableCount < 1)
            {
                throw new NotAvailableBookException("This book is now availabe now");
            }
            BookRental bookRental = new BookRental(bookid,booktitle,borrowdate,returndate);
            _database.bookRentals.Add(bookRental);
            book.availableCount--;
        }

        public void Delete(int id)
        {
            var bookRental = _database.bookRentals.Find(br => br.Id == id);
            if (bookRental is null)
            {
                throw new NotFoundException("This book rental does not exist");
            }
            _database.bookRentals.Remove(bookRental);
            var book = _database.books.Find(b => b.Id == bookRental.bookId);
            book.availableCount++;
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

        public void Update(int id, Guid bookid, string booktitle, DateTime borrowdate, DateTime returndate)
        {
            if (string.IsNullOrEmpty(booktitle))
            {
                throw new ArgumentNullException("Title should not be null");
            }
            if (borrowdate > DateTime.Now || returndate > DateTime.Now)
            {
                throw new FutureDateException("Date should not be in the future");
            }
            if (borrowdate > returndate)
            {
                throw new InConsistentDateIntervalException("Borrow date should not be greater than return date");
            }
            var newbook = _database.books.Find(b => b.Id == bookid);
            if (newbook is null)
            {
                throw new NotFoundException("This book does not exist");
            }
            var bookRental = _database.bookRentals.Find(br => br.Id == id);
            if(bookRental is null)
            {
                throw new NotFoundException("This book rental does not exist");
            }
            if (newbook.Id != bookRental.bookId)
            {
                var oldBook = _database.books.Find(b => b.Id == bookRental.bookId);
                oldBook.availableCount++;
                newbook.availableCount--;
            }
            bookRental.bookId = bookid;
            bookRental.borrowDate = borrowdate;
            bookRental.returnDate = returndate;
            bookRental.bookTitle = booktitle;
        }
        public BookRental GetById(int id)
        {
            var bookRental = _database.bookRentals.Find(br => br.Id == id);
            if (bookRental is null)
            {
                throw new NotFoundException("This book rental does not exist");
            }
            return bookRental;
        }
    }
}
