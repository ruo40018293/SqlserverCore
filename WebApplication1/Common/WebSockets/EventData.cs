using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.WebSockets
{
    [Serializable]
    public class EventData<TData>
    {
        /// <summary>
        /// 事件名称
        /// </summary>
        /// <value></value>
        [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
        public Events Event { get; set; }

        /// <summary>
        /// 事件参数
        /// </summary>
        /// <value></value>
        public TData Data { get; set; }
    }
}
