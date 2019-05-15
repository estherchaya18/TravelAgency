using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TravelAgency.Models;

namespace TravelAgency.Models
{
    public class TravelAgencyContext : DbContext
    {
        public TravelAgencyContext (DbContextOptions<TravelAgencyContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderPassagers>()
                .HasKey(t => new { t.PassangerId, t.OrderId });

            modelBuilder.Entity<OrderPassagers>()
                .HasOne(pt => pt.Passanger)
                .WithMany(p => p.orderPassangers)
                .HasForeignKey(pt => pt.PassangerId);

            modelBuilder.Entity<OrderPassagers>()
                .HasOne(pt => pt.Order)
                .WithMany(t => t.orderPassangers)
                .HasForeignKey(pt => pt.OrderId);

        }

        public DbSet<TravelAgency.Models.Airlines> Airlines { get; set; }

        public DbSet<TravelAgency.Models.Airports> Airports { get; set; }

        public DbSet<TravelAgency.Models.Clients> Clients { get; set; }

        public DbSet<TravelAgency.Models.Flights> Flights { get; set; }

        public DbSet<TravelAgency.Models.Order> Order { get; set; }

        public DbSet<TravelAgency.Models.OrderPassagers> OrderPassagers { get; set; }

        public DbSet<TravelAgency.Models.Passanger> Passanger { get; set; }
    }
}
