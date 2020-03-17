using Common.Core;
using Model.EntityCommon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Common.Data
{
    public interface IDbBase<Entity>: IDependencyTransient,IValidate<Entity> where Entity : TableCommon, new()
    {
        ///// <summary>
        ///// EF上下文
        ///// </summary>
        //IEFDbcontext DbContext { get; }

        /// <summary>
        /// 懒加载实体
        /// </summary>
        IQueryable<Entity> Queryable { get; }
        IQueryable<Entity> GetQueryable();
        /// <summary>
        /// 提交EF实体更改
        /// </summary>
        /// <returns></returns>
        ResultModel Commit();

        /// <summary>
        /// 添加单个实体,添加时请保证主键为0
        /// </summary>
        /// <param name="entity">要添加的实体</param>
        /// <param name="save">是否持久化到数据默认为true</param>
        /// <returns></returns>
        ResultModel Add(Entity entity, bool save = true);

        /// <summary>
        /// 批量添加实体
        /// </summary>
        /// <param name="entities">要添加的实体集合</param>
        /// <param name="save">是否持久化到数据库</param>
        /// <param name="skipInvalid">遇到验证失败时是否中断操作</param>
        /// <returns></returns>
        ResultModel AddList(IList<Entity> entities, bool save = true, bool skipInvalid = true);

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="id">主键id</param>
        /// <param name="save">是否持久化到数据库</param>
        /// <returns></returns>
        ResultModel Remove(long id, bool IsLogic, bool save = true);

        /// <summary>
        /// 批量删除实体
        /// </summary>
        /// <param name="condition">删除条件表达式</param>
        /// <param name="save">是否持久化到数据库</param>
        /// <returns></returns>
        ResultModel RemoveList(Expression<Func<Entity, bool>> condition, bool IsLogic, bool save = true);

        /// <summary>
        /// 修改实体
        /// </summary>
        /// <param name="entity">要修改的实体</param>
        /// <param name="save">是否持久化到数据</param>
        /// <returns></returns>
        ResultModel Edit(Entity entity, bool save = true);

        /// <summary>
        /// 添加或者修改实体，会根据是否传递了主键进行判断
        /// </summary>
        /// <param name="entity">要新增或者修改的实体</param>
        /// <param name="save">是否持久化到数据库</param>
        /// <returns></returns>
        ResultModel AddOrEdit(Entity entity, bool save = true);

        /// <summary>
        /// 执行某条SQL
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        ResultModel ExcuteSql(string sql);

        /// <summary>
        /// 更具主键获取某个实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Entity Get(long id);

        /// <summary>
        /// 根据条件表达式获取某个实体
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        Entity Get(Expression<Func<Entity, bool>> condition);

        /// <summary>
        /// 根据条件表达式获取某个实体的某个 值
        /// </summary>
        /// <typeparam name="TVal">想要的值的类型</typeparam>
        /// <param name="condition">查找某个实体的条件表达式</param>
        /// <param name="selector">筛选某个条件的筛选表达式</param>
        /// <returns></returns>
        TVal GetValue<TVal>(Expression<Func<Entity, bool>> condition, Expression<Func<Entity, TVal>> selector);

        /// <summary>
        /// 根据条件表达式获取实体集合的某个值
        /// </summary>
        /// <typeparam name="TVal">想要的值的类型</typeparam>
        /// <param name="condition">查找某个实体集合的条件表达式</param>
        /// <param name="selector">筛选某个条件的筛选表达式</param>
        /// <returns></returns>
        IList<TVal> GetValues<TVal>(Expression<Func<Entity, bool>> condition, Expression<Func<Entity, TVal>> selector);

        /// <summary>
        /// 根据表达式获取某个实体的集合
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IList<Entity> GetList(Expression<Func<Entity, bool>> condition);

        /// <summary>
        /// 直接获取某个实体的集合
        /// </summary>
        /// <returns></returns>
        IList<Entity> GetList();

        /// <summary>
        /// 更具查询条件查出某个实体的集合并根据排序条件进行排序
        /// </summary>
        /// <typeparam name="TVal">排序条件的数据类型</typeparam>
        /// <param name="condition">筛选实体的条件表达式</param>
        /// <param name="orderBy">排序的条件表达式</param>
        /// <param name="direction">正序还是逆序</param>
        /// <returns></returns>
        IList<Entity> GetList<TVal>(Expression<Func<Entity, bool>> condition, Expression<Func<Entity, TVal>> orderBy, ListSortDirection direction);

        /// <summary>
        /// 根据条件查询某个实体的集合并且根据排序条件进行排序而且带了分页
        /// </summary>
        /// <typeparam name="TVal">排序条件的数据类型</typeparam>
        /// <param name="rows">总数据行数</param>
        /// <param name="condition">筛选集合的条件表达式</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">每页显示的行数</param>
        /// <param name="orderBy">排序条件</param>
        /// <param name="direction">正序逆序</param>
        /// <returns></returns>
        IList<Entity> GetList<TVal>(out int rows, Expression<Func<Entity, bool>> condition = null, int pageIndex = 1, int pageSize = 20, Expression<Func<Entity, TVal>> orderBy = null, ListSortDirection direction = ListSortDirection.Descending);

        /// <summary>
        /// 根据分页获取实体集合
        /// </summary>
        /// <param name="pageSize">每页显示的行数</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="rows">总行数</param>
        /// <returns></returns>
        IList<Entity> GetList(int pageSize, int pageIndex, out int rows);

        /// <summary>
        /// 根据条件表达式获取某个实体的数量
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        int GetCount(Expression<Func<Entity, bool>> condition);
    }
}
