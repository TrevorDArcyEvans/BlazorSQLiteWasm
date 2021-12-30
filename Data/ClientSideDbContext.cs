namespace BlazorSQLiteWasm.Data;

using Microsoft.EntityFrameworkCore;
using Models;

public sealed class ClientSideDbContext : DbContext
{
  public DbSet<Car> Cars { get; set; } = default!;

  public ClientSideDbContext(DbContextOptions<ClientSideDbContext> options) :
    base(options)
  {
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Car>(car =>
    {
      car.HasKey(e => e.Id);
      car.Property(e => e.Id).ValueGeneratedOnAdd();
    });
  }
}
