using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiddlewareDemo
{
    public static class MiddlewareExtension
    {
        public static void UseCustomExtension(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                await context.Response.WriteAsync("Custom logic before next middleware");
                await next();
                await context.Response.WriteAsync("<br>Custom logic after next middleware");
            });
        }


    }
}
