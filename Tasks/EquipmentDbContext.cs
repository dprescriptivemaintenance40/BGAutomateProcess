using Microsoft.EntityFrameworkCore;
using ConsoleApp106.Model;
using Plant.Models.Plant;

namespace ConsoleApp106.DAL
{
    public class PlantDBContext : DbContext
    {
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<FailureMode> FailureMode { get; set; }
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
         
        }
    }

}




