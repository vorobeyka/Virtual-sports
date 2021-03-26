using System.Collections.Generic;
using VirtualSports.DAL.Entities;

namespace VirtualSports.BLL.Services.DatabaseServices.Impl
{
    internal class Root
    {
        public List<Game> Games { get; set; }
        public List<Category> Categories { get; set; }
        public List<Provider> Providers { get; set; }
        public List<Tag> Tags { get; set; }
    }
}