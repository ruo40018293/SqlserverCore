using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
namespace Common.Core
{
    /// <summary>
    /// 配置文件读取帮助类
    /// </summary>
    public class ConfigHelper
    {
        /// <summary>
        /// 此变量请在startup中赋值
        /// </summary>
        public static IConfiguration Configs;

        public static TVal GetValue<TVal>(string key)
        {
            var res = Utility.Parse<TVal>(Configs[key]);
            return res;
        }
    }
}
