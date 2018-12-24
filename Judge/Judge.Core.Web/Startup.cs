using Judge.Application;
using Judge.Web.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;
using SimpleInjector.Integration.AspNetCore.Mvc;
using SimpleInjector.Lifestyles;
using System;
using System.Security.Principal;

namespace Judge.Core.Web
{
    public class Startup
    {
        private Container container = new Container();

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie("Identity.Application");

            IntegrateSimpleInjector(services);

            //return new UnityServiceProvider(container);
        }

        private void IntegrateSimpleInjector(IServiceCollection services)
        {
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            container.RegisterInstance<IServiceProvider>(container);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<IControllerActivator>(
                new SimpleInjectorControllerActivator(container));
            services.AddSingleton<IViewComponentActivator>(
                new SimpleInjectorViewComponentActivator(container));

            services.AddRouting();

            services.EnableSimpleInjectorCrossWiring(container);
            services.UseSimpleInjectorAspNetRequestScoping(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "SolutionSubmit",
                    template: "Problems/Solution/{submitResultId}",
                    defaults: new { controller = "Problems", Action = "Solution" }
                );

                routes.MapRoute(
                    name: "ContestTask",
                    template: "Contests/{contestId}/Task/{label}",
                    defaults: new { controller = "Contests", Action = "Task" }
                );

                routes.MapRoute(
                    name: "ProblemsList",
                    template: "Problems/Page/{page}",
                    defaults: new { controller = "Problems", action = "Index" }
                );
            });

            container.RegisterMvcControllers(app);
            container.RegisterMvcViewComponents(app);

            // Add application services. For instance:
            var connectionString = Configuration.GetConnectionString("connection");
            new ApplicationExtension(connectionString).Configure(container);
            container.Register<IHttpContextAccessor, HttpContextAccessor>(new AsyncScopedLifestyle());
            container.Register<ISessionService, SessionService>(new AsyncScopedLifestyle());
            container.Register<IPrincipal, HttpContextPrinciple>(new AsyncScopedLifestyle());

            app.UseAuthentication();

            // Allow Simple Injector to resolve services from ASP.NET Core.
            container.AutoCrossWireAspNetComponents(app);
        }
    }
}
