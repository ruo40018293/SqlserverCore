using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Core
{
    public class Mapper
    {
        /// <summary>
        /// 对象字段映射方法，可以把两个对象中类型和字段名相同的字段进行映射
        ///
        /// </summary>
        /// <typeparam name="TSource">源对象</typeparam>
        /// <typeparam name="TTarget">要赋值的目标对象类型</typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static (TTarget target, bool hasError) Map<TSource, TTarget>(TSource source)
        {
            //获取源对象的类型
            var sourceType = typeof(TSource);
            //获取源对象的所有属性
            var sourceProperites = sourceType.GetProperties();
            //获取目标对象的类型
            var targetType = typeof(TTarget);
            //获取目标对象所有属性
            var targetProperties = targetType.GetProperties();
            //实例化目标类型对象
            var res = Activator.CreateInstance<TTarget>();
            bool hasError = false;
            foreach (var targetPro in targetProperties)
            {
                foreach (var sourcePro in sourceProperites)
                {
                    if (targetPro.PropertyType == sourcePro.PropertyType && targetPro.Name == sourcePro.Name)
                    {
                        try
                        {
                            targetPro.SetValue(res, sourcePro.GetValue(source));
                        }
                        catch (Exception)
                        {
                            hasError = true;
                        }
                    }
                }
            }
            return (res, hasError);
        }

        /// <summary>
        /// 映射两个具有相同字段的集合
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static (IEnumerable<TTarget> targets, bool hasError) Map<TSource, TTarget>(IEnumerable<TSource> source)
        {
            var res = new List<(TTarget, bool)>();
            foreach (var item in source)
            {
                var tp = Map<TSource, TTarget>(item);
                res.Add(tp);
            }
            var hasError = res.Count(x => x.Item2) > 0;
            return (res.Select(x => x.Item1), hasError);
        }

        /// <summary>
        /// 对比两个相同的实体有没有值不同的字段，有的话进行更换
        /// </summary>
        /// <typeparam name="TEntity">对象类型</typeparam>
        /// <param name="fromObj">旧实体</param>
        /// <param name="toObj">新实体</param>
        /// <param name="updateDefaultValue">是否检测默认值</param>
        /// <returns></returns>
        public static (TEntity result, bool hasError) UpdateEntity<TEntity>(TEntity fromObj, TEntity toObj, bool updateDefaultValue = false)
        {
            var properites = typeof(TEntity).GetProperties();
            bool hasError = false;
            try
            {
                foreach (var p in properites)
                {
                    var fromValue = p.GetValue(fromObj);
                    var toValue = p.GetValue(toObj);
                    if (fromValue != toValue)
                    {
                        if (updateDefaultValue)
                        {
                            var deVal = DefaultForType(p.PropertyType);
                            if (fromValue != deVal)
                            {
                                p.SetValue(toObj, fromValue);
                            }
                        }
                        else
                        {
                            p.SetValue(toObj, fromValue);
                        }
                    }
                }
            }
            catch (Exception)
            {

                hasError = false;
            }
            return (toObj, hasError);
        }

        public static object DefaultForType(Type targetType)
        {
            return targetType.IsValueType ? Activator.CreateInstance(targetType) : null;
        }
    }
}
