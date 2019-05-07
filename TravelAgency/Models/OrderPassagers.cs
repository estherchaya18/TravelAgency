using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelAgency.Models
{
    public class OrderPassagers
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public string Name { get; set; }

        public string PassportId { get; set; }

        public DateTime BirthDate { get; set; }


    }
}
