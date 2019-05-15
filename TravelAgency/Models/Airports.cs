using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TravelAgency.Models
{
    public class Airports
    {
        public int Id { get; set; }

        public string AirportDetailes { get; set; }


        [InverseProperty("AppearanceAirport")]
        public ICollection<Flights> RequestsRaised { get; set; }
        [InverseProperty("LandingAirport")]
        public ICollection<Flights> RequestsAssigned { get; set; }
    }
}
