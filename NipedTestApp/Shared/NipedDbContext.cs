using Microsoft.EntityFrameworkCore;
using Shared.DataModels;

namespace Shared;

public class NipedDbContext : DbContext
{
    public NipedDbContext(DbContextOptions<NipedDbContext> options) : base(options)
    {
    }
    
    public DbSet<Client> Clients { get; set; }
    
    public DbSet<Bloodwork> Bloodworks { get; set; }

    public DbSet<Questionnaire> Questionnaires { get; set; }
    
    public DbSet<Guideline> Guidelines { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bloodwork>()
            .HasOne(b => b.Client)
            .WithMany(c => c.Bloodworks)
            .HasForeignKey(b => b.ClientId)
            .HasPrincipalKey(c => c.Id);
        
        modelBuilder.Entity<Questionnaire>()
            .HasOne(q => q.Client)
            .WithMany(c => c.Questionnaires)
            .HasForeignKey(q => q.ClientId)
            .HasPrincipalKey(c => c.Id);
    }
}