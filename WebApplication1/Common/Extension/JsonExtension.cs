using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Common.Extension
{
    public static class JsonExtension
    {
        /// <summary>
        /// Json字符串转换成对象
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static object ToObject(this string json)
        {
            return json == null ? null : JsonConvert.DeserializeObject(json);
        }

        /// <summary>
        /// 对象转成Json
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson(this object obj)
        {
            var timeConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
            return JsonConvert.SerializeObject(obj, timeConverter);
        }

        /// <summary>
        /// 根据指定的时间格式，把对象中的日期字段转化成你制定的格式
        /// </summary>
        /// <param name="obj">带Date的对象</param>
        /// <param name="datetimeformats">时间格式：yyyy-MM-dd</param>
        /// <returns></returns>
        public static string ToJson(this object obj, string datetimeformats)
        {
            var timeConverter = new IsoDateTimeConverter { DateTimeFormat = datetimeformats };
            return JsonConvert.SerializeObject(obj, timeConverter);
        }

        /// <summary>
        /// 把Json序列化成一个制定类型的对象，如果不传json默认初始化
        /// </summary>
        /// <typeparam name="T">制定的对象类型</typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T ToObject<T>(this string json)
        {
            return json == null ? default : JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// Json反序列化为List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this string json)
        {
            return json == null ? null : JsonConvert.DeserializeObject<List<T>>(json);
        }

        /// <summary>
        /// Json反序列为DataTable
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static DataTable ToTable(this string json)
        {
            return json == null ? null : JsonConvert.DeserializeObject<DataTable>(json);
        }
    }
}
