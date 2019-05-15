using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelAgency.Models
{
    public class OrderPassagers
    {

        public int PassangerId { get; set; }
        public int OrderId { get; set; }


        public virtual Order Order { get; set; }
        public virtual Passanger Passanger { get; set; }

    }
}
