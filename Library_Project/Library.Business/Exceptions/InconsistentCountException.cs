using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Business.Exceptions
{
    public class InconsistentCountException : Exception
    {
        public InconsistentCountException(string message) : base(message)
        {
                
        }
    }
}
