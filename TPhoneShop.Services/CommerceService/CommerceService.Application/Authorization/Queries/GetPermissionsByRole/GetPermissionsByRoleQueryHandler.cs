using CommerceService.Application.Authorization.Dtos;

namespace CommerceService.Application.Authorization.Queries.GetPermissionsByRole
{
    public class GetPermissionsByRoleQueryHandler : IRequestHandler<GetPermissionsByRoleQuery, ListPermissionResponse>
    {
        private readonly CommerceDbContext _dbContext;
        public GetPermissionsByRoleQueryHandler(CommerceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ListPermissionResponse> Handle(GetPermissionsByRoleQuery request, CancellationToken cancellationToken)
        {
            var listPermission = await _dbContext.RolePermissions.AsNoTracking()
                                                                 .Where(e => e.RoleId == request.RoleId)
                                                                 .Select(e => new PermissionDto { Id = e.PermissionId, Name = e.Permission.Name })
                                                                 .ToListAsync(cancellationToken);
            return new ListPermissionResponse { Permissions = listPermission };
        }
    }
}
