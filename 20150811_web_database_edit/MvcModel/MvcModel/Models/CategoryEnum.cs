using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace MvcModel.Models
{
    public enum CategoryEnum
    {
    [Display(Name = ".NET")]
    DotNet,

    [Display(Name = "クラウド")]
    Cloud,

    [Display(Name = "リファレンス")]
    Reference
    }
}