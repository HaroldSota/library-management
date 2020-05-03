using Microsoft.AspNetCore.Builder;
using LibraryManagement.Core;

namespace LibraryManagement.Api.Framework.Extensions
{
    /// <summary>
    ///     ApplicationBuilderExtensions
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        public static void UseApiRequestPipeline(this IApplicationBuilder app)
        {
            app.UseCors("CorsPolicy");
            app.UseHttpsRedirection();
            app.UseSwagger();

            app.UseSwaggerUI(cfg =>
            {
                cfg.SwaggerEndpoint("swagger/v1/swagger.json", "Library Management API V1");
                cfg.RoutePrefix = string.Empty;
            });

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public static void UseEngineContext(this IApplicationBuilder app)
        {
            EngineContext.Current.SetServiceProvider(app.ApplicationServices);
        }
    }
}
