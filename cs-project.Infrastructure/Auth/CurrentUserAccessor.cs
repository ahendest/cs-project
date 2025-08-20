using cs_project.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace cs_project.Infrastructure.Auth
{
    public class CurrentUserAccessor(IHttpContextAccessor http) : ICurrentUserAccessor
    {
        private readonly IHttpContextAccessor _http = http;

        public long GetCurrentUserId()
        {
            var user = _http.HttpContext?.User;
            if (user == null || !user.Identity?.IsAuthenticated == true) return 0;

            var id = user.FindFirst("sub")?.Value ?? user.FindFirst("userid")?.Value;
            return long.TryParse(id, out var lid) ? lid : 0;
        }
    }
}
