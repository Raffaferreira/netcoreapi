using Infrastructure.Context;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TestingXUnit;
using Microsoft.AspNetCore.TestHost;
using Presentation;

namespace WebApi.Sql.IntegrationTests.WebApi.UseCases
{
    public class ApplicationTest : IntegrationTesting
    {
        public ApplicationTest(CustomWebApiApplicationFactory<Program> factory) : base(factory)
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var provider = scope.ServiceProvider;
                using (var webApiDbContext = provider.GetRequiredService<WebApiDbContext>())
                {
                    webApiDbContext.Database.EnsureCreatedAsync();

                    webApiDbContext.Credito.AddAsync(new Domain.Models.Credito { Id = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"), Value = 5865.00M, AccountTobeCredited = 0011 });
                    webApiDbContext.SaveChangesAsync();
                }
            }
        }

        [Fact]
        public async Task MiddlewareTest_ReturnsNotFoundForRequest()
        {
            using var host = await new HostBuilder()
                .ConfigureWebHost(webBuilder =>
                {
                    webBuilder
                        .UseTestServer()
                        .ConfigureServices(services =>
                        {
                            //services.AddMyServices();
                        })
                        .ConfigureTestServices(app =>
                        {
                            //app.UseMiddleware<MyMiddleware>();
                        });
                })
                .StartAsync();

            var response = await host.GetTestClient().GetAsync("/");

            Assert.NotEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task TestMiddleware_ExpectedResponse()
        {
            using var host = await new HostBuilder()
                .ConfigureWebHost(webBuilder =>
                {
                    webBuilder
                        .UseTestServer()
                        .ConfigureServices(services =>
                        {
                            //services.AddMyServices();
                        })
                        .ConfigureTestServices(app =>
                        {
                            //app.UseMiddleware<MyMiddleware>();
                        });
                })
                .StartAsync();

            var server = host.GetTestServer();
            server.BaseAddress = new Uri("https://example.com/A/Path/");

            var context = await server.SendAsync(c =>
            {
                c.Request.Method = HttpMethods.Post;
                c.Request.Path = "/and/file.txt";
                c.Request.QueryString = new QueryString("?and=query");
            });

            Assert.True(context.RequestAborted.CanBeCanceled);
            Assert.Equal(HttpProtocol.Http11, context.Request.Protocol);
            Assert.Equal("POST", context.Request.Method);
            Assert.Equal("https", context.Request.Scheme);
            Assert.Equal("example.com", context.Request.Host.Value);
            Assert.Equal("/A/Path", context.Request.PathBase.Value);
            Assert.Equal("/and/file.txt", context.Request.Path.Value);
            Assert.Equal("?and=query", context.Request.QueryString.Value);
            Assert.NotNull(context.Request.Body);
            Assert.NotNull(context.Request.Headers);
            Assert.NotNull(context.Response.Headers);
            Assert.NotNull(context.Response.Body);
            Assert.Equal(404, context.Response.StatusCode);
            Assert.Null(context.Features.Get<IHttpResponseFeature>().ReasonPhrase);
        }
    }
}
