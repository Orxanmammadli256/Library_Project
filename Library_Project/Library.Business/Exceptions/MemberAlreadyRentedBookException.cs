using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Library.Business.Exceptions
{
    public class MemberAlreadyRentedBookException : Exception
    {
        public MemberAlreadyRentedBookException(string message) : base(message)
        {
                
        }
    }
}
