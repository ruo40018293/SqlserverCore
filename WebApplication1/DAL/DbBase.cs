using Common.Core;
using Common.Data;
using Microsoft.EntityFrameworkCore;
using Model.EntityCommon;
using Model.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

namespace DAL
{
    public class DbBase<Entity> : IDbBase<Entity> where Entity : EntityBase, new()
    {
        #region 属性
        /// <summary>
        /// EF上下文，并且确保在当前线程下是同一个上下文
        /// </summary>
        //private EFDbcontext DbContext
        //{
        //    get
        //    {
        //        var obj = ForCallContext.GetData("DbContext");
        //        if (obj == null)
        //        {
        //            obj = DIHelper.ServiceProvider.GetService(typeof(EFDbcontext)) as EFDbcontext;
        //            //dbcontext = DIHelper.ServiceProvider.GetService(typeof(EFDbcontext)) as EFDbcontext;
        //            ForCallContext.SetData("DbContext", obj);
        //        }
        //        return (EFDbcontext)obj;
        //    }
        //    set { DbContext = value; }
        //}

        protected EFDbcontext DbContext;
        #endregion

        #region 构造函数

        public DbBase(EFDbcontext _DbContext)
        {
            DbContext = _DbContext;
        }
        
        #endregion

        #region 查询

        #region 获取集合
        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <param name="condition">筛选表达式</param>
        public virtual IList<Entity> GetList(Expression<Func<Entity, bool>> condition)
        {
            if (condition != null)
            {
                return this.GetQueryable().Where(condition).ToList();
            }
            return this.GetQueryable().ToList();
        }

        /// <summary>
        /// 获取所有数据集合
        /// </summary>
        public virtual IList<Entity> GetList()
        {
            return this.GetQueryable().ToList();
        }

        /// <summary>
        /// 获取数据集合并排序
        /// </summary>
        /// <typeparam name="TVal">要排序字段的类型</typeparam>
        /// <param name="condition">筛选条件表达式</param>
        /// <param name="orderBy">排序表达式</param>
        /// <param name="direction">升序降序</param>
        public virtual IList<Entity> GetList<TVal>(Expression<Func<Entity, bool>> condition, Expression<Func<Entity, TVal>> orderBy, ListSortDirection direction = ListSortDirection.Descending)
        {
            IList<Entity> res = null;
            if (direction == ListSortDirection.Ascending)
            {
                res = GetQueryable().Where(condition).OrderBy(orderBy).ToList();
            }
            else
            {
                res = GetQueryable().Where(condition).OrderByDescending(orderBy).ToList();
            }
            return res;
        }

        /// <summary>
        /// 分页获取数据集合
        /// </summary>
        /// <typeparam name="TVal"></typeparam>
        /// <param name="rows">筛选后的数据总数量</param>
        /// <param name="condition">筛选条件表达式</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="pageSize">每页数据条数</param>
        /// <param name="orderBy">排序表达式</param>
        /// <param name="direction">升序降序</param>
        public virtual IList<Entity> GetList<TVal>(out int rows, Expression<Func<Entity, bool>> condition = null, int pageIndex = 1, int pageSize = 20, Expression<Func<Entity, TVal>> orderBy = null, ListSortDirection direction = ListSortDirection.Descending)
        {
            var queryable = GetQueryable();
            if (condition != null)
            {
                queryable = queryable.Where(condition);
            }
            rows = queryable.Count();
            if (orderBy != null)
            {
                if (direction == ListSortDirection.Descending)
                    queryable = queryable.OrderByDescending(orderBy);
                else
                    queryable = queryable.OrderBy(orderBy);
            }
            else
            {
                if (direction == ListSortDirection.Descending)
                    queryable = queryable.OrderByDescending(x => x.Id);
                else
                    queryable = queryable.OrderBy(x => x.Id);
            }
            queryable = queryable.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            var res = queryable.ToList();
            return res;
        }

        /// <summary>
        /// 简单分页，不需要排序
        /// </summary>
        /// <param name="pageSize">每页数据条数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="rows">数据总数量</param>
        public virtual IList<Entity> GetList(int pageSize, int pageIndex, out int rows)
        {
            var res = GetList<long>(rows: out rows, condition: null, pageIndex: pageIndex, pageSize: pageSize, orderBy: x => x.Id, direction: ListSortDirection.Descending);
            return res;
        }

        #endregion

        #region 获取实体
        /// <summary>
        /// 根据Id获取实体
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public virtual Entity Get(long id)
        {
            return GetList(l=>l.Id.Equals(id)).FirstOrDefault();
        }

