using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Business.Exceptions
{
    public class BookRentalExistException : Exception
    {
        public BookRentalExistException(string message) : base(message)
        {
            
        }
    }
}
