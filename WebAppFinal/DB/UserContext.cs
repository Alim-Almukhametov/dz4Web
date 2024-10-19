using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebAppFinal.Models;

namespace WebAppFinal.DB
{
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

       

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {

            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

            optionsBuilder.UseMySql(configuration.GetConnectionString("db"), ServerVersion.AutoDetect(configuration.GetConnectionString("db"))).UseLazyLoadingProxies().LogTo(Console.WriteLine);
        }
            



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.HasKey(x => x.Id)
                      .HasName("user_pk");

                entity.Property(x => x.Name)
                      .HasColumnName("name")
                      .HasMaxLength(25);

                entity.HasIndex(x => x.Name)
                      .IsUnique();

                entity.Property(x => x.Password)
                      .HasColumnName("password");

                entity.Property(x => x.Salt)
                      .HasColumnName("salt");

                entity.Property(x => x.RoleId)
                      .HasConversion<int>();

                entity.HasOne(x => x.Role)
                      .WithMany(x => x.Users)
                      .HasForeignKey(x => x.RoleId);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("role");

                entity.Property(x => x.RoleId)
                      .HasConversion<int>();

                entity.HasData(Enum.GetValues(typeof(RoleId))
                      .Cast<RoleId>()
                      .Select(x => new Role { RoleId = x, Name = x.ToString() }));
            });
        }
    }
}