        /// <summary>
        /// 根据表达式获取实体
        /// </summary>
        /// <param name="condition">表达式</param>
        public virtual Entity Get(Expression<Func<Entity, bool>> condition)
        {
            return GetQueryable().FirstOrDefault(condition);
        }


        #endregion

        #region 其他

        /// <summary>
        /// 获取指定实体的Queryable
        /// </summary>
        public IQueryable<Entity> Queryable
        {
            get { return DbContext.Set<Entity>(); }
        }

        public virtual int GetCount(Expression<Func<Entity, bool>> condition)
        {
            if (condition != null)
            {
                return this.GetQueryable().Count(condition);
            }
            else
            {
                return this.GetQueryable().Count();
            }
        }

        /// <summary>
        /// 获取集合
        /// </summary>
        /// <returns>IQueryable集合</returns>
        public virtual IQueryable<Entity> GetQueryable()
        {
            return DbContext.Set<Entity>();
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <typeparam name="TVal"></typeparam>
        /// <param name="condition">表达式</param>
        /// <param name="selector">选择器</param>
        public virtual TVal GetValue<TVal>(Expression<Func<Entity, bool>> condition, Expression<Func<Entity, TVal>> selector)
        {
            var res = GetQueryable().Where(condition).Select(selector).FirstOrDefault();
            return res;
        }

        /// <summary>
        /// 获取数据集
        /// </summary>
        /// <typeparam name="TVal"></typeparam>
        /// <param name="condition"></param>
        /// <param name="selector"></param>
        public virtual IList<TVal> GetValues<TVal>(Expression<Func<Entity, bool>> condition, Expression<Func<Entity, TVal>> selector)
        {
            var res = GetQueryable().Where(condition).Select(selector).ToList();
            return res;
        }

        #endregion

        #endregion

        #region 增删改

        #region 各个表通用属性的处理
        /// <summary>
        /// 对象实体集合处理方法
        /// 该方法目前只做了简单的处理，例如第一次新增时添加创建时间，修改时更改最后的更改时间
        /// 使用者可以根据自己的业务进行扩展，例如插入人是谁等等
        /// </summary>
        /// <param name="entities">要新增修改的实体集合</param>
        /// <param name="initAdd">是否为新增</param>
        /// <param name="maxId">之前数据的最大主键值</param>
        private Entity WrapEntities(Entity entity, DataOperation DataType)
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
            if (DataType == DataOperation.Add)
                entity.Status = 1;

            return entity;
        }
        #endregion

        #region 增加
        /// <summary>
        /// 批量插入某个实体的数据
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <param name="save">是否保存</param>
        /// <param name="skipInvalid">是否跳过验证</param>
        public virtual ResultModel AddList(IList<Entity> entities, bool save = true, bool skipInvalid = true)
        {
            var validator = this as IValidate<Entity>;
            if (validator != null)
            {
                foreach (var item in entities)
                {
                    var entity = WrapEntities(item,DataOperation.Add);
                    var valiRes = validator.Validate(entity);
                    if (valiRes.Success)
                    {
                        DbContext.Set<Entity>().Add(entity);
                    }
                    else
                    {
                        if (!skipInvalid)
                        {
                            return valiRes;
                        }
                    }
                }
            }
            else
            {
                DbContext.Set<Entity>().AddRange(entities);
            }
            if (save)
            {
                var res = Commit();
                if (res.Success)
                {
                    res.Data = entities.Select(x => x.Id).ToList();
                }
                return res;
            }
            return new ResultModel();
        }

        /// <summary>
        /// 为某个实体在数据库中插入一条数据
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="save">是否保存</param>
        public virtual ResultModel Add(Entity entity, bool save = true)
        {
            //检测当前示例是否实现了数据验证接口，如果实现了，进行数据校验
            var validator = this as IValidate<Entity>;
            if (validator != null)
            {
                var valiRes = validator.Validate(entity);
                if (!valiRes.Success)
                {
                    return valiRes;
                }
            }
            entity=WrapEntities(entity, DataOperation.Add);
            DbContext.Add<Entity>(entity);
            if (save)
            {
                var res = Commit();
                //像EF提交持久化以后，把更改数据的ID返回去
                res.Data = entity.Id;
                return res;
            }
            return new ResultModel();
        }

        public virtual ResultModel AddOrEdit(Entity entity, bool save = true)
        {
            if (entity.Id != 0)
            {
                return Add(entity, save);
            }
            else
            {
                return Edit(entity, save);
            }
        }
        #endregion

