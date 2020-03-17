using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.WebSockets
{
    public class MessageEntity
    {
        /// <summary>
        /// 消息发送者
        /// </summary>
        /// <value></value>
        public string Sender { get; set; }

        /// <summary>
        /// 消息接收者
        /// </summary>
        /// <value></value>
        public string Receiver { get; set; }

        /// <summary>
        /// 发送时间
        /// </summary>
        /// <value></value>
        public DateTime SendTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 消息内容
        /// </summary>
        /// <value></value>
        public string Message { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        /// <value></value>
        [JsonConverter(typeof(StringEnumConverter))]
        public MessageType Type { get; set; }
    }
}
