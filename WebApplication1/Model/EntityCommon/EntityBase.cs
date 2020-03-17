using System;
using System.Collections.Generic;
using System.Text;

namespace Model.EntityCommon
{
    public class EntityBase:TableCommon
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime? LastModifyTime { get; set; }

        /// <summary>
        /// 数据状态，(0:失效 || 1:正常)
        /// </summary>
        public int Status { get; set; }
    }
}
