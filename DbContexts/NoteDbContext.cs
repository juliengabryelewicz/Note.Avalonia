using System;
using System.Linq;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;

using Note.AppCtx;
using Note.Entities;

namespace Note.DbContexts
{
    public class NoteDbContext : DbContext
    {
        public virtual DbSet<NoteEntity> Notes { get; set; }

        public NoteDbContext()
        {
            ChangeTracker.StateChanged += UpdateTimestamps;
            ChangeTracker.Tracked += UpdateTimestamps;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseLoggerFactory(new Serilog.Extensions.Logging.SerilogLoggerFactory(LoggingCtx.LogSql));

            options
                .UseSqlite(Program.ConfigCtx.AppDbConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (IMutableForeignKey fk in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
        protected void UpdateTimestamps(object sender, EntityEntryEventArgs e)
        {
            e.Entry.Entity.GetType().GetProperty("Updated").SetValue(e.Entry.Entity, DateTime.UtcNow);
        }        
    }
}