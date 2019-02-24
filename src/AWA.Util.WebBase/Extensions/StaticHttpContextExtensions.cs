using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace AWA.Util.WebBase.Extensions
{
    public static class StaticHttpContextExtensions
    {
        //2.1后，框架会自动注入
        //public static void AddHttpContextAccessor(this IServiceCollection services)
        //{
        //    services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        //}

        public static IApplicationBuilder UseStaticHttpContext(this IApplicationBuilder app)
        {
            var httpContextAccessor = app.ApplicationServices.GetRequiredService<IHttpContextAccessor>();
            HttpContextExtensions.Configure(httpContextAccessor);
            return app;
        }
    }

    public static class HttpContextExtensions
    {
        private static IHttpContextAccessor _accessor;

        public static Microsoft.AspNetCore.Http.HttpContext Current => _accessor.HttpContext;

        internal static void Configure(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }
    }
}