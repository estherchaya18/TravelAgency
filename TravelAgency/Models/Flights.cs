using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelAgency.Models
{
    public class Flights
    {

        public int Id { get; set; }

        public int AppearanceAirportId { get; set; }

        public int LandingAirportId { get; set; }

        public DateTime AppppearanceDateTime { get; set; }

        public DateTime LandingDateTime { get; set; }

        public string AppearanceTerminal { get; set; }

        public string LandingTerminal { get; set; }

        public double Price { get; set; }

        public int TotalSeats { get; set; }

        public int ReservedSeats { get; set; }
    }
}
