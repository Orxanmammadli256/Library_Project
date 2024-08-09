﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Business.Exceptions
{
    public class PastDateException : Exception
    {
        public PastDateException(string message) : base(message)
        {

        }
    }
}