using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualSports.BLL.Exceptions
{
    class NotExistedProviderException : Exception
    {
        public NotExistedProviderException()
        {
        }

        public NotExistedProviderException(string message) : base(message)
        {
        }
    }
}
