using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthenticationSystem.Data;
using AuthenticationSystem.Data.Models;
using IdentityModel;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationSystem.Quickstart
{
    public class UserStore : IUserStore
    {
        private readonly UserContext _userContext;
        private readonly Encrypt _crypt;

        public UserStore(
            UserContext userContext,
            Encrypt crypt
        )
        {
            _userContext = userContext;
            _crypt = crypt;
        }

        public Claim[] GetUserClaims(Users user)
        {
            return new Claim[]
            {
                new Claim(JwtClaimTypes.Name, user.UserName ?? ""),
                new Claim(JwtClaimTypes.GivenName, user.UserName ?? ""),
                new Claim(JwtClaimTypes.FamilyName, user.UserName ?? ""),
                new Claim(JwtClaimTypes.Email, user.Email ?? "")
            };
        }

        public async Task<Users> FindBySubjectId(string subjectId)
        {
            var user = await _userContext.Users.SingleOrDefaultAsync(u => u.Id == subjectId && u.Active == true);
            user.Claims = GetUserClaims(user);
            return user;
        }

        public async Task<Users> FindByUsername(string username)
        {
            var user = await _userContext.Users.SingleOrDefaultAsync(u => u.Email == username && u.Active == true);
            user.Claims = GetUserClaims(user);
            return user;
        }

        public async Task<bool> SaveAppUser(Users user)
        {
            bool complete = true;
            try
            {
                user.PasswordSalt = _crypt.PasswordSaltInBase64();
                user.PasswordHash = _crypt.PasswordToHashBase64(user.PasswordString, user.PasswordSalt);
                await _userContext.Users.AddAsync(user);
                await _userContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return false;
            }
            return complete;
        }

        public async Task<bool> ValidateCredentials(string username, string password)
        {
            try
            {
                var user = await _userContext.Users.SingleOrDefaultAsync(u => u.Email == username && u.Active == true && u.EmailConfirmed == true);
                if (user == null)
                {
                    return false;
                }
                return (String.IsNullOrEmpty(user.PasswordHash) || String.IsNullOrEmpty(user.PasswordSalt)) ? false
                    : _crypt.PasswordValidation(user.PasswordHash, user.PasswordSalt, password);
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
