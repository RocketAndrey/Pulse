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

        public DbSet<Employee > Employees { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<UserInfo> UserInfo { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Operation>()
                .HasNoKey();
        }
    }
}
