using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualSports.BLL.Exceptions
{
    class InvalidCategoryPlatformException : Exception
    {
        public InvalidCategoryPlatformException()
        {
        }

        public InvalidCategoryPlatformException(string message) : base(message)
        {
        }
    }
}
