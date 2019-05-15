using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelAgency.Models
{
    public class Airlines
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Flights> Flights { get; set; }
    }
}
