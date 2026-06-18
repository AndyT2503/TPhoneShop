using BuildingBlocks.Application.Auth;
using Microsoft.AspNetCore.Authorization;

namespace CommerceService.Infrastructure.Authorization
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly UserAuthorizationService _userAuthorizationService;
        private readonly ICurrentUser _currentUser;

        public PermissionAuthorizationHandler(UserAuthorizationService userAuthorizationService, ICurrentUser currentUser)
        {
            _userAuthorizationService = userAuthorizationService;
            _currentUser = currentUser;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            if (string.IsNullOrWhiteSpace(requirement.Permission))
            {
                context.Succeed(requirement);
                return;
            }

            var hasPermission = await _userAuthorizationService.HasPermissionAsync(_currentUser.Id, requirement.Permission);
            if (hasPermission)
            {
                context.Succeed(requirement);
            }
        }
    }
}
