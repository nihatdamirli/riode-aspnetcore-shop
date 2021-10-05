using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Riode.WebUI.Models.DataContexts;
using Riode.WebUI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.AppCode.Middlewears
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class AuditMiddlewear
    {
        private readonly RequestDelegate _next;

        public AuditMiddlewear(RequestDelegate next)
        {
           
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            using (var scope =  httpContext.RequestServices.CreateScope())
            {
                RiodeDbContext db = scope.ServiceProvider.GetRequiredService<RiodeDbContext>();

                var routeData = httpContext.GetRouteData();
                var log = new AuditLog();

                log.CreatedDate = DateTime.Now;
                log.CreatedDate = DateTime.Now;
                log.IsHttps = httpContext.Request.IsHttps;
                log.Method = httpContext.Request.Method;
                log.Path = httpContext.Request.Path;
                if (routeData.Values.TryGetValue("area", out object area))
                {
                    log.Area = area.ToString();
                }

                if (routeData.Values.TryGetValue("controller", out object controller))
                {
                    log.Controller = controller.ToString();
                }

                if (routeData.Values.TryGetValue("action", out object action))
                {
                    log.Action = action.ToString();
                }

                if (!string.IsNullOrWhiteSpace(httpContext.Request.QueryString.Value))
                {
                    log.QueryString = httpContext.Request.QueryString.Value;
                }

                await _next(httpContext);
                log.StatusCode = httpContext.Response.StatusCode;
                log.ResponseTime = DateTime.Now;

                db.AuditLogs.Add(log);
                db.SaveChanges();
            }

            
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class AuditMiddlewearExtensions
    {
        public static IApplicationBuilder UseAuditMiddlewear(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuditMiddlewear>();
        }
    }
}
