using System.Reflection;
using FeatureFlag.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Repository.Persistence;

public class FeatureFlagDbContext : DbContext
{
    public FeatureFlagDbContext(DbContextOptions<FeatureFlagDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<Recurso> Recursos { get; set; }
    public DbSet<Consumidor> Consumidores { get; set; }
    public DbSet<RecursoConsumidor> RecursosConsumidores { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.Entity<Recurso>()
            .HasMany(r => r.Consumidores)
            .WithMany(c => c.Recursos)
            .UsingEntity<RecursoConsumidor>();
    }
}