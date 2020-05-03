using System;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.Core
{
    /// <inheritdoc />
    public class EngineContext : IEngine
    {
        public static IEngine Current
        {
            get
            {
                if (Singleton<IEngine>.Instance == null)
                {
                    Create();
                }

                return Singleton<IEngine>.Instance;
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private static IEngine Create()
        {
            return Singleton<IEngine>.Instance ?? (Singleton<IEngine>.Instance = new EngineContext());
        }

        private IServiceProvider ServiceProvider { get; set; }

        /// <inheritdoc />
        public void SetServiceProvider(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        /// <summary>
        ///     Get IServiceProvider
        /// </summary>
        /// <returns>IServiceProvider</returns>
        private IServiceProvider GetServiceProvider()
        {
            var accessor = ServiceProvider.GetService<IHttpContextAccessor>();
            var context = accessor.HttpContext;
            return context?.RequestServices ?? ServiceProvider;
        }

        /// <inheritdoc />
        public T Resolve<T>() where T : class
        {
            return (T)GetServiceProvider().GetRequiredService(typeof(T));
        }
    }
}
