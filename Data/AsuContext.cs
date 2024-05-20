using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pulse.Models;
using Pulse.Models.Views;

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
        public DbSet<GroupLaborOperation> LaborOperations { get; set; }
        public DbSet<Pulse.Models.Estimator.TestProgram> EstimatorTestPrograms { get; set; }
        public DbSet<Pulse.Models.Estimator.ElementType> Estimator_ElementTypes { get; set; }
        public DbSet<Pulse.Models.Estimator.TestChainItem> ChainItems { get; set; }
        public DbSet<Pulse.Models.Estimator.ChainItemFilter> ChainItemFilter { get; set; }
        public DbSet<Pulse.Models.Estimator.OperationsList> OperationsList { get; set; }
        public DbSet<ContractLaborView> ContractLaborViewList { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Operation>()
                .HasNoKey();
            modelBuilder.Entity<Journal>()
            .HasNoKey();
            modelBuilder.Entity<GroupLaborOperation>()
                .HasNoKey();
            modelBuilder.Entity<ContractLaborView>()
            .HasNoKey();
            modelBuilder.Entity<Pulse.Models.Estimator.ChainItemFilter>().HasNoKey();
        }
    }
}
