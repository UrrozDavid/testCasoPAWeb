using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PAWCP2.Core.Manager;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace PAWCP2.API.Auth
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IManagerUser _managerUser;

        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IManagerUser managerUser) : base(options, logger, encoder, clock)
        {
            _managerUser = managerUser;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization")) return AuthenticateResult.Fail("Missing Authorization");

            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);

                if (!"Basic".Equals(authHeader.Scheme, StringComparison.OrdinalIgnoreCase)) return AuthenticateResult.Fail("Invalid scheme");

                var credentialBytes = Convert.FromBase64String(authHeader.Parameter ?? "");
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(":", 2);
                if (credentials.Length != 2) return AuthenticateResult.Fail("Invalid credentials");

                var username = credentials[0];
                var password = credentials[1];

                var user = await _managerUser.AuthenticateAsync(username, password);
                if (user is null) return AuthenticateResult.Fail("Invalid username or password");

                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, user.Username ?? username),
                    new Claim(ClaimTypes.Email, user.Email ?? username),
                };

                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);
                return AuthenticateResult.Success(ticket);
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail(ex.Message);
            }
        }
    }
}