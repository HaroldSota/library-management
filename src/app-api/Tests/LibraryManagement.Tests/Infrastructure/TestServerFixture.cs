using LibraryManagement.Api;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net.Http;

namespace LibraryManagement.Tests.Infrastructure
{
    public sealed class TestServerFixture : IDisposable
    {

        #region [ Fields ]


        private static readonly TestServer _testServer;

        #endregion

        #region  [ Ctor ]
        static TestServerFixture()
        {
            var builder = new WebHostBuilder()
               .ConfigureServices(services => services.AddAutofac())
                .UseEnvironment("Testing")
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.Tests.json"));
                })
                .UseStartup<Startup>();
         
            _testServer = new TestServer(builder);
            
        }

        public TestServerFixture()
        {
            Client = _testServer.CreateClient();
        }
        #endregion

        #region [ Peroperties ]

        public HttpClient Client { get; private set;}

        #endregion

        #region [ IDisposable: Implementation ]
        public void Dispose()
        {
            Client.Dispose();
        }

        #endregion
    }
}
