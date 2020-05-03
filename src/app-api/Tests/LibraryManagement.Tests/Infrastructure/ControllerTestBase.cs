using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using LibraryManagement.Application.Model.UserModel;
using Xunit;

namespace LibraryManagement.Tests.Infrastructure
{
    public abstract class ControllerTestBase : IClassFixture<TestServerFixture>
    {
        protected readonly TestServerFixture _fixture;
        private string _controllPath;

        protected ControllerTestBase(TestServerFixture fixture, string controllPath)
        {
            _fixture = fixture;
            _controllPath = controllPath;
        }

        public string GetQueryString(object obj)
        {
            var properties = from p in obj.GetType().GetProperties()
                             where p.GetValue(obj, null) != null
                             select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(obj, null).ToString());

            return string.Join("&", properties.ToArray());
        }

        protected async Task<HttpResponseMessage> GetAsync(string action)
        {
            return await _fixture.Client.GetAsync($"{_controllPath}{action}");
        }

        protected async Task<HttpResponseMessage> GetAsync(string action, string token)
        {
            _fixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return await _fixture.Client.GetAsync($"{_controllPath}{action}");
        }

        protected async Task<HttpResponseMessage> GetAsync(string action, object obj)
        {
            return await _fixture.Client.GetAsync($"{_controllPath}{action}?{GetQueryString(obj)}");
        }

        protected async Task<HttpResponseMessage> PostAsync<T>(string action, T @object)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{_controllPath}{action}")
            {
                Content = @object.ToJsonContent()
            };
            return await _fixture.Client.SendAsync(requestMessage);
        }

        protected async Task<HttpResponseMessage> PostAsync<T>(string action, string token,T @object)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{_controllPath}{action}")
            {
                Content = @object.ToJsonContent()
            };

            _fixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return await _fixture.Client.SendAsync(requestMessage);
        }

        protected async Task<HttpResponseMessage> PostFromQueryStringAsync(string action, string token)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{_controllPath}{action}");

            _fixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return await _fixture.Client.SendAsync(requestMessage);
        }
        protected async Task<string> GetAuthenticationToken(string username, string password)
        {

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/api/User/Authenticate")
            {
                Content = new AuthenticateRequest
                {
                    Username = username,
                    Password = password
                }.ToJsonContent()
            };
            var response = await _fixture.Client.SendAsync(requestMessage);
            //Assert
            var result = response.GetObject<AuthenticateResponse>();
            return result.Token;
        }
    }
}