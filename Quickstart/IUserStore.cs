using AuthenticationSystem.Data.Models;
using System.Threading.Tasks;

namespace AuthenticationSystem.Quickstart
{
    public interface IUserStore
    {
        Task<bool> ValidateCredentials(string username, string password);
        Task<Users> FindBySubjectId(string subjectId);
        Task<Users> FindByUsername(string username);
        Task<bool> SaveAppUser(Users user);
    }
}
