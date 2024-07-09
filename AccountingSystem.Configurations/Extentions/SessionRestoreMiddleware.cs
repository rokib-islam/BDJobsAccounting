using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSystem.Configurations.Extentions
{
    public class SessionRestoreMiddleware
    {
        private readonly RequestDelegate _next;

        public SessionRestoreMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.User.Identity.IsAuthenticated && context.Session.GetString("Name") == null)
            {
                // User is authenticated but session is null, restore session from claims
                var claimsIdentity = context.User.Identity as ClaimsIdentity;
                if (claimsIdentity != null)
                {
                    context.Session.SetString("Name", claimsIdentity.FindFirst("Name")?.Value);
                    context.Session.SetInt32("UserID", int.Parse(claimsIdentity.FindFirst("Id")?.Value));
                    context.Session.SetString("AccessRight", claimsIdentity.FindFirst("AccessRight")?.Value);
                    context.Session.SetString("ApproveRight", claimsIdentity.FindFirst("ApproveRight")?.Value);
                    context.Session.SetInt32("AccountDep", int.Parse(claimsIdentity.FindFirst("AccountDep")?.Value));
                    context.Session.SetInt32("CanModifyAdmin", int.Parse(claimsIdentity.FindFirst("CanModifyAdmin")?.Value));
                }
            }

            await _next(context);
        }
    }
}
