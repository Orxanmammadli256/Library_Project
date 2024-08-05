using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Business.Exceptions
{
    internal class BookExistException : Exception
    {
        public BookExistException(string message) : base(message)
        {

        }
    }
}
