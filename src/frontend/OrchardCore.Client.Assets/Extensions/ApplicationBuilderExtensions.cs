using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;

namespace OrchardCore.Client.Assets
{
    public static class ApplicationBuilderExtensions
    {
        public static void AddGovUkAssets(this IApplicationBuilder app)
        {
            var componentAssembly = typeof(ApplicationBuilderExtensions).Assembly;
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new EmbeddedFileProvider(componentAssembly, "OrchardCore.Client.Assets.wwwroot"),
                RequestPath = new PathString("/contents")
            });
        }
    }
}
