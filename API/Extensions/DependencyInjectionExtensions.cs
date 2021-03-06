using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddScopedAsSelfByConvention(this IServiceCollection services, Assembly assembly, Predicate<Type> predicate)
        {
            List<Type> implementations = assembly.ExportedTypes
                .Where(x => !x.IsInterface && !x.IsAbstract && predicate.Invoke(x))
                .ToList();

            foreach (Type implementation in implementations)
            {
                services.AddScoped(implementation);
            }

            return services;
        }
    }
}