using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Proxies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Microsoft.EntityFrameworkCore.Infrastructure;
using HelpDesk.Models;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace HelpDesk
{
    public class Startup
    {
        public IConfiguration configuration { get; }

        public Startup(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //Security Configuration

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Login/Index";
                    options.LogoutPath = "/LogIn/SignOut";
                    options.AccessDeniedPath = "/LogIn/AccessDenied";
                });

            services.AddSession();
            
            services.AddMvc().AddSessionStateTempDataProvider();
            
            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
            });

            var connection = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<HelpDeskContext>(options => options.UseLazyLoadingProxies
                ().UseSqlServer(connection));

        }

        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            app.UseSession();

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                     name: "default",
                    template: "{controller=LogIn}/{action=Index}/{id?}");
            });
        }





    }

}

