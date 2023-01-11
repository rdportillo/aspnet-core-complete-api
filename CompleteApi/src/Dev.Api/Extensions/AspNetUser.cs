using Dev.Business.Interfaces;
using System.Security.Claims;

namespace Dev.Api.Extensions
{
    public class AspNetUser : IUser
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public AspNetUser(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public string Name => _contextAccessor.HttpContext.User.Identity.Name;

        public IEnumerable<Claim> GetClaimsIdentity()
        {
            throw new NotImplementedException();
        }

        public string GetUserEmail()
        {
            return IsAuthenticated() ? _contextAccessor.HttpContext.User.GetUserEmail() : "";
        }

        public Guid GetUserId()
        {
            return IsAuthenticated() ? Guid.Parse(_contextAccessor.HttpContext.User.GetUserId()) : Guid.NewGuid();
        }

        public bool IsAuthenticated()
        {
            return _contextAccessor.HttpContext.User.Identity.IsAuthenticated;
        }

        public bool IsRole(string roleName)
        {
            return _contextAccessor.HttpContext.User.IsInRole(roleName);
        }
    }
}
