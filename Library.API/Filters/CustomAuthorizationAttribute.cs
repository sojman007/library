using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Library.API.FauxDb;
using Library.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Internal;

namespace Library.API.Filters
{
    public class CustomAuthorizationAttribute : TypeFilterAttribute
    {
        public CustomAuthorizationAttribute()
            : base(typeof(CustomAuthorizationFilter))
        {
            Arguments = new object[] { };
        }

        public CustomAuthorizationAttribute(string claimType, string claimValue)
        : base(typeof(CustomAuthorizationFilter))
        {
            Arguments = new object[] { new Claim(claimType, claimValue) };
        }
    }

    public class CustomAuthorizationFilter : IAuthorizationFilter
    {
        private readonly Claim _claim;
        private readonly IUserService _userService;

        public CustomAuthorizationFilter(IUserService userService, Claim claim = null)
        {
            this._claim = claim;
            this._userService = userService;
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
            var tokenIsValid = this._userService.TokenIsValid(token);
            if (!tokenIsValid)
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
