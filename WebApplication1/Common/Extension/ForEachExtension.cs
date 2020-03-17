using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Extension
{
    /// <summary>
    /// Forecha扩展方法,根据自己传入的委托执行ForEach
    /// </summary>
    public static class ForEachExtension
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
            {
                action(item);
            }
        }
    }
}
