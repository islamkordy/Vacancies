using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Persistence.Context
{
    public class ContextWrite : DbContext
    {
        protected readonly IConfiguration Configuration;

        public ContextWrite(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // connect to sql server with connection string from app settings
            optionsBuilder.UseSqlServer(Configuration.GetConnectionString("ConnStr"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserAccount>(entity =>
            {
                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserAccounts)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("user_role_fk");


            });

            modelBuilder.Entity<Employer>(entity =>
            {
                entity.HasOne(d => d.UserAccount)
                    .WithOne(p => p.Employer)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Employer_fk");
            });

            //modelBuilder.Entity<Vacancy>()
            //  .HasMany(e => e.Applicants)
            //  .WithMany(e => e.Vacancies)
            //  .UsingEntity(
            //      "VacancyApplicant",
            //      l => l.HasOne(typeof(Vacancy)).WithMany().HasForeignKey("VacancyId").HasPrincipalKey(nameof(Vacancy.Id)),
            //      r => r.HasOne(typeof(Applicant)).WithMany().HasForeignKey("ApplicantId").HasPrincipalKey(nameof(Applicant.Id)),
            //      j => j.HasKey("VacancyId", "ApplicantId"));
        }
    }
}
