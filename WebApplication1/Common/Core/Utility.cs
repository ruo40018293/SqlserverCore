using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Common.Core
{
    /// <summary>
    /// 一个简单的公共方法工具类
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// 不通类型之间的装换方法
        /// </summary>
        /// <typeparam name="TType"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static TType Parse<TType>(object value, TType defaultValue = default(TType))
        {
            if (value == null || value == DBNull.Value || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return defaultValue;
            }
            Type type = typeof(TType);
            Type underlylingType = Nullable.GetUnderlyingType(type);
            return (TType)Convert.ChangeType(value.ToString(), underlylingType ?? type);
        }

        /// <summary>
        /// 获取Unix时间戳
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static long GetUnixTimeTick(DateTime? date = null)
        {
            if (date == null)
            {
                date = DateTime.Now;
            }
            var timespan = date.Value.Subtract(new DateTime(1970, 1, 1));
            return timespan.Ticks;
        }

        /// <summary>
        /// 获取不通地区的时间字符串
        /// </summary>
        /// <param name="format">字符串格式化标准，默认yyyy/MM/dd hh:mm:ss</param>
        /// <param name="date">要转换的时间，不填则默认当前时间</param>
        /// <param name="country">地区默认中国地区</param>
        /// <returns></returns>
        public static string GetDateString(string format = "yyyy/MM/dd hh:mm:ss", DateTime? date = null, string country = "zn-CN")
        {
            if (!date.HasValue)
            {
                date = DateTime.Now;
            }
            var dateString = date.Value.ToString(format, new CultureInfo(country));
            return dateString;
        }

        /// <summary>
        /// 计算从某刻时刻开始，加上某个时间段后，距离当前时间还剩多长时间，超过一天会以天为单位，超过一个小时会以小时为单位，否则以分钟为单位，精确到小数点后一位
        /// 商城中计算当前订单还剩多久过期时经常使用
        /// </summary>
        /// <param name="startTime">从哪个时间开始算起</param>
        /// <param name="minutes">经过多少分钟后</param>
        /// <returns></returns>
        public static string RemainingTime(DateTime startTime, int minutes)
        {
            string time = "";
            var endtime = startTime.AddMinutes(minutes);
            if (endtime > DateTime.Now)
            {
                TimeSpan now = new TimeSpan(DateTime.Now.Ticks);
                TimeSpan end = new TimeSpan(endtime.Ticks);
                TimeSpan ts = now.Subtract(end).Duration();

                if (ts.TotalMinutes < 60)
                {
                    time = ((int)ts.TotalMinutes).ToString() + "分钟";
                }
                else if (ts.TotalHours >= 1 && ts.TotalDays < 1)
                {
                    time = ts.TotalHours.ToString("0.0") + "小时";
                }
                else if (ts.TotalDays >= 1)
                {
                    time = ts.TotalDays.ToString("0.0") + "天";
                }
            }
            return time;
        }

        /// <summary>
        /// 计算从现在到传入的时间一共过去了多长时间（自动推算年月日）
        /// </summary>
        /// <param name="date">从现在到哪个时间点</param>
        /// <returns></returns>
        public static string DateStringFromNow(DateTime date)
        {
            TimeSpan span = DateTime.Now - date;
            if (span.TotalDays > 60)
            {
                return date.ToShortDateString();
            }
            if (span.TotalDays > 30)
            {
                return "1个月前";
            }
            if (span.TotalDays > 14)
            {
                return "2周前";
            }
            if (span.TotalDays > 7)
            {
                return "1周前";
            }
            if (span.TotalDays > 1)
            {
                return $"{(int)Math.Floor(span.TotalDays)}天前";
            }
            if (span.TotalHours > 1)
            {
                return $"{(int)Math.Floor(span.TotalHours)}小时前";
            }
            if (span.TotalMinutes > 1)
            {
                return $"{(int)Math.Floor(span.TotalMinutes)}分钟前";
            }
            return span.TotalSeconds >= 1 ? $"{(int)Math.Floor(span.TotalSeconds)}秒前" : "刚刚";
        }

        /// <summary>
        /// 生成一个制定范围的随机数,不制定的话默认生成0到int最大值之间的随机数
        /// </summary>
        /// <param name="start_num"></param>
        /// <param name="end_num"></param>
        /// <returns></returns>
        public static int CreateRandom(int start_num = 0, int end_num = int.MaxValue)
        {
            Random random = new Random();
            return random.Next(start_num, end_num);
        }
    }
}
