using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using System;
using System.Threading.Tasks;

namespace AuthenticationSystem.Quickstart
{
    public class ProfileService : IProfileService
    {
        protected readonly IUserStore _userstore;

        public ProfileService(IUserStore userstore)
        {
            _userstore = userstore;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var user = await _userstore.FindBySubjectId(context.Subject.GetSubjectId());
            if (user != null)
            {
                context.AddRequestedClaims(user.Claims);
            }
            return;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var user = await _userstore.FindBySubjectId(context.Subject.GetSubjectId());
            context.IsActive = !(user is null);
            return;
        }
    }
}