        #region 修改

        public virtual ResultModel Edit(Entity entity, bool save = true)
        {
            var validator = this as IValidate<Entity>;
            if (validator != null)
            {
                var valiRes = validator.Validate(entity);
                if (!valiRes.Success)
                {
                    return valiRes;
                }
            }
            entity=WrapEntities(entity, DataOperation.Update);
            DbContext.Set<Entity>().Update(entity);
            if (save)
            {
                var res = Commit();
                return res;
            }
            return new ResultModel();
        }

        #endregion

        #region 删除
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="condition">linq筛选表达式</param>
        /// <param name="save">是否保存</param>
        public virtual ResultModel RemoveList(Expression<Func<Entity, bool>> condition,bool IsLogic=true, bool save = true)
        {
            var remove = DbContext.Set<Entity>().Where(condition).ToList();
            for(int i = 0; i < remove.Count; i++)
            {
                Remove(remove[i], IsLogic,false);
            }
            if (save)
            {
                var res = Commit();
                return res;
            }
            else
                return new ResultModel();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="save">是否逻辑删除</param>
        /// <param name="save">是否保存</param>
        public virtual ResultModel Remove(long id, bool IsLogic = true, bool save = true)
        {
            var remove = DbContext.Set<Entity>().FirstOrDefault(x => x.Id == id);
            if (remove != null)
            {
                if (IsLogic)
                {
                    remove.Status = 0;
                    return Edit(remove);
                }
                else
                {
                    DbContext.Set<Entity>().Remove(remove);
                }
            }
            
            if (save)
            {
                var res = Commit();
                return res;
            }
            else
                return new ResultModel();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="save">是否逻辑删除</param>
        /// <param name="save">是否保存</param>
        public virtual ResultModel Remove(Entity entity, bool IsLogic = true, bool save = true)
        {
            if (entity != null)
            {
                if (IsLogic)
                {
                    entity.Status = 0;
                    return Edit(entity);
                }
                else
                {
                    DbContext.Set<Entity>().Remove(entity);
                }
            }

            if (save)
            {
                var res = Commit();
                return res;
            }
            else
                return new ResultModel();
        }

        #endregion

        #endregion

        #region 执行SQL语句

        /// <summary>
        /// 执行Sql语句，并且返回影响的行数
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public virtual ResultModel ExcuteSql(string sql)
        {
            var res = DbContext.Database.ExecuteSqlCommand(sql);
            return new ResultModel { Success = true, Data = res };
        }

        /// <summary>
        /// 执行Sql查询语句，并且返回DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual ResultModel ExcuteSqlReturnDataTable(string sql, params SqlParameter[] parameters)
        {
            var connect = DbContext.Database.GetDbConnection();
            DataTable dt = new DataTable();
            using (SqlConnection conn = connect as SqlConnection)
            {
                using (SqlCommand selectCmd = new SqlCommand())
                {
                    selectCmd.CommandType = CommandType.Text;
                    selectCmd.Connection = conn;
                    selectCmd.CommandText = sql;
                    selectCmd.CommandTimeout = 30;
                    if (null != parameters && parameters.Length > 0)
                    {
                        selectCmd.Parameters.AddRange(parameters);
                    }
                    using (SqlDataAdapter adapter = new SqlDataAdapter(selectCmd))
                    {
                        DataSet ds = new DataSet();
                        adapter.Fill(ds, "DataTable");
                        dt = ds.Tables["DataTable"];
                        if (null != selectCmd && null != selectCmd.Parameters)
                        {
                            selectCmd.Parameters.Clear();
                        }
                    }
                }
            }


            return new ResultModel { Success = true, Data = dt };
        }

        #endregion

        #region 保存
        public virtual ResultModel Commit()
        {
            var result = new ResultModel();
            try
            {
                var value_count = DbContext.SaveChanges();
                result.Data = value_count;
                if (value_count > 0)
                {
                    result.Success = true;
                }
                else
                {
                    result.Success = false;
                    result.Message = "没有保存任何数据";
                }
            }
            catch (DbUpdateException db_Ex)
            {
                result.Success = false;
                result.Message = db_Ex.Message;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message += ex.Message;
            }
            return result;
        }

        

        #endregion

        #region 数据验证接口
        public virtual ResultModel Validate(Entity entity)
        {
            return new ResultModel { Success = true, Message = "该实体类未实现验证方法,默认返回True" };
        }
        #endregion
    }
}
