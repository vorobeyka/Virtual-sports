using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualSports.BLL.Exceptions
{
    class NotExistedCategoryException : Exception
    {
        public NotExistedCategoryException()
        {
        }

        public NotExistedCategoryException(string message) : base(message)
        {
        }
    }
}
