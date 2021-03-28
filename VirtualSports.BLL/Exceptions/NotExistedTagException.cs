using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualSports.BLL.Exceptions
{
    class NotExistedTagException : Exception
    {
        public NotExistedTagException()
        {
        }

        public NotExistedTagException(string message) : base(message)
        {
        }
    }
}
