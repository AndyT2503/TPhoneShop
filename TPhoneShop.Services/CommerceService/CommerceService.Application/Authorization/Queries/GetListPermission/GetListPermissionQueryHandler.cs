using CommerceService.Application.Authorization.Dtos;

namespace CommerceService.Application.Authorization.Queries.GetListPermission
{
    public class GetListPermissionQueryHandler : IRequestHandler<GetListPermissionQuery, ListPermissionResponse>
    {
        private readonly CommerceDbContext _dbContext;
        public GetListPermissionQueryHandler(CommerceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ListPermissionResponse> Handle(GetListPermissionQuery request, CancellationToken cancellationToken)
        {
            var listPermission = await _dbContext.Permissions.AsNoTracking()
                                                             .Select(e => new PermissionDto { Id = e.Id, Name = e.Name })
                                                             .ToListAsync(cancellationToken);
            return new ListPermissionResponse { Permissions = listPermission };
        }
    }
}
