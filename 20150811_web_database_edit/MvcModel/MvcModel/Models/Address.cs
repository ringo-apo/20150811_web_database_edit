using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;


namespace MvcModel.Models
{
    [ComplexType]
    public class Address
    {
        //[Column("Prefecture")]
        public string Prefecture { get; set; }

        public string City { get; set; }
        public string Street { get; set; }
    }
}