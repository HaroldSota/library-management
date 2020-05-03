namespace LibraryManagement.Core.Configuration
{
    /// <summary>
    ///     Meta data definition for the application configuration file LibraryManagementConfig section
    /// </summary>
    public interface ILibraryManagementConfig
    {
        /// <summary>
        ///     Determine if the app is called by automated testing
        /// </summary>
        bool IsTesting { get; }

        /// <summary>
        ///     Authentication security key
        /// </summary>
        string AppSecret { get;  }

        /// <summary>
        ///     Database connection string
        /// </summary>
        string DataConnectionString { get; }
    }
}
