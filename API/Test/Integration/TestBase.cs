using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Integration
{
    public class TestBase : WebApplicationFactory<Program>
    {
        public HttpClient? client { get; private set; }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.UseEnvironment("Testing");
            builder.ConfigureServices(services => {
                services.AddAuthentication(options => {
                    options.DefaultAuthenticateScheme = "Test Scheme";
                    options.DefaultChallengeScheme = "Test Scheme";
                }).AddTestAuth(o => {});
            });

            var host = base.CreateHost(builder);
            client = host.GetTestClient();

            return host;
        }
    }
}
