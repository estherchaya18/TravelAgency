using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelAgency.Models
{
    public class Passanger
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string PassportId { get; set; }

        public DateTime BirthDate { get; set; }

        public ICollection<OrderPassagers> orderPassangers { get; set; }
    }
}
