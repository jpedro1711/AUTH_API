using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace authAPI
{
  public class AuthApiContext : DbContext
  {
    public DbSet<Usuario> Usuario {get; set;} = null!;
    public DbSet<Role> Role {get; set;} = null!;
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=LAPTOP-P8BTRSBI\SQLEXPRESS;Database=DB_JWT;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>()
          .HasIndex(u => u.Username)
          .IsUnique();
    }

   
    }
}