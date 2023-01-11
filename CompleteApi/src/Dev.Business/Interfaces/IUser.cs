using System.Security.Claims;

namespace Dev.Business.Interfaces
{
    public interface IUser
    {
        string Name { get; }

        Guid GetUserId();

        string GetUserEmail();

        bool IsAuthenticated();

        bool IsRole(string roleName);

        IEnumerable<Claim> GetClaimsIdentity();
    }
}
