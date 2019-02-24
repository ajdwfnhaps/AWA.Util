using System;

#if NETCORE
using Microsoft.Extensions.Configuration;
#endif

namespace AWA.Util
{
    /// <summary>
    /// 框架环境变量
    /// </summary>
    public static class GlobalVariable
    {
        public static IServiceProvider ServiceProvider;

#if NETCORE
        /// <summary>
        /// IConfigurationRoot
        /// </summary>
        public static IConfigurationRoot Configuration
        {
            get
            {
                return (IConfigurationRoot)ServiceProvider.GetService(typeof(IConfigurationRoot));

            }
        }

#endif

        static GlobalVariable()
        { }

        /// <summary>
        /// 应用程序日志键名
        /// </summary>
        public const string LoggerKey = "AppLogger";
        /// <summary>
        /// 应用程序数据库日志键名
        /// </summary>
        public const string DbLoggerKey = "AppDbLogger";

        /// <summary>
        /// 允许上传图片最大size
        /// 1左移20位乘5
        /// </summary>
        public const int ImgAllowedLength = 5 * 1 << 20;
    }
}
