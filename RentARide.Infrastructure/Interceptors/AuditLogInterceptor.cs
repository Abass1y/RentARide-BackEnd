using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using RentARide.Domain.Entities;
using System.Text.Json;

namespace RentARide.Infrastructure.Interceptors
{
    public class AuditLogInterceptor : SaveChangesInterceptor
    {
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            var context = eventData.Context;
            
            if (context == null) return base.SavingChangesAsync(eventData, result, cancellationToken);

            var entries = context.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted)
                .ToList();

            foreach (var entry in entries)
            {
                if (entry.Entity is AuditLog) continue;

                var auditLog = new AuditLog
                {
                    EntityName = entry.Entity.GetType().Name,
                    Action = entry.State.ToString(),
                    UserId = "System",
                    Changes = GetChanges(entry)
                };

                context.Set<AuditLog>().Add(auditLog);
            }

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

      
        private static string GetChanges(Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entry)
        {
           
            Dictionary<string, object> changes = [];

            foreach (var property in entry.Properties)
            {
                if (entry.State == EntityState.Modified && property.IsModified)
                {
                    changes[property.Metadata.Name] = new { Old = property.OriginalValue, New = property.CurrentValue };
                }
                else if (entry.State == EntityState.Added)
                {
                    changes[property.Metadata.Name] = property.CurrentValue ?? "null"; 
                }
            }
            return JsonSerializer.Serialize(changes);
        }
    }
}