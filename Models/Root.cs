﻿using System.Collections.Generic;
using VirtualSports.BE.Models.DatabaseModels;

namespace VirtualSports.BE.Models
{
    public class Root
    {
        public List<Provider> Providers { get; set; }
        public List<Category> Categories { get; set; }
        public List<Tag> Tags { get; set; }
        public List<Game> Games { get; set; }
    }
}
