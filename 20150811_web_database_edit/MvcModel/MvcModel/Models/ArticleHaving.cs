using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcModel.Models
{
    public class ArticleHaving
    {
        public CategoryEnum Category { get; set; }

        public double ViewAverage { get; set; }
    }
}