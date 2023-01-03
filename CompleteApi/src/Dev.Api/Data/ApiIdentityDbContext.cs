using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Dev.Api.Data
{
    public class ApiIdentityDbContext : IdentityDbContext
    {
        public ApiIdentityDbContext(DbContextOptions<ApiIdentityDbContext> options) : base(options) { }
    }
}
