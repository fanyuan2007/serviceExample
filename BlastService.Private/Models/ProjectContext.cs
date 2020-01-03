using Microsoft.EntityFrameworkCore;

namespace BlastService.Private.Models
{
    public partial class ProjectContext : DbContext
    {
        public ProjectContext()
        {
        }

        public ProjectContext(DbContextOptions<ProjectContext> options)
            : base(options)
        {
        }

        public virtual DbSet<PatternDb> Patterns { get; set; }

        public virtual DbSet<HoleDb> Holes { get; set; }

        public virtual DbSet<FragmentationDb> Fragments { get; set; }

        public virtual DbSet<ChargeIntervalDb> ChargingIntervals { get; set; }

        public virtual DbSet<ProjectDb> Projects { get; set; }


        //protected override void BuildTargetModel(ModelBuilder modelBuilder)
        //{
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("postgis");

            modelBuilder.Entity<ProjectDb>()
                .Property(p => p.Unit)
                .HasConversion<string>();

            modelBuilder.Entity<ProjectDb>()
                .Property(p => p.Mapping)
                .HasConversion<string>();

            modelBuilder.Entity<PatternDb>()
                .Property(p => p.PatternType)
                .HasConversion<string>();

            modelBuilder.Entity<PatternDb>()
                .Property(p => p.ValidationState)
                .HasConversion<string>();

            modelBuilder.Entity<ChargeIntervalDb>()
                 .Property(c => c.ProfileType)
                 .HasConversion<string>();
        }
    }
}
