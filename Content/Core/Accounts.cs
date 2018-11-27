using System.Security.Claims;
using System.Threading.Tasks;
using Dolittle.Serialization.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Core
{
    public static class Accounts
    {
        public static void UseAccountsManagement(this IApplicationBuilder app)
        {
         var routeBuilder = new RouteBuilder(app);
            routeBuilder.MapGet("/Accounts/signin", async (request, response, routeData) => 
            {
                var httpContext = request.HttpContext;
                await httpContext.ChallengeAsync(OpenIdConnectDefaults.AuthenticationScheme, new AuthenticationProperties
                {
                    RedirectUri = "/"
                });

                await Task.CompletedTask;
            });

            routeBuilder.MapGet("/Accounts/GetStatus", async (request, response, routeData) =>
            {
                response.StatusCode = 200;

                var name = request.HttpContext.User.FindFirst(_ => _.Type == "name")?.Value ?? "unknown";
                var dnvglAccountName = request.HttpContext.User.FindFirst(_ => _.Type == "dnvglAccountName")?.Value ?? "unknown";
                var upn = request.HttpContext.User.FindFirst(ClaimTypes.Upn)?.Value ?? "unknown";

                var userStatus = new {
                    UserName = upn,
                    Name = name,
                    dnvglAccountName = dnvglAccountName,
                    isAuthenticated = request.HttpContext.User.Identity.IsAuthenticated
                };

                var serializer = app.ApplicationServices.GetService(typeof(ISerializer)) as ISerializer;
                var json = serializer.ToJson(userStatus, SerializationOptions.CamelCase);
                await response.WriteAsync(json);
            });
            app.UseRouter(routeBuilder.Build());
        }
    }
}