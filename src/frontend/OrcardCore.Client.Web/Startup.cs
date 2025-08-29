using OrcardCore.Client.Web.Repositories;
using OrchardCore.Client.Assets;
using OrchardCore.Client.Core.Extensions;

namespace OrcardCore.Client.Web
{
    public class Startup
    {
        #region Contractor & Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup" /> class.
        /// </summary>
        /// <param name="configuration">configuration.</param>
        /// <param name="logger">The logger.</param>
        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            Logger = logger;
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        private IConfiguration Configuration { get; }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        private ILogger<Startup> Logger { get; }
        #endregion

        #region Public Functions

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">Web application builder.Build().</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.AddGovUkAssets();
            app.UseAuthorization();
            app.UseSession();

            if (!env.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            ConfigureRouting(app);
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">Service collection.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            SetConfiguration(services);
            SetDI(services);
            SetMVC(services);
        }

        #endregion

        #region Private functions setting services

        /// <summary>
        /// Set configuration setting classes.
        /// </summary>
        /// <param name="services">Service collection of the application.</param>
        private void SetConfiguration(IServiceCollection services)
        {
            services.AddOrchardCoreClient(Configuration);
        }

        /// <summary>
        /// Set MVC settings.
        /// </summary>
        /// <param name="services">Service collection of the application.</param>
        private void SetMVC(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddControllersWithViews();
            services.AddHttpContextAccessor();
        }

        /// <summary>
        /// Set depentency injection.
        /// </summary>
        /// <param name="services">Service collection of the application.</param>
        private void SetDI(IServiceCollection services)
        {
            services.AddScoped<IPageRepository, PageRepository>();
        }

        #endregion

        #region Private functions setting application builder

        /// <summary>
        /// Sets the rooting table of the application.
        /// </summary>
        /// <param name="app">Application builder.</param>
        private void ConfigureRouting(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }

        #endregion

    }
}
