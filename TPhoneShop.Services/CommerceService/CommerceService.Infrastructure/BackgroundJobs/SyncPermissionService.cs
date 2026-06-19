using CommerceService.Domain.Constants;
using CommerceService.Domain.Entities;
using CommerceService.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace CommerceService.Infrastructure.BackgroundJobs
{
    internal class SyncPermissionService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        public SyncPermissionService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await SyncPermission(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private async Task SyncPermission(CancellationToken cancellationToken)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<CommerceDbContext>();

            var definedPermissions = typeof(Permissions)
                                        .GetFields(BindingFlags.Public | BindingFlags.Static)
                                        .Where(f =>
                                            f.IsLiteral &&
                                            !f.IsInitOnly &&
                                            f.FieldType == typeof(string))
                                        .Select(f => (string)f.GetRawConstantValue()!)
                                        .ToHashSet();
            var dbPermissions = await dbContext.Permissions.ToListAsync(cancellationToken);

            var dbPermissionNames = dbPermissions.Select(x => x.Name).ToHashSet();

            var permissionsToAdd = definedPermissions.Except(dbPermissionNames)
                                            .Select(permissionName => new Permission { Name = permissionName })
                                            .ToList();

            dbContext.Permissions.AddRange(permissionsToAdd);

            var permissionsToRemove = dbPermissions
                           .Where(x => !definedPermissions.Contains(x.Name))
                           .ToList();

            dbContext.Permissions.RemoveRange(permissionsToRemove);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
