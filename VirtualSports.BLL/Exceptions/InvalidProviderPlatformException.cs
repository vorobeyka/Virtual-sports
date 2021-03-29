using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualSports.BLL.Exceptions
{
    class InvalidProviderPlatformException : Exception
    {
        public InvalidProviderPlatformException()
        {
        }

        public InvalidProviderPlatformException(string message) : base(message)
        {
        }
    }
}
