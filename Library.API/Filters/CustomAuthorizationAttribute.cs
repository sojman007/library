using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Library.API.FauxDb;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Internal;

namespace Library.API.Filters
{
    public class CustomAuthorizationAttribute : TypeFilterAttribute
    {
        public CustomAuthorizationAttribute(string claimType = "", string claimValue = "")
        : base(typeof(CustomAuthorizationFilter))
        {
            if (string.IsNullOrEmpty(claimType) || string.IsNullOrWhiteSpace(claimValue))
                Arguments = new object[] {null};
            else 
                Arguments = new object[]{new Claim(claimType, claimValue)};
        }
    }

    public class CustomAuthorizationFilter : IAuthorizationFilter
    {
        private readonly Claim _claim;

        public CustomAuthorizationFilter(Claim claim)
        {
            this._claim = claim;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var authorizationHeader = context.HttpContext.Request.Headers[Constants.AuthorizationText].FirstOrDefault();
            if (authorizationHeader == null || !authorizationHeader.StartsWith(Constants.BearerText, StringComparison.OrdinalIgnoreCase))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            string token = authorizationHeader.Substring(Constants.BearerText.Length).Trim();
            if (!Db.UserTokens.Any(x => x.token == token))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (this._claim != null)
            {
                var hasClaim = context.HttpContext.User.Claims.Any(x => x.Type == _claim.Type && x.Value == _claim.Value);
                if (!hasClaim)
                    context.Result = new ForbidResult();
            }

        }
    }
}
