using CommerceService.Application.Authorization.Queries.Dtos;

namespace CommerceService.Application.Authorization.Queries.GetListRole
{
    internal class GetListRoleQueryHanlder : IRequestHandler<GetListRoleQuery, ListRoleResponse>
    {
        private readonly CommerceDbContext _dbContext;
        public GetListRoleQueryHanlder(CommerceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ListRoleResponse> Handle(GetListRoleQuery request, CancellationToken cancellationToken)
        {
            var listRole = await _dbContext.Roles.AsNoTracking()
                                                 .Select(e => new RoleDto { Id = e.Id, Name = e.Name })
                                                 .ToListAsync(cancellationToken);
            return new ListRoleResponse { Roles = listRole };
        }
    }
}
