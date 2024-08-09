using Library.Business.Abstractions;
using Library.Business.Exceptions;
using Library.Core.Entities;
using Library.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Library.Business.Implementations
{
    public class MemberService : IMemberService
    {
        private Database _database;
        private BookRentalService _bookrentalservice; 
        public MemberService(Database database, BookRentalService bookrentalservice)
        {
            _database = database;
            _bookrentalservice = bookrentalservice;
        }
        public void Create(string name, string pin ,string? surname)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("Member name should not be null");
            }
            if(string.IsNullOrEmpty(pin))
            {
                throw new ArgumentNullException("Member pin should not be null");
            }
            if(pin.Count() != 7)
            {
                throw new InvalidPinException("Member pin count should be 7");
            }
            Member member = new Member(name, pin ,surname);
            _database.members.Add(member);
        }

        public void Delete(int id)
        {
            var member = _database.members.Find(m => m.Id == id);
            if(member is null)
            {
                throw new NotFoundException("This member does not exist");
            }
            if(member.bookRented == true)
            {
                throw new BookRentalExistException("There is a book rental in the name of this member"); 
            }
            _database.members.Remove(member);
        }

        public List<Member> GetAll()
        {
            return _database.members;
        }

        public Member GetById(int id)
        {
            var member = _database.members.Find(m => m.Id == id);
            if (member is null)
            {
                throw new NotFoundException("This member does not exist");
            }
            return member;
        }
        public void LoanBook(string pin, Guid bookid, DateTime returndate)
        {
            if (returndate < DateTime.Now)
            {
                throw new PastDateException("Return date should not be in the past");
            }
            var book = _database.books.Find(b => b.Id == bookid);
            if (book is null)
            {
                throw new NotFoundException("This book does not exist");
            }
            if (book.availableCount < 1)
            {
                throw new NotAvailableBookException("This book is not available now");
            }
            var member = _database.members.Find(m => m.Pin == pin);
            if (member is null)
            {
                throw new NotFoundException("This member does not exist");
            }
            if (member.bookRented == true)
            {
                throw new MemberAlreadyRentedBookException("This member already rented a book");
            }
            book.availableCount--;
            member.bookRented = true;
            _bookrentalservice.Create(member,book,returndate);
        }


        public void ReturnBook(string pin)
        {
            if (string.IsNullOrEmpty(pin))
            {
                throw new ArgumentNullException("Pin should not be null");
            }
            if (pin.Count() != 7)
            {
                throw new InvalidPinException("Pin count should be 7"); // ask this one
            }
            var member = _database.members.Find(m => m.Pin == pin);
            if (member is null)
            {
                throw new NotFoundException("This member does not exist");
            }
            if(member.bookRented == false)
            {
                throw new NotExistRentedBookException("This member did not rent a book");
            }
            Book book = _bookrentalservice.GetByMember(member).Book;
            book.availableCount++;
            member.bookRented = false;
            _bookrentalservice.Delete(member.Id, book.Id);
        }

        public List<Member> SearchByName(string name)
        {
            var members = _database.members.FindAll(m => m.Name.ToLower().Contains(name.ToLower()));
            return members;
        }

        public List<Member> SearchBySurname(string? surname)
        {
            if(surname is null)
            {
                _database.members.FindAll(m => m.Surname is null);
            }
            var members = _database.members.FindAll(m => m.Surname is not null && m.Surname.ToLower().Contains(surname.ToLower()));
            return members;
        }

        public void Update(int id, string name, string? surname)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("Member name should not be null");
            }
            var member = _database.members.Find(a => a.Id == id);
            if (member is null)
            {
                throw new NotFoundException("This member does not exist");
            }
            member.Name = name;
            member.Surname = surname;
        }
    }
}
