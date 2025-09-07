using cs_project.Core.Interfaces;
using Microsoft.AspNetCore.Http;

namespace cs_project.Infrastructure.Auth
{
    public class CurrentUserAccessor(IHttpContextAccessor http) : ICurrentUserAccessor
    {
        private readonly IHttpContextAccessor _http = http;

        public string? GetCurrentUserId()
        {
            var user = _http.HttpContext?.User;
            if (user == null || !user.Identity?.IsAuthenticated == true) return null;

            return user.FindFirst("sub")?.Value ?? user.FindFirst("userid")?.Value;
        }
    }
}
