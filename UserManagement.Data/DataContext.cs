using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using UserManagement.Models;

namespace UserManagement.Data;

public class DataContext : DbContext, IDataContext
{
    public DataContext() => Database.EnsureCreated();

    public override int SaveChanges()
    {
        AddAuditLogs();
        return base.SaveChanges();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseInMemoryDatabase("UserManagement.Data.DataContext");

    protected override void OnModelCreating(ModelBuilder model)
        => model.Entity<User>().HasData(new[]
        {
            new User { Id = 1, Forename = "Peter", Surname = "Loew", Email = "ploew@example.com", IsActive = true, DateOfBirth = new DateTime(1980, 1, 1) },
            new User { Id = 2, Forename = "Benjamin Franklin", Surname = "Gates", Email = "bfgates@example.com", IsActive = true, DateOfBirth = new DateTime( 1975, 5, 15) },
            new User { Id = 3, Forename = "Castor", Surname = "Troy", Email = "ctroy@example.com", IsActive = false, DateOfBirth = new DateTime(1985, 1, 1) },
            new User { Id = 4, Forename = "Memphis", Surname = "Raines", Email = "mraines@example.com", IsActive = true, DateOfBirth = new DateTime( 1982, 3, 10) },
            new User { Id = 5, Forename = "Stanley", Surname = "Goodspeed", Email = "sgodspeed@example.com", IsActive = true, DateOfBirth = new DateTime( 1980, 1, 1)},
            new User { Id = 6, Forename = "H.I.", Surname = "McDunnough", Email = "himcdunnough@example.com", IsActive = true, DateOfBirth = new DateTime( 1983, 7, 20) },
            new User { Id = 7, Forename = "Cameron", Surname = "Poe", Email = "cpoe@example.com", IsActive = false, DateOfBirth = new DateTime( 1981, 2, 28) },
            new User { Id = 8, Forename = "Edward", Surname = "Malus", Email = "emalus@example.com", IsActive = false, DateOfBirth = new DateTime( 1980, 1, 1)},
            new User { Id = 9, Forename = "Damon", Surname = "Macready", Email = "dmacready@example.com", IsActive = false, DateOfBirth = new DateTime( 1984, 6, 30) },
            new User { Id = 10, Forename = "Johnny", Surname = "Blaze", Email = "jblaze@example.com", IsActive = true, DateOfBirth = new DateTime( 1980, 1, 1)},
            new User { Id = 11, Forename = "Robin", Surname = "Feld", Email = "rfeld@example.com", IsActive = true, DateOfBirth = new DateTime( 1980, 1, 1)},
        });

    public DbSet<User>? Users { get; set; }
    public DbSet<UserChangeLog>? UserChangeLogs { get; set; }

    public IQueryable<TEntity> GetAll<TEntity>() where TEntity : class
        => base.Set<TEntity>();

    public void Create<TEntity>(TEntity entity) where TEntity : class
    {
        base.Add(entity);
        SaveChanges();
    }

    public new void Update<TEntity>(TEntity entity) where TEntity : class
    {
        var set = Set<TEntity>();
        var tracked = set.Local.FirstOrDefault(e =>
            Entry(e).Property("Id").CurrentValue?.ToString() ==
            Entry(entity).Property("Id").CurrentValue?.ToString());

        if (tracked != null)
        {
            Entry(tracked).CurrentValues.SetValues(entity);
        }
        else
        {
            var entry = Entry(entity);
            var idValue = entry.Property("Id").CurrentValue;
            var existing = set.Find(idValue);
            if (existing != null)
            {
                Entry(existing).CurrentValues.SetValues(entity);
            }
            else
            {
                set.Attach(entity);
                entry.State = EntityState.Modified;
            }
        }

        SaveChanges();
    }

    public void Delete<TEntity>(TEntity entity) where TEntity : class
    {
        base.Remove(entity);
        SaveChanges();
    }

    private void AddAuditLogs()
    {
        var auditEntries = ChangeTracker.Entries<User>()
            .Where(e => e.State == EntityState.Modified)
            .SelectMany(e => e.Properties
                .Where(p => p.IsModified)
                .Select(p => new UserChangeLog
                {
                    UserId = e.Entity.Id,
                    FieldName = p.Metadata.Name,
                    OldValue = p.OriginalValue?.ToString(),
                    NewValue = p.CurrentValue?.ToString(),
                    ChangedAt = DateTime.UtcNow
                }));

        UserChangeLogs!.AddRange(auditEntries);
    }
}
