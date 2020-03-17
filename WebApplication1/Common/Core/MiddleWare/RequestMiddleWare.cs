using Common.Extension;
using Logger;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Common.Core.MiddleWare
{
    /// <summary>
    /// 处理Request请求的Core中间件
    /// </summary>
    public class RequestMiddleWare
    {
        private readonly RequestDelegate _next;
        public RequestMiddleWare(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext httpContext)
        {
            LogHelper.WriteInfo($"【登录IP】：{httpContext.Connection.RemoteIpAddress}");
            //var token = httpContext.Request.Headers["Token"].ObjToString();
            //if (token.IsEmpty())
            //{

            //    await httpContext.Response.WriteAsync("1");
            //}
            //else
            //{
            //    await _next.Invoke(httpContext);
            //}
            await _next.Invoke(httpContext);
        }
    }
}
