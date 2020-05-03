using Microsoft.Extensions.Configuration;

namespace LibraryManagement.Core.Configuration
{

    /// <inheritdoc cref="ILibraryManagementConfig" />
    public sealed class LibraryManagementConfig : BaseConfigurationProvider, ILibraryManagementConfig
    {
        /// <summary>
        ///     LibraryManagementConfig ctor.
        /// </summary>
        /// <param name="configuration"></param>
        public LibraryManagementConfig(IConfiguration configuration)
            : base(configuration, "LibraryManagementConfig")
        {
        }

        /// <inheritdoc />
        public bool IsTesting => GetConfiguration<bool>("IsTesting");

        private string _appSecret;
        /// <inheritdoc />
        public string AppSecret => _appSecret ??= GetConfiguration<string>("AppSecret");

        private string _dataConnectionString;

        /// <inheritdoc />
        public string DataConnectionString => _dataConnectionString ??= GetConfiguration<string>("DataConnectionString");
    }
}
