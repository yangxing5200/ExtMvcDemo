/*************************************************************************************
     * CLR 版本：       4.0.30319.34014
     * 类 名 称：       AliasToBeanResult
     * 机器名称：       JASON
     * 命名空间：       X.Framework.Common.Utility
     * 文 件 名：       AliasToBeanResult
     * 创建时间：       2014/11/7 14:15:10
	 * 计算机名：		Yang
     * 作    者：       Jason.Yang(yangxing1002@gmail.com)
     * 说    明： 
     * 修改时间：
     * 修 改 人：
**************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace YX.Framework.Common.Utility
{
    /// <summary>
    /// transform alias to bean result
    /// </summary>
    public static class AliasToBeanResult
    {
        private static readonly Type Int16Type = typeof(short);
        private static readonly Type NullableInt16Type = typeof(short?);

        private static readonly Type Int32Type = typeof(int);
        private static readonly Type NullableInt32Type = typeof(int?);

        private static readonly Type Int64Type = typeof(long);
        private static readonly Type NullableInt64Type = typeof(long?);

        private static readonly Type ByteType = typeof(byte);
        private static readonly Type NullableByteType = typeof(byte?);

        private static readonly Type FloatType = typeof(float);
        private static readonly Type NullableFloatType = typeof(float?);

        private static readonly Type DoubleType = typeof(double);
        private static readonly Type NullableDoubleType = typeof(double?);

        private static readonly Type DecimalType = typeof(decimal);
        private static readonly Type NullableDecimalType = typeof(decimal?);

        private static readonly Type DateTimeType = typeof(DateTime);
        private static readonly Type NullableDateTimeType = typeof(DateTime?);

        private static readonly Type BooleanType = typeof(bool);
        private static readonly Type NullableBooleanType = typeof(bool?);

        public static T Transform<T>(object[] rows, string[] aliases)
        {
            return Transform<T>(rows, aliases, true);
        }

        public static T Transform<T>(object[] rows, string[] aliases, bool strictMode)
        {
            if (rows == null || rows.Length == 0 || aliases == null || aliases.Length == 0)
                return default(T);
            if (strictMode && rows.Length != aliases.Length)
                throw new NotSupportedException(string.Format("rows length({0}) != aliases length({1})", rows.Length, aliases.Length));
            return AliasToBeanResultTypeCache<T>.Transform(rows, aliases);
        }

        /// <summary>
        /// 利用反射和泛型将SqlDataReader转换成List模型
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="aliases"></param>
        /// <returns></returns>
        public static T Transform<T>(SqlDataReader reader, string[] aliases) 
        {
            if (aliases.Any())
            {
                return AliasToBeanResultTypeCache<T>.SetValue<T>(reader, aliases);
            }
            return  AliasToBeanResultTypeCache<T>.SetValue<T>(reader);
        }

    

        private static class AliasToBeanResultTypeCache<T>
        {
            private static readonly object sync = new object();
            private static readonly Type beanType = typeof(T);
            private static bool? canCreateInstance = new bool?();
            private static readonly Dictionary<string, PropertyInfo> propertyInfoTable = new Dictionary<string, PropertyInfo>();

            internal static T Transform(object[] values, string[] aliases)
            {
                if (!CanCreateInstance())
                    return default(T);

                var instance = (T)Activator.CreateInstance(beanType);
                for (int i = 0; i < aliases.Length; i++)
                {
                    try
                    {
                        PropertyInfo propertyInfo = GetPropertyInfo(aliases[i]);
                        object val = ConvertVal(propertyInfo, values[i]);
                        if (val != null)
                            propertyInfo.SetValue(instance, val, null);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format("error property {0},{1},{2}", aliases[i], ex, values[i]));
                    }

                }

                return instance;
            }


            private static bool CanCreateInstance()
            {
                if (canCreateInstance.HasValue)
                    return canCreateInstance.Value;
                if (beanType.IsAbstract || beanType.IsInterface || beanType.IsArray || beanType.IsGenericTypeDefinition || beanType == typeof(void))
                    canCreateInstance = false;
                else if (beanType.GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, new Type[0], null) == null)
                    canCreateInstance = false;
                else
                    canCreateInstance = true;

                return canCreateInstance.Value;
            }

            private static PropertyInfo GetPropertyInfo(string propertyName)
            {
                lock (sync)
                {
                    if (!propertyInfoTable.ContainsKey(propertyName))
                    {
                        PropertyInfo info = beanType.GetProperty(propertyName);
                        propertyInfoTable.Add(propertyName, info);
                    }
                    return propertyInfoTable[propertyName];
                }
            }
            /// <summary>
            /// 判断SqlDataReader是否存在某列
            /// </summary>
            /// <param name="dr">SqlDataReader</param>
            /// <param name="columnName">列名</param>
            /// <returns></returns>
            internal static bool ReaderExists(SqlDataReader dr, string columnName)
            {
                var schemaTable = dr.GetSchemaTable();
                if (schemaTable != null)
                    schemaTable.DefaultView.RowFilter = "ColumnName= '" + columnName + "'";
                var dataTable = dr.GetSchemaTable();
                return dataTable != null && (dataTable.DefaultView.Count > 0);
            }



            internal static T SetValue<T>(SqlDataReader reader, IEnumerable<string> aliases) 
            {
                var instance = (T)Activator.CreateInstance(beanType);
                foreach (string aliase in aliases)
                {
                    try
                    {
                        PropertyInfo propertyInfo = GetPropertyInfo(aliase);
                        object val = ConvertVal(propertyInfo, reader[aliase]);
                        if (val != null)
                            propertyInfo.SetValue(instance, val, null);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format("error property {0},{1},{2}", aliase, ex, reader[aliase]));
                    }
                }
                return instance;
            }
            internal static T SetValue<T>(SqlDataReader reader) 
            {
                var t = (T)Activator.CreateInstance(typeof(T));
                #region aliases is null

                PropertyInfo[] propertys = t.GetType().GetProperties();

                foreach (PropertyInfo pi in propertys)
                {
                    string tempName = pi.Name;

                    if (AliasToBeanResultTypeCache<T>.ReaderExists(reader, tempName))
                    {
                        if (!pi.CanWrite)
                        {
                            continue;
                        }

                        var value = reader[tempName];

                        if (value != DBNull.Value)
                        {
                            pi.SetValue(t, value, null);
                        }
                    }
                }

                #endregion

                return t;
            }
            private static object ConvertVal(PropertyInfo propertyInfo, object data)
            {
                if (data == null)
                    return null;
                if (!propertyInfo.PropertyType.IsValueType)
                    return data;
                Type valueType = data.GetType();
                if (valueType == propertyInfo.PropertyType)
                    return data;

                if (Int16Type == propertyInfo.PropertyType || NullableInt16Type == propertyInfo.PropertyType)
                    return Convert.ToInt16(data);
                if (Int32Type == propertyInfo.PropertyType || NullableInt32Type == propertyInfo.PropertyType)
                    return Convert.ToInt32(data);
                if (Int64Type == propertyInfo.PropertyType || NullableInt64Type == propertyInfo.PropertyType)
                    return Convert.ToInt64(data);
                if (ByteType == propertyInfo.PropertyType || NullableByteType == propertyInfo.PropertyType)
                    return Convert.ToByte(data);
                if (FloatType == propertyInfo.PropertyType || NullableFloatType == propertyInfo.PropertyType)
                    return Convert.ToSingle(data);
                if (DoubleType == propertyInfo.PropertyType || NullableDoubleType == propertyInfo.PropertyType)
                    return Convert.ToDouble(data);
                if (DecimalType == propertyInfo.PropertyType || NullableDecimalType == propertyInfo.PropertyType)
                    return Convert.ToDecimal(data);
                if (DateTimeType == propertyInfo.PropertyType || NullableDateTimeType == propertyInfo.PropertyType)
                    return Convert.ToDateTime(data);
                if (BooleanType == propertyInfo.PropertyType || NullableBooleanType == propertyInfo.PropertyType)
                    return Convert.ToBoolean(data);

                throw new NotImplementedException();
            }
        }
    }
}
