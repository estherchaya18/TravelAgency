using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelAgency.Models
{
    public class Clients
    {
        public int Id { get; set; }

        public string Mail { get; set; }

        public string Password { get; set; }

        public bool Director { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
