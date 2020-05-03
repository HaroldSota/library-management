using System.Text.Json;
using System.Text.Json.Serialization;

namespace LibraryManagement.Tests.Infrastructure
{
    public static class ClientSerializer
    {
        private static readonly JsonSerializerOptions serializerOptions;
        static ClientSerializer()
        {
            serializerOptions= new JsonSerializerOptions
            {
                IgnoreNullValues = true
            };
        }

        public static T Deserialize<T>(string jsonString)
        {
            return JsonSerializer.Deserialize<T>(jsonString, serializerOptions);
        }

        public static string Serialize<T>(T @object)
        {
            return JsonSerializer.Serialize(@object, serializerOptions);
        }
    }
}
