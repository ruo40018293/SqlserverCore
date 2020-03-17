using Common.Core.MiddleWare;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Extension.MiddleWareExtension
{
    public static class RequestMiddleWareEx
    {
        public static IApplicationBuilder UseHandleRequest(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestMiddleWare>();
        }
    }
}
