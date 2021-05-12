using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherApplication.Models
{
    public class Loc
    {
        [Key]
        public int id { set; get; }
        public string loc { set; get; }
    }
}
