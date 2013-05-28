using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace EARTH.Jaguar
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "GetNewPublicMessages",
                routeTemplate: "api/public/messages/{last}",
                defaults: new { controller = "PublicMessages", action = "GetNewPublicMessages" }
            );

            config.Routes.MapHttpRoute(
                name: "GetOldPublicMessages",
                routeTemplate: "api/old/public/messages/{last}",
                defaults: new { controller = "PublicMessages", action = "GetOldPublicMessages" }
            );

            config.Routes.MapHttpRoute(
                name: "GetNewUserMessages",
                routeTemplate: "api/{userName}/messages/{last}",
                defaults: new { controller = "Messages", action = "GetNewUserMessages" }
            );

            config.Routes.MapHttpRoute(
                name: "GetOldUserMessages",
                routeTemplate: "api/{userName}/old/messages/{last}",
                defaults: new { controller = "Messages", action = "GetOldUserMessages" }
            );

            config.Routes.MapHttpRoute(
                name: "GetYearsByUser",
                routeTemplate: "api/{userName}/years",
                defaults: new { controller = "Years", action = "GetYearsByUser" }
            );

            config.Routes.MapHttpRoute(
                name: "GetPeriodsByYearAndUser",
                routeTemplate: "api/{userName}/{year}/periods",
                defaults: new { controller = "Periods", action = "GetPeriodsByYearAndUser" }
            );

            config.Routes.MapHttpRoute(
                name: "GetGrades",
                routeTemplate: "api/{userName}/{year}/{period}/grades",
                defaults: new { controller = "Grades", action = "GetGrades" }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable<T> return type.
            // To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.
            // For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.
            //config.EnableQuerySupport();

            // To disable tracing in your application, please comment out or remove the following line of code
            // For more information, refer to: http://www.asp.net/web-api
            config.EnableSystemDiagnosticsTracing();
        }
    }
}
