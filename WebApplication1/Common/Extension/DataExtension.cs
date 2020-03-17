using Model.EntityCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Extension
{
    public static class DataExtension
    {
        /// <summary>
        /// 返回实体类中Status为1的数据
        /// </summary>
        /// <typeparam name="Entity"></typeparam>
        /// <param name="list">实体集合</param>
        /// <returns></returns>
        public static List<Entity> GetUsed<Entity>(this List<Entity> list) where Entity : EntityBase
        {
            return list.Where(l => l.Status == 1).ToList();
        }

        /// <summary>
        /// 如果该实体的Status为1则返回，否则返回空
        /// </summary>
        /// <typeparam name="Entity"></typeparam>
        /// <param name="entity">实体</param>
        public static Entity GetUsed<Entity>(this Entity entity) where Entity : EntityBase
        {
            if (entity.Status == 1)
            {
                return entity;
            }
            else
            {
                return null;
            }
        }
    }
}
