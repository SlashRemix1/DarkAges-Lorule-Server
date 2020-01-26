using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Darkages;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Web
{
    public class Startup
    {
        public static Instance _ServerIntance;

        public class Instance : ServerContext
        {
            public Instance() : base()
            {
                LoadConstants();
            }
        }

        public static string SERVER_CONFIG => Path.Combine(Environment.CurrentDirectory, "ServerConfig");
        public static string SERVER_TBL    => Path.Combine(SERVER_CONFIG, "server.tbl");
        public static string SERVER_DAT    => Path.Combine(SERVER_CONFIG, "sotp.dat");
        public static string SERVER_XML    => Path.Combine(SERVER_CONFIG, "MServerTable.xml");
        public static string NEWS_FILE     => Path.Combine(SERVER_CONFIG, "news.txt");

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            _ServerIntance = new Instance();
            _ServerIntance.Start(
                    SERVER_TBL,
                    SERVER_DAT,
                    SERVER_XML,
                    NEWS_FILE
                );
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
