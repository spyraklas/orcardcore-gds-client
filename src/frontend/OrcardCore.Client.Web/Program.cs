using Microsoft.AspNetCore;

namespace OrcardCore.Client.Web
{
    /// <summary>
    /// Main application entry point.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main application entry point.
        /// </summary>
        /// <param name="args">args.</param>
        /// <returns>IWebHostBuilder.</returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args)
                .Build()
                .Run();
        }
    }
}
