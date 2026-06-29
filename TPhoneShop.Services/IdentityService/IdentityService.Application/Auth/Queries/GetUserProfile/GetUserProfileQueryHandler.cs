using BuildingBlocks.Application.Auth;
using IdentityService.Application.Auth.Queries.Dtos;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Application.Auth.Queries.GetUserProfile
{
    internal class GetUserProfileQueryHandler(IdentityDbContext dbContext, ICurrentUser currentUser) : IRequestHandler<GetUserProfileQuery, UserProfileResponse>
    {
        public async Task<UserProfileResponse> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
        {
            var userProfile = await dbContext.Users.AsNoTracking()
                                                   .Select(e => new UserProfileResponse
                                                   {
                                                       Email = e.Email,
                                                       FullName = e.FullName,
                                                       Id = e.Id
                                                   })
                                                   .FirstOrDefaultAsync(e => e.Id == currentUser.Id, cancellationToken);
            if (userProfile == null)
            {
                throw new NotFoundException("Người dùng không tồn tại trong hệ thống");
            }
            return userProfile;
        }
    }
}
