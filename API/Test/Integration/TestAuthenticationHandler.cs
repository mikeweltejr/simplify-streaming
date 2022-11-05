using System;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Integration
{
    public class TestAuthenticationHandler : AuthenticationHandler<TestAuthenticationOptions>
    {
        public TestAuthenticationHandler(IOptionsMonitor<TestAuthenticationOptions> options, ILoggerFactory logger,
            UrlEncoder encoder, ISystemClock clock
        ) : base(options, logger, encoder, clock) {}

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var authTicket = new AuthenticationTicket(
                new ClaimsPrincipal(Options.Identity),
                new AuthenticationProperties(),
                "Test Scheme"
            );

            return Task.FromResult(AuthenticateResult.Success(authTicket));
        }
    }

    public static class TestAuthenticationExtensions
    {
        public static AuthenticationBuilder AddTestAuth(this AuthenticationBuilder builder, Action<TestAuthenticationOptions> configureOptions)
        {
            return builder.AddScheme<TestAuthenticationOptions, TestAuthenticationHandler>("Test Scheme", "Test Auth", configureOptions);
        }
    }

    public class TestAuthenticationOptions : AuthenticationSchemeOptions
    {
        public virtual ClaimsIdentity Identity { get; } = new ClaimsIdentity(new Claim[]
        {
            new Claim("http://testauth.com", Guid.NewGuid().ToString())
        }, "test");
    }
}
