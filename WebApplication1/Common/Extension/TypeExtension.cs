using Logger;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Common.Extension
{
    public static class TypeExtension
    {
        #region 字符串转换

        /// <summary>
        /// 分割字符串转成数组
        /// </summary>
        /// <param name="data">字符串</param>
        /// <param name="split">分割字符</param>
        /// <returns></returns>
        public static string[] ToSplitArray(this string data, string split = ",")
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                return new string[] { };
            }
            return data.Split(new[] { split }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// 是否为空
        /// </summary>
        /// <param name="value">字符串</param>
        public static bool IsEmpty(this string value)
        {
            return string.IsNullOrWhiteSpace(value) || value.Equals("&nbsp;") || value.Equals("null");
        }

        /// <summary>
        /// 转换成字符串，为空时或者出错时返回空字符串
        /// </summary>
        /// <param name="o">源数据</param>
        public static string ObjToString(this object o)
        {
            try
            {
                return o == null || o == DBNull.Value ? string.Empty : o.ToString();
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLog("类型转换异常", ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// 转换成字符串，为空时或者出错时返回默认值
        /// </summary>
        /// <param name="o">源数据</param>
        /// <param name="defaultValue">自定义的默认值</param>
        public static string ObjToString(this object o, string defaultValue)
        {
            try
            {
                return o == null || o == DBNull.Value ? defaultValue : o.ToString();
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLog("类型转换异常", ex);
                return defaultValue;
            }
        }

        /// <summary>
        /// 转换成字符串，为空时返回null
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string ObjToStringOrNull(this object o)
        {
            try
            {
                return o == null || o == DBNull.Value ? null : o.ToString();
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLog("类型转换异常", ex);
                return null;
            }
        }

        #endregion

        #region 布尔值转换

        public static bool ObjToBool(this object o)
        {
            if (o == null)
                return false;
            var value = GetBool(o);
            if (value != null)
                return value.Value;
            bool result;
            return bool.TryParse(o.ObjToString(), out result) && result;
        }

        public static bool ObjToBool(this object o, bool defaultValue)
        {
            try
            {
                return (o == null) || (o == DBNull.Value) ? defaultValue : Convert.ToBoolean(o);
            }
            catch
            {
                return defaultValue;
            }
        }

        public static bool? ObjToBoolOrNull(this object o)
        {
            if (o == null)
                return null;
            var value = GetBool(o);
            if (value != null)
                return value.Value;
            bool result;
            var isValid = bool.TryParse(o.ObjToString(), out result);
            if (isValid)
                return result;
            return null;
        }

        /// <summary>
        ///  获取布尔值
        /// </summary>
        /// <param name="o">数据</param>
        /// <returns></returns>
        private static bool? GetBool(this object o)
        {
            switch (o.ToString().Trim().ToLower())
            {
                case "0":
                    return false;
                case "1":
                    return true;
                case "是":
                    return true;
                case "否":
                    return false;
                case "yes":
                    return true;
                case "no":
                    return false;
                case "true":
                    return true;
                case "false":
                    return false;
                default:
                    return null;
            }
        }

        #endregion

        #region 时间类型转换

        /// <summary>
        /// 把对象转换成DateTime对象，默认返回当前时间
        /// </summary>
        /// <param name="o">要转换的对象</param>
        public static DateTime ObjToDateTime(this object o)
        {
            DateTime time;
            return DateTime.TryParse(o.ObjToString(), out time) ? time : DateTime.MinValue;
        }
        /// <summary>
        /// 转换成DateTime对象，默认返回默认值
        /// </summary>
        /// <param name="o"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime ObjToDateTime(this object o, DateTime defaultValue)
        {
            try
            {
                return (o == null) || (o == DBNull.Value) ? defaultValue : Convert.ToDateTime(o);
            }
            catch
            {
                return defaultValue;
            }
        }

        public static DateTime? ObjToDateTimeOrNull(this object o)
        {
            if (o == null)
                return null;
            DateTime result;
            var isValid = DateTime.TryParse(o.ObjToString(), out result);
            if (isValid)
                return result;
            return null;
        }

        #endregion

        #region Decimal类型转换
        public static decimal ObjToDecimal(this object o)
        {
            decimal num;
            return decimal.TryParse(o.ObjToString(), out num) ? num : 0;
        }

        public static decimal ObjToDecimal(this object o, decimal defaultValue)
        {
            try
            {
                return (o == null) || (o == DBNull.Value) ? defaultValue : Convert.ToDecimal(o);
            }
            catch
            {
                return defaultValue;
            }
        }
        public static decimal? ObjToDecimalOrNull(this object o)
        {
            if (o == null)
                return null;
            decimal result;
            var isValid = decimal.TryParse(o.ObjToString(), out result);
            if (isValid)
                return result;
            return null;
        }
        #endregion

        #region Double类型转换

        public static double ObjToDouble(this object o)
        {
            double num;
            return Double.TryParse(o.ObjToString(), out num) ? num : 0;
        }

        public static double ObjToDouble(this object o, double defaultValue)
        {
            try
            {
                return (o == null) || (o == DBNull.Value) ? defaultValue : Convert.ToDouble(o);
            }
            catch
            {
                return defaultValue;
            }
        }

        public static double? ObjToDoubleOrNull(this object o)
        {
            if (o == null)
                return null;
            double result;
            var isValid = double.TryParse(o.ObjToString(), out result);
            if (isValid)
                return result;
            return null;
        }

        #endregion

        #region Int类型转换

        public static int ObjToInt(this object o)
        {
            int num;
            return int.TryParse(o.ObjToString(), out num) ? num : 0;
        }

        public static int ObjToInt(this object o, int defaultValue)
        {
            if ((o == null) || (o == DBNull.Value))
            {
                return defaultValue;
            }
            try
            {
                int returnValue = 0;
                if (o.ToString().IndexOf(".", System.StringComparison.Ordinal) > -1)
                {
                    double dValue = 0;
                    double.TryParse(o.ObjToString(), out dValue);
                    o = Math.Round(dValue, 0);
                }
                int.TryParse(o.ObjToString(), out returnValue);
                return returnValue;
            }
            catch
            {
                return defaultValue;
            }
        }

        public static int? ObjToIntOrNull(this object o)
        {
            if (o == null)
                return null;
            int result;
            var isValid = int.TryParse(o.ToString(), out result);
            if (isValid)
                return result;
            return null;
        }

        #region 把中文数字转换成Int32
        public static int ChineseToInt(this object data)
        {
            try
            {
                var val = data.ToString().Trim();
                if (val.Length > 0)
                {
                    val = val.Replace(",", "");
                    if (val.IsExistChinese())
                    {
                        //上限为20亿
                        var rgxText = "";
                        List<string> strList = new List<string>();
                        if (val.Contains("万"))
                        {
                            var text = val.Split('万');
                            if (text[0].Contains("百") || text[0].Contains("佰") || text[0].Contains("零"))
                            {
                                rgxText += "([12一二壹贰][0十拾]?亿)?([123456789一二三四五六七八九壹贰叁肆伍陆柒捌玖]?[千仟零])?([123456789一二三四五六七八九壹贰叁肆伍陆柒捌玖]?[百佰零])?";
                            }
                            else
                            {
                                for (var i = 0; i < 3; i++)
                                {
                                    strList.Add("");
                                }
                            }
                            rgxText += "([123456789一二三四五六七八九壹贰叁肆伍陆柒捌玖][十拾]?[0123456789一二三四五六七八九壹贰叁肆伍陆柒捌玖]?万)?";
                        }
                        else
                        {
                            for (var i = 0; i < 4; i++)
                            {
                                strList.Add("");
                            }
                        }
                        rgxText += "([123456789一二三四五六七八九壹贰叁肆伍陆柒捌玖]?[千仟零])?([123456789一二三四五六七八九壹贰叁肆伍陆柒捌玖]?[百佰零])?([123456789一二三四五六七八九壹贰叁肆伍陆柒捌玖]?[0十拾零])?([123456789一二三四五六七八九壹贰叁肆伍陆柒捌玖])?";
                        Regex rgx = new Regex(rgxText);

                        var groups = rgx.Match(val).Groups;
                        for (var i = 1; i < groups.Count; i++)
                        {
                            strList.Add(groups[i].Value);
                        }
                        if (strList.Count != 0)
                        {
                            List<int> NumList = new List<int>();
                            foreach (var str in strList)
                            {
                                NumList.Add(ChineseNumToNum(str));
                            }
                            var num = NumList[0] * 100000000 + NumList[1] * 10000000 + NumList[2] * 1000000 + NumList[3] * 10000 + NumList[4] * 1000 + NumList[5] * 100 + NumList[6] + NumList[7];
                            return num;
                        }
                    }
                    else
                    {
                        return Convert.ToInt32(val);
                    }
                }
                return 0;
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLog("类型转换异常", ex);
                return 0;
            }
        }

        public static bool IsExistChinese(this string text)
        {
            var result = false;
            char[] c = text.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] >= 0x4e00 && c[i] <= 0x9fbb)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        private static int ChineseNumToNum(string strNum)
        {
            try
            {
                int num;
                strNum = strNum.Replace("亿", "").Replace("千", "").Replace("仟", "").Replace("百", "").Replace("佰", "").Replace("万", "");
                if (int.TryParse(strNum, out num))
                {
                    return num;
                }
                else
                {
                    var array = strNum.ToCharArray();
                    num = 0;
                    if (array.Length <= 3 && !string.IsNullOrEmpty(strNum))
                    {
                        if (array.Length == 1)
                        {
                            num = TextToNum(strNum);
                        }
                        else if (array.Length == 2)
                        {
                            num = TextToNum(array[0].ToString()) * 10;
                        }
                        else
                        {
                            num = TextToNum(array[0].ToString()) * 10 + TextToNum(array[2].ToString());
                        }
                    }
                }
                return num;
            }
            catch
            {
                return 0;
            }
        }

        private static int TextToNum(string text)
        {
            var result = 0;
            switch (text)
            {
                case "一":
                case "壹":
                    result = 1;
                    break;
                case "二":
                case "贰":
                    result = 2;
                    break;
                case "三":
                case "叁":
                    result = 3;
                    break;
                case "四":
                case "肆":
                    result = 4;
                    break;
                case "五":
                case "伍":
                    result = 5;
                    break;
                case "六":
                case "陆":
                    result = 6;
                    break;
                case "七":
                case "柒":
                    result = 7;
                    break;
                case "八":
                case "捌":
                    result = 8;
                    break;
                case "九":
                case "玖":
                    result = 9;
                    break;
                case "十":
                case "拾":
                    result = 10;
                    break;
                default:
                    result = 0;
                    break;
            }
            return result;
        }
        #endregion

        #endregion

        #region Long类型转换

        public static long ObjToLong(this object o)
        {
            long num;
            return long.TryParse(o.ObjToString(), out num) ? num : 0;
        }

        public static long ObjToLong(this object o, int defaultValue)
        {
            if ((o == null) || (o == DBNull.Value))
            {
                return defaultValue;
            }
            try
            {
                long returnValue = 0;
                if (o.ToString().IndexOf(".", System.StringComparison.Ordinal) > -1)
                {
                    double dValue = 0;
                    double.TryParse(o.ObjToString(), out dValue);
                    o = Math.Round(dValue, 0);
                }
                long.TryParse(o.ObjToString(), out returnValue);
                return returnValue;
            }
            catch
            {
                return defaultValue;
            }
        }

        public static long? ObjToLongOrNull(this object o)
        {
            if (o == null)
                return null;
            long result;
            var isValid = long.TryParse(o.ObjToString(), out result);
            if (isValid)
                return result;
            return null;
        }

        #endregion

        #region Short类型转换

        public static short ObjToShort(this object o)
        {
            short num;
            return short.TryParse(o.ObjToString(), out num) ? num : (short)0;
        }

        public static short ObjToShort(this object o, short defaultValue)
        {
            if ((o == null) || (o == DBNull.Value))
            {
                return defaultValue;
            }
            try
            {
                short returnValue = 0;
                if (o.ToString().IndexOf(".", System.StringComparison.Ordinal) > -1)
                {
                    double dValue = 0;
                    double.TryParse(o.ObjToString(), out dValue);
                    o = Math.Round(dValue, 0);
                }
                short.TryParse(o.ObjToString(), out returnValue);
                return returnValue;
            }
            catch
            {
                return defaultValue;
            }
        }

        public static short? ObjToShortOrNull(this object o)
        {
            if (o == null)
                return null;
            short result;
            var isValid = short.TryParse(o.ObjToString(), out result);
            if (isValid)
                return result;
            return null;
        }

        #endregion

        #region Model转换
        /// <summary>
        /// 强转Model
        /// </summary>
        /// <typeparam name="T">Model类型</typeparam>
        /// <param name="oldModel">原Model</param>
        /// <returns>newModel</returns>
        public static T ToNewModel<T>(this T oldModel) where T : new()
        {
            T model = new T();
            PropertyInfo[] f = model.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (PropertyInfo pi in f)
            {
                string field = pi.Name;
                object value = pi.GetValue(oldModel, null);
                pi.SetValue(model, value, null);
            }
            return model;
        }

        /// <summary>
        /// Model简单对象转成JObject
        /// </summary>
        /// <typeparam name="T">Model类型</typeparam>
        /// <param name="Model">对象</param>
        public static JObject ToJObject<T>(this T Model) where T : new()
        {
            PropertyInfo[] f = Model.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
            JObject obj = new JObject();
            foreach (PropertyInfo pi in f)
            {
                string field = pi.Name;
                object value = pi.GetValue(Model, null);
                obj[field] = value.ObjToString();
            }
            return obj;
        }

        /// <summary>
        /// Model简单对象集合转成JArray
        /// </summary>
        /// <typeparam name="T">Model类型</typeparam>
        /// <param name="ModelList">对象集合</param>
        public static JArray ToJArray<T>(this List<T> ModelList) where T : new()
        {
            JArray array = new JArray();
            foreach (var Model in ModelList)
            {
                JObject obj = Model.ToJObject();
                array.Add(obj);
            }

            return array;
        }

        /// <summary>
        /// Model名转换
        /// </summary>
        /// <param name="ModelName">ModelName</param>
        /// <returns>ModelName</returns>
        public static string ToModelName(this string ModelName)
        {
            return ModelName.Replace("View", "");
        }

        #endregion

        #region Data转换

        public static Dictionary<string, List<Dictionary<string, object>>> DataSetToDictionary(this DataSet dataSet)
        {
            try
            {
                Dictionary<string, List<Dictionary<string, object>>> dictionary = new Dictionary<string, List<Dictionary<string, object>>>();
                if (dataSet != null)
                {
                    foreach (DataTable table in dataSet.Tables)
                    {
                        List<Dictionary<string, object>> list = table.DataTableToDictionaryList();
                        dictionary.Add(table.TableName, list);
                    }
                }
                return dictionary;
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLog("类型转换异常", ex);
                return null;
            }
        }

        public static List<Dictionary<string, object>> DataTableToDictionaryList(this DataTable dataTable)
        {
            try
            {
                List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
                if (dataTable != null)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        Dictionary<string, object> item = new Dictionary<string, object>();
                        foreach (DataColumn column in dataTable.Columns)
                        {
                            item.Add(column.ColumnName, row[column]);
                        }
                        list.Add(item);
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLog("类型转换异常", ex);
                return null;
            }
        }


        /// <summary>
        /// DataRow转化成Model
        /// </summary>
        /// <param name="dr">DataRow</param>
        /// <returns>Model</returns>
        public static T DataRowToModel<T>(this DataRow dr) where T : new()
        {
            T t = new T();
            try
            {
                if (dr != null && t != null)
                {
                    Type type = t.GetType();
                    PropertyInfo[] f = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
                    //string tableName = type.Name;
                    foreach (PropertyInfo pi in f)
                    {
                        string field = pi.Name;
                        if (dr.Table.Columns.Contains(field) && dr[field] != DBNull.Value)
                            pi.SetValue(t, dr[field], null);
                    }
                }
                return t;
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLog("类型转换异常", ex);
                return t;
            }
        }

        /// <summary>
        /// DataSet转化成Model
        /// </summary>
        /// <param name="ds">DataRow</param>
        /// <returns>Model</returns>
        public static List<T> DataReaderToModel<T>(this DataSet ds) where T : new()
        {
            List<T> list = new List<T>();
            try
            {
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        list.Add(dr.DataRowToModel<T>());
                    }
                }

            }
            catch (Exception ex)
            {
                LogHelper.ErrorLog("类型转换异常", ex);
            }
            return list;
        }


        /// <summary>
        /// DataTable转化成ModelList
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <returns>ModelList</returns>
        public static List<T> DataTableToList<T>(this DataTable dt) where T : new()
        {
            List<T> list = new List<T>();
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(dr.DataRowToModel<T>());
                }
                return list;
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLog("类型转换异常" ,ex);
                return list;
            }
        }

        #endregion
    }
}
