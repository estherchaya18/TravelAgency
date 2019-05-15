using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelAgency.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int FlightId { get; set; }

        public int ClientId { get; set; }

        public DateTime DateOrder { get; set; }

        public Flights Flights { get; set; }

        public Clients Clients { get; set; }

        public ICollection<OrderPassagers> orderPassangers { get; set; }
    }
}
