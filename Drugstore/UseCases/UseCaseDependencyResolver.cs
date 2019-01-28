using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Drugstore.UseCases
{
    public class UseCaseDependencyResolver
    {
        public static void Resolve(IServiceCollection services)
        {
            var useCases = typeof(Startup).Assembly.GetTypes().Where(t => t.FullName.EndsWith("UseCase"));
            foreach (var useCase in useCases)
            {
                services.AddScoped(useCase);
            }
        }
    }
}
