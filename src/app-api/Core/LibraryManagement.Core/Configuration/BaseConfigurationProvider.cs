using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace LibraryManagement.Core.Configuration
{
    public abstract class BaseConfigurationProvider
    {
        protected readonly IConfiguration Configuration;
        private readonly string _prefix;

        protected BaseConfigurationProvider(IConfiguration configuration, string prefix)
        {
            if (!string.IsNullOrEmpty(prefix))
                _prefix = $"{prefix}{(prefix.EndsWith(':') ? string.Empty : ":")}";
            Configuration = configuration;
        }

        protected T GetConfiguration<T>(string selector)
        {
            return Configuration.GetValue<T>(GetQualifiedSelector(selector));
        }

        protected IConfigurationSection GetSection(string selector)
        {
            return Configuration.GetSection(GetQualifiedSelector(selector));
        }

        protected virtual string GetQualifiedSelector(string path) => $"{_prefix}{path}";
    }
}
