using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcModel.Models
{
    public class ArticleGroup
    {
        public CategoryEnum Category { get; set; }

        public DateTime Published { get; set; }
    }
}