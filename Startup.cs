using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity.MongoDB;
using Microsoft.Extensions.Logging;
using M101N.Models;
using M101N.Services;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Http;

namespace M101N
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile($"hosting.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see https://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets<Startup>();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connstring = Configuration.GetSection("MongoConnection:ConnectionString").Value;
            services.Configure<Settings>(options =>
                {
                    options.ConnectionString = connstring;
                    options.Database = Configuration.GetSection("MongoConnection:Database").Value;
                });
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            // Add framework services.
            services.AddIdentityWithMongoStoresUsingCustomTypes<ApplicationUser, IdentityRole>(Configuration.GetSection("MongoConnection:IdentityString").Value);
            services.AddMvc();
            services.Configure<IdentityOptions>(options => 
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 4;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            });
            // Add application services.
            services.AddScoped<Microsoft.AspNetCore.Identity.IUserClaimsPrincipalFactory<ApplicationUser>, AppClaimsPrincipalFactory>();            
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            services.AddTransient<BlogContext, BlogContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseIdentity();

            // Add external authentication middleware below. To configure them please see https://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
