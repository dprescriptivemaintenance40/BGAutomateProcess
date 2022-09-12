using Microsoft.EntityFrameworkCore;
using ConsoleApp106.Model;
using Tasks.Models;

namespace ConsoleApp106.DAL
{
    public class PlantDBContext : DbContext
    {
        public DbSet<Asset_Equipment> Asset_Equipments { get; set; }
        public DbSet<Asset_FailureMode> Asset_FailureMode { get; set; }


        //Parameters
        public DbSet<ScrewParameter> ScrewParameters { get; set; }
        public DbSet<ScrewStagingTable> ScrewStagingTables { get; set; }
        public DbSet<ScrewCleaningTable> ScrewCleaningTables { get; set; }
        public DbSet<ScrewErrorTable> ScrewErrorTables { get; set; }
        public DbSet<ScrewProcessedTable> ScrewProcessedTables { get; set; }
        public DbSet<ScrewPredictedTable> ScrewPredictedTables { get; set; }

        public DbSet<CentrifugalParameter> CentrifugalParameters { get; set; }
        public DbSet<CentrifugalStagingTable> CentrifugalStagingTables { get; set; }
        public DbSet<CentrifugalCleaningTable> CentrifugalCleaningTables { get; set; }
        public DbSet<CentrifugalErrorTable> CentrifugalErrorTables { get; set; }
        public DbSet<CentrifugalProcessedTable> CentrifugalProcessedTables { get; set; }
        public DbSet<CentrifugalPredictedTable> CentrifugalPredictedTables { get; set; }

        public DbSet<ReciprocatingParameter> ReciprocatingParameters { get; set; }
        public DbSet<ReciprocatingStagingTable> ReciprocatingStagingTables { get; set; }
        public DbSet<ReciprocatingCleaningTable> ReciprocatingCleaningTables { get; set; }
        public DbSet<ReciprocatingErrorTable> ReciprocatingErrorTables { get; set; }
        public DbSet<ReciprocatingProcessedTable> ReciprocatingProcessedTables { get; set; }
        public DbSet<ReciprocatingPredictedTable> ReciprocatingPredicteds { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-CGG65T8;Initial Catalog=DPM;Integrated Security=True");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Asset_FailureMode>()
               .HasOne(r => r.equipments)
               .WithMany(r => r.asset_failureModes)
               .HasForeignKey(r => r.EquipmentId);

            modelBuilder.Entity<ScrewParameter>()
               .HasOne(r => r.asset_failureModes)
               .WithMany(r => r.screwParameter)
               .HasForeignKey(r => r.FailureModeId);

            modelBuilder.Entity<ScrewStagingTable>()
               .HasOne(r => r.screwParameter)
               .WithMany(r => r.screwStagingTable)
               .HasForeignKey(r => r.SPId);
            modelBuilder.Entity<ScrewCleaningTable>()
               .HasOne(r => r.screwParameter)
               .WithMany(r => r.screwCleaningTable)
               .HasForeignKey(r => r.SPId);
            modelBuilder.Entity<ScrewErrorTable>()
               .HasOne(r => r.screwParameter)
               .WithMany(r => r.screwErrorTable)
               .HasForeignKey(r => r.SPId);
            modelBuilder.Entity<ScrewProcessedTable>()
               .HasOne(r => r.screwParameter)
               .WithMany(r => r.screwProcessedTable)
               .HasForeignKey(r => r.SPId);
            modelBuilder.Entity<ScrewPredictedTable>()
               .HasOne(r => r.screwParameter)
               .WithMany(r => r.screwPredictedTable)
               .HasForeignKey(r => r.SPId);


            modelBuilder.Entity<CentrifugalParameter>()
               .HasOne(r => r.asset_failureModes)
               .WithMany(r => r.centrifugalParameter)
               .HasForeignKey(r => r.FailureModeId);

            modelBuilder.Entity<CentrifugalStagingTable>()
               .HasOne(r => r.centrifugalParameter)
               .WithMany(r => r.centrifugalStagingTable)
               .HasForeignKey(r => r.CPId);
            modelBuilder.Entity<CentrifugalCleaningTable>()
               .HasOne(r => r.centrifugalParameter)
               .WithMany(r => r.centrifugalCleaningTable)
               .HasForeignKey(r => r.CPId);
            modelBuilder.Entity<CentrifugalErrorTable>()
               .HasOne(r => r.centrifugalParameter)
               .WithMany(r => r.centrifugalErrorTable)
               .HasForeignKey(r => r.CPId);
            modelBuilder.Entity<CentrifugalProcessedTable>()
               .HasOne(r => r.centrifugalParameter)
               .WithMany(r => r.centrifugalProcessedTable)
               .HasForeignKey(r => r.CPId);
            modelBuilder.Entity<CentrifugalPredictedTable>()
               .HasOne(r => r.centrifugalParameter)
               .WithMany(r => r.centrifugalPredictedTable)
               .HasForeignKey(r => r.CPId);


            modelBuilder.Entity<ReciprocatingParameter>()
               .HasOne(r => r.asset_failureModes)
               .WithMany(r => r.reciprocatingParameter)
               .HasForeignKey(r => r.FailureModeId);

            modelBuilder.Entity<ReciprocatingStagingTable>()
               .HasOne(r => r.reciprocatingParameter)
               .WithMany(r => r.reciprocatingStagingTable)
               .HasForeignKey(r => r.RPId);
            modelBuilder.Entity<ReciprocatingCleaningTable>()
               .HasOne(r => r.reciprocatingParameter)
               .WithMany(r => r.reciprocatingCleaningTable)
               .HasForeignKey(r => r.RPId);
            modelBuilder.Entity<ReciprocatingErrorTable>()
               .HasOne(r => r.reciprocatingParameter)
               .WithMany(r => r.reciprocatingErrorTable)
               .HasForeignKey(r => r.RPId);
            modelBuilder.Entity<ReciprocatingProcessedTable>()
               .HasOne(r => r.reciprocatingParameter)
               .WithMany(r => r.reciprocatingProcessedTable)
               .HasForeignKey(r => r.RPId);
            modelBuilder.Entity<ReciprocatingPredictedTable>()
               .HasOne(r => r.reciprocatingParameter)
               .WithMany(r => r.reciprocatingPredictedTable)
               .HasForeignKey(r => r.RPId);
        }
    }

}




