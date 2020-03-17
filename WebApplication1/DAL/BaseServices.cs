using DAL.InterFace;
using Model.EntityCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Enum;
using Common.Core;

namespace DAL
{
    public class BaseServices<TEntity> : DbBase<TEntity>, IValidate<TEntity> where TEntity : EntityBase, new()
    {
        #region 构造函数
        public IEFDbcontext DbContext { get; }

        public BaseServices(EFDbcontext dbContext) : base()
        {
            

        }
        public BaseServices() : base()
        {

        }
        #endregion

        #region 各个表通用属性的处理
        /// <summary>
        /// 对象实体集合处理方法
        /// 该方法目前只做了简单的处理，例如第一次新增时添加创建时间，修改时更改最后的更改时间
        /// 使用者可以根据自己的业务进行扩展，例如插入人是谁等等
        /// </summary>
        /// <param name="entities">要新增修改的实体集合</param>
        /// <param name="initAdd">是否为新增</param>
        /// <param name="maxId">之前数据的最大主键值</param>
        /// <returns></returns>
        private IList<TEntity> WrapEntities(IList<TEntity> entities, DataOperation DataType, long maxId = 0)
        {
            foreach (var entity in entities)
            {
                if (entity.LastModifyTime == null)
                {
                    entity.LastModifyTime = DateTime.Now;
                }
                if (entity.CreatedTime == null && DataType == DataOperation.Add)
                {
                    entity.CreatedTime = DateTime.Now;
                }

                if (DataType == DataOperation.Delete)
                    entity.Status = 0;
                else
                    entity.Status = 1;
            }
            return entities;
        }
        #endregion

        #region 增删改

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity">业务实体类</param>
        /// <param name="save">是否立刻保存</param>
        public virtual ResultModel Add<TMapEntity>(TMapEntity entity, bool save = true)
        {
            var data = Mapper.Map<TMapEntity, TEntity>(entity);
            if (data.hasError)
            {
                return new ResultModel { Success = false, Message = "对象装配失败" };
            }
            var list = new List<TEntity>();
            list.Add(data.target);
            //获取之前数据的最大ID值
            long maxId = Queryable.Count() > 0 ? Queryable.Max(x => x.Id) : 0;
            return AddEntity(WrapEntities(list, DataOperation.Add, maxId)[0], save);
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">业务实体类</param>
        /// <param name="save">是否立刻保存</param>
        public virtual ResultModel Update<TMapEntity>(TMapEntity entity, bool save = true)
        {
            var data = Mapper.Map<TMapEntity, TEntity>(entity);
            if (data.hasError)
            {
                return new ResultModel { Success = false, Message = "对象装配失败" };
            }
            var list = new List<TEntity>();
            list.Add(data.target);
            return Edit(WrapEntities(list, DataOperation.Update)[0], save);
        }

        public virtual ResultModel Remove(long Id)
        {
            return Remove(Id);
        }

        #endregion

        #region 查询
        public virtual IList<TEntity> GetList()
        {
            return base.GetList();
        }
        public virtual (IList<TEntity>, int rows_count) GetList(int pageIndex, int pageSize)
        {
            int rows = 0;
            return (GetList(pageSize, pageIndex, out rows), rows);
        }
        #endregion

        #region 数据验证接口
        public virtual ResultModel Validate(TEntity entity)
        {
            return new ResultModel { Success = true, Message = "该实体类未实现验证方法,默认返回True" };
        }
        #endregion
    }
}
