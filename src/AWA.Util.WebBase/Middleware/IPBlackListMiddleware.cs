using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using AWA.Util.Enums;
using AWA.Util.Abstract;
using AWA.Util.Extensions;

namespace AWA.Util.WebBase.Middleware
{
    /// <summary>
    /// IP黑名单中间件
    /// </summary>
    public class IPBlackListMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly IIPListStoreProvider _ipListStoreProvider;

        public IPBlackListMiddleware(RequestDelegate next
            , ILoggerFactory loggerFactory
            , IIPListStoreProvider ipListStoreProvider)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<IPBlackListMiddleware>();
            _ipListStoreProvider = ipListStoreProvider;
        }

        public async Task Invoke(HttpContext context)
        {
            //获取客户端ip
            var remoteIpAddress = context.Connection.RemoteIpAddress.ToString();

            if(_ipListStoreProvider.BlackList.Contains("*") && !_ipListStoreProvider.WhiteList.Any(i => i == remoteIpAddress))
            {
                var msg = $"访问白名单不存在IP:{remoteIpAddress}，拒绝访问！";
                _logger.LogWarning(msg);
                context.Response.ContentType = "text/html; charset=utf-8";
                await context.Response.WriteAsync(msg);
                return;
            }


            //是否在ip黑名单中
            if (_ipListStoreProvider.BlackList.Any(i => i == remoteIpAddress) && !_ipListStoreProvider.WhiteList.Any(i => i == remoteIpAddress))
            {
                _logger.LogWarning(string.Format("黑名单IP:{0}请求访问，IP黑名单中间件已短路Pipeline，响应不允许访问信息！", remoteIpAddress));

                context.Response.ContentType = "text/html; charset=utf-8";
                await context.Response.WriteAsync(string.Format("IP:{0}已被列为访问黑名单，获取更多信息请联系管理！", remoteIpAddress));
                return;

            }


            //_logger.LogDebug("IP黑名单中间件处理中: 请求IP：" + remoteIpAddress);
            await _next.Invoke(context);
            //_logger.LogDebug("已通过IP黑名单中间件认证.请求IP：" + remoteIpAddress);
        }

    }

    /// <summary>
    /// 默认IP名单存储提供者
    /// </summary>
    public class DefaultIPListStoreProvider : IIPListStoreProvider
    {
        public IList<string> BlackList
        {
            get
            {
                return "IPList:BlackList".ValueOfConfig().Split(',').ToList();
            }
        }

        public IList<string> WhiteList
        {
            get
            {
                return "IPList:WhiteList".ValueOfConfig().Split(',').ToList();
            }
        }

        public bool Add(string ip, IPListType listType = IPListType.Black)
        {
            throw new NotImplementedException();
        }
    }
}