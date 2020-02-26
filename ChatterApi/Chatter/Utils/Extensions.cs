using System.Linq;
using System.Reflection;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Chatter.Utils
{
    public static class Extensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services, Assembly assembly)
        {
            var serviceTypes = assembly.GetExportedTypes().Where(t => t.IsPublic && !t.IsInterface && t.Name.EndsWith("Service"));
            foreach (var serviceType in serviceTypes)
            {
                var serviceName = $"{serviceType.Namespace}.I{serviceType.Name}";
                var implementationType = assembly.GetType(serviceName);
                services.AddScoped(implementationType, serviceType);
            }

            return services;
        }

        public static int GetUserIdFromAuth(this ControllerBase controller)
        {
            var claimsIdentity = (ClaimsIdentity) controller.HttpContext.User.Identity;
            return int.Parse(claimsIdentity.FindFirst(ClaimTypes.Name).Value);
        }
    }
}