using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Common.IOC
{
    public static class IOCRegister
    {
        public static IServiceCollection Register(this IServiceCollection serviceCollection, params Assembly[] assemblies)
        {
            Type baseType = typeof(IDependencyTransient);
            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes().Where(x => baseType.IsAssignableFrom(x)).Where(x => x.IsClass && x.IsAbstract == false))
                {
                    foreach (var interfaceType in type.GetInterfaces().Where(x => baseType.IsAssignableFrom(x)))
                    {
                        serviceCollection.Add(new ServiceDescriptor(interfaceType, type, ServiceLifetime.Transient));
                    }
                }
            }
            return serviceCollection;
        }

        /// <summary>
        /// 将提供的程序集中的所有类使用.net core自带的DI进行注入,生存周期为Transient
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public static IServiceCollection RegisterAssembliesTransient(this IServiceCollection serviceCollection, params Assembly[] assemblies)
        {
            Type baseType = typeof(IDependencyTransient);
            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes().Where(x => baseType.IsAssignableFrom(x)).Where(x => x.IsClass && x.IsAbstract == false))
                {
                    serviceCollection.AddTransient(type);
                }
            }
            return serviceCollection;
        }

        /// <summary>
        /// 将提供的程序集中的所有类使用.net core自带的DI进行注入,生存周期为Scoped
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public static IServiceCollection RegisterAssembliesScoped(this IServiceCollection serviceCollection, params Assembly[] assemblies)
        {
            Type baseType = typeof(IDependencyScoped);
            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes().Where(x => baseType.IsAssignableFrom(x)).Where(x => x.IsClass && x.IsAbstract == false))
                {
                    serviceCollection.AddScoped(type);
                }
            }
            return serviceCollection;
        }
    }
}
