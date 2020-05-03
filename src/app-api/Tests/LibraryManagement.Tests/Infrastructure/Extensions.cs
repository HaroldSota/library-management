using System.Net.Http;

namespace LibraryManagement.Tests.Infrastructure
{
    public static class Extensions
    {
        public static HttpContent ToJsonContent<T>(this T @object)
        {
            var jsonString = ClientSerializer.Serialize(@object);
            return new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");
        }
    }
}
