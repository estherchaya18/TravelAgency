using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TravelAgency.Models
{
    public class Airlines
    {
        public int Id { get; set; }

        
        [Display(Name = "Airlines name")]
        [Required]
        //[StringLength(30, ErrorMessage = "Too long")]
        //[RegularExpression(@"^[A-Z]+$", ErrorMessage = "Only uppercase")]
           public string Name { get; set; }

        public ICollection<Flights> Flights { get; set; }

       
    }
}
