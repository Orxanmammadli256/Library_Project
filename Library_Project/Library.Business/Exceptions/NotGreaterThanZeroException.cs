using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Business.Exceptions
{
    public class NotGreaterThanZeroException : Exception
    {
        public NotGreaterThanZeroException(string message) : base(message)
        {
            
        }
    }
}
