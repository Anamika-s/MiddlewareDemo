using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MiddlewareDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseCustomExtension();
            app.Map("/newbranch", a => a.Run(c => c.Response.WriteAsync("Running from the newbranch branch!")));

            app.UseWhen(context => context.Request.Query.ContainsKey("role"),
a => a.Use(async (c, next) => {
    var role = c.Request.Query["role"];
    Console.WriteLine($"Role passed {role}");
    await c.Response.WriteAsync($"Role passed {role}");

    // Do work that doesn't write to the Response.
    await next();
}));
           

            app.Use(async (context ,next)  =>
            {
                await context.Response.WriteAsync("Hello, World!");
                  await next();
                await context.Response.WriteAsync("\n This is after another Hello, World!");
            });
            app.Run(async context =>
            {
                await context.Response.WriteAsync("Another Hello, World!");
            });


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
