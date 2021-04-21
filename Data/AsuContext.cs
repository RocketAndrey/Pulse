using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pulse.Models; 

namespace Pulse.Data
{
    public class AsuContext:DbContext
    {
        public AsuContext (DbContextOptions <AsuContext> options ):base (options)
        {

        }
        public DbSet<RouteOperation> RouteOperations { get; set; }
        public DbSet<Employee > Employees { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<UserInfo> UserInfo { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Journal> Journal { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Ware> Wares { get; set; }
        public DbSet<Lot> Lots { get; set; }
        public DbSet<TestProgram> TestPrograms { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Operation>()
                .HasNoKey();
            modelBuilder.Entity<Journal>()
            .HasNoKey();
        }
    }
}
