using AWA.Util.Abstract;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AWA.Util.WebBase.Middleware
{
    /// <summary>
    /// 自定义封装框架中间件扩展
    /// </summary>
    public static class BuilderExtensions
    {
        /// <summary>
        /// 注入IP黑名单中间件所需的服务
        /// </summary>
        /// <param name="services"></param>
        public static void AddIPBlackList(this IServiceCollection services)
        {
            services.AddSingleton<IIPListStoreProvider>(new DefaultIPListStoreProvider());
        }

        /// <summary>
        /// 使用IP黑名单中间件
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseIPBlackList(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<IPBlackListMiddleware>();
        }

        /// <summary>
        /// 使用全局异常处理中间件
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseGlobalErrorHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GlobalErrorHandlingMiddleware>();
        }
    }
}