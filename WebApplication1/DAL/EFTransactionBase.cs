using Common.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public class EFTransactionBase
    {
        #region 属性
        //public EFDbcontext DbContext { get; set; }
        public EFDbcontext DbContext;

        public EFTransactionBase(EFDbcontext _DbContext) 
        {
            DbContext = _DbContext;
        }
        #endregion

        #region EF事务相关

        /// <summary>
        /// 开启一个事务
        /// </summary>
        public virtual void BeginOrUseTransaction()
        {
            DbContext.Database.BeginTransaction();
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public virtual void CommitTransaction()
        {
            if (DbContext.Database.CurrentTransaction != null)
            {
                DbContext.Database.CommitTransaction();//提交事务
            }
        }

        /// <summary>
        /// 清除所有改动
        /// </summary>
        /// <param name="context"></param>
        public void CleanChanges(DbContext context)
        {
            var entries = DbContext.ChangeTracker.Entries().ToArray();
            foreach (var entry in entries)
            {
                entry.State = EntityState.Detached;
            }
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        public virtual void Rollback()
        {
            if (DbContext.Database.CurrentTransaction != null)
            {
                DbContext.Database.RollbackTransaction();
            }
        }

        #endregion
    }
}
