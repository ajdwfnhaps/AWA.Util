using System;
using CSRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AWA.Util.CSRedis
{
    public static class BuilderExtensions
    {
        /// <summary>
        /// 添加CSRedis 普通模式
        /// </summary>
        /// <param name="services"></param>
        /// <param name="opt"></param>
        /// <returns></returns>
        public static IServiceCollection AddCSRedis(this IServiceCollection services, Action<Option> opt = null)
        {
            var config = services.BuildServiceProvider().GetService<IConfiguration>();

            var defaultOpt = new Option
            {
                ConnectionString = config.GetSection("Redis:Host")?.Value
            };
            opt?.Invoke(defaultOpt);

            if (string.IsNullOrEmpty(defaultOpt.ConnectionString) || defaultOpt.ConnectionString.Split(',').Length <= 1)
                throw new ArgumentException("找不到Redis配置或Redis配置不正确");

            var cSRedisClient = new CSRedisClient(defaultOpt.ConnectionString);

            services.AddSingleton(cSRedisClient);
            services.AddScoped<ICSRedisManager, CSRedis>();
            return services;
        }

        /// <summary>
        /// 添加CSRedis 哨兵模式
        /// </summary>
        /// <param name="services"></param>
        /// <param name="opt"></param>
        /// <returns></returns>
        public static IServiceCollection AddCSRedis(this IServiceCollection services, SentinelOption opt)
        {
            var cSRedisClient = new CSRedisClient(opt.ConnectionString, opt.Sentinels);

            services.AddSingleton(cSRedisClient);
            services.AddScoped<ICSRedisManager, CSRedis>();
            return services;
        }

        /// <summary>
        /// 添加CSRedis 分区模式
        /// </summary>
        /// <param name="services"></param>
        /// <param name="NodeRule"></param>
        /// <param name="connectionStrings"></param>
        /// <returns></returns>
        public static IServiceCollection AddCSRedis(this IServiceCollection services, Func<string, string> NodeRule, params string[] connectionStrings)
        {
            var cSRedisClient = new CSRedisClient(NodeRule, connectionStrings);

            services.AddSingleton(cSRedisClient);
            services.AddScoped<ICSRedisManager, CSRedis>();
            return services;
        }

    }
}
