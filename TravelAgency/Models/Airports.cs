using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelAgency.Models
{
    public class Airports
    {
        public int Id { get; set; }

        public string AirportDetailes { get; set; }


        public ICollection<Flights> Flights { get; set; }
    }
}
