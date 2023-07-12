using Microsoft.EntityFrameworkCore;
using UdemyProject.Models.Domain;

namespace UdemyProject.Data
{
    public class DBContextClass: DbContext
    {
        public DBContextClass(DbContextOptions<DBContextClass> options): base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<UsersRolesDomain>()
                .HasOne(x => x.UserDomain) // From UserDomain class
                .WithMany(y => y.userRolesDomain) // From both roles & user domain
                .HasForeignKey(z => z.UserID); // From UserRolesDomain

            modelBuilder.Entity<UsersRolesDomain>()
                .HasOne(x => x.RoleDomain) // From RoleDomain class
                .WithMany(y => y.userRolesDomain) // From both roles & user domain
                .HasForeignKey(z => z.RoleID); // From UserRolesDomain

        }

        public DbSet<RegionDomain> RegionTable { get; set; }
        public DbSet<WalkDomain> WalkTable { get; set; }
        public DbSet<DifficultyDomain> DifficultyTable { get; set; }
        public DbSet<ImageDomain> ImagesTable { get; set; }
        public DbSet<UsersDomain> UsersTable { get; set; }
        public DbSet<RolesDomain> RolesTable { get; set; }
        public DbSet<UsersRolesDomain> UsersRolesTable { get; set; }
    }
}

// dotnet ef migrations add "Inital Migration"
// dotnet ef database update