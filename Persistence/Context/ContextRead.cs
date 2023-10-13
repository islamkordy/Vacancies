using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Persistence.Context
{
    public class ContextRead : DbContext
    {
        protected readonly IConfiguration Configuration;

        public ContextRead(IConfiguration configuration)
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


            //modelBuilder.Entity<Vacancy>()
            //  .HasMany(x => x.Applicants)
            //  .WithMany(x => x.Vacancies)
            //  .UsingEntity<VacancyApplicant>(
            //      l => l.HasOne(x => x.Vacancy).WithMany().HasForeignKey(x => x.VacancyId),
            //      r => r.HasOne(x => x.Applicant).WithMany().HasForeignKey(x => x.ApplicantId));



    //        builder.HasMany(x => x.Tags).WithMany(x => x.Courses)
    //.UsingEntity<CourseTag>(
    //r => r.HasOne(x => x.Tag).WithMany().HasForeignKey(x => x.TagId),
    //l => l.HasOne(x => x.Courses).WithMany().HasForeignKey(x => x.CourseId));
        
    }
    }
}
           