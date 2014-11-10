/*************************************************************************************
     * CLR 版本：       4.0.30319.34014
     * 类 名 称：       TypeExtension
     * 机器名称：       JASON
     * 命名空间：       X.Framework.Common.Utility
     * 文 件 名：       TypeExtension
     * 创建时间：       2014/11/6 16:51:54
	 * 计算机名：		Yang
     * 作    者：       Jason.Yang(yangxing1002@gmail.com)
     * 说    明： 
     * 修改时间：
     * 修 改 人：
**************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web;

namespace YX.Framework.Common.Utility
{
    public static class StringExtensions
    {
        // Methods
        public static TValue As<TValue>(this string value)
        {
            return value.As<TValue>(default(TValue));
        }

        public static TValue As<TValue>(this string value, TValue defaultValue)
        {
            try
            {
                TypeConverter converter = TypeDescriptor.GetConverter(typeof(TValue));
                if (converter.CanConvertFrom(typeof(string)))
                {
                    return (TValue)converter.ConvertFrom(value);
                }
                converter = TypeDescriptor.GetConverter(typeof(string));
                if (converter.CanConvertTo(typeof(TValue)))
                {
                    return (TValue)converter.ConvertTo(value, typeof(TValue));
                }
            }
            catch (Exception)
            {
            }
            return defaultValue;
        }

        public static bool AsBool(this string value)
        {
            return value.As<bool>(false);
        }

        public static bool AsBool(this string value, bool defaultValue)
        {
            return value.As<bool>(defaultValue);
        }

        public static DateTime AsDateTime(this string value)
        {
            return value.As<DateTime>();
        }

        public static DateTime AsDateTime(this string value, DateTime defaultValue)
        {
            return value.As<DateTime>(defaultValue);
        }

        public static decimal AsDecimal(this string value)
        {
            return value.As<decimal>();
        }

        public static decimal AsDecimal(this string value, decimal defaultValue)
        {
            return value.As<decimal>(defaultValue);
        }

        public static double AsDouble(this string value)
        {
            return value.As<double>();
        }

        public static double AsDouble(this string value, double defaultValue)
        {
            return value.As<double>(defaultValue);
        }

        public static float AsFloat(this string value)
        {
            return value.As<float>();
        }

        public static float AsFloat(this string value, float defaultValue)
        {
            return value.As<float>(defaultValue);
        }

        public static int AsInt(this string value)
        {
            return value.As<int>();
        }

        public static int AsInt(this string value, int defaultValue)
        {
            return value.As<int>(defaultValue);
        }

        public static bool Is<TValue>(this string value)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(TValue));
            return (((converter != null) && converter.CanConvertFrom(typeof(string))) && converter.IsValid(value));
        }

        public static bool IsBool(this string value)
        {
            return value.Is<bool>();
        }

        public static bool IsDateTime(this string value)
        {
            return value.Is<DateTime>();
        }
        public static bool IsDouble(this string value)
        {
            return value.Is<double>();
        }
        public static bool IsDecimal(this string value)
        {
            return value.Is<decimal>();
        }

        public static bool IsEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static bool IsFloat(this string value)
        {
            return value.Is<float>();
        }

        public static bool IsInt(this string value)
        {
            return value.Is<int>();
        }
        /// <summary>
        ///  spilt string and validate the spilted string array length
        /// </summary>
        /// <param name="val">target string</param>
        /// <param name="separator">assigned separator</param>
        /// <param name="len">excepted array length</param>
        /// <param name="validate">the spilted string array</param>
        /// <returns></returns>
        public static string[] SpiltAndValidateArrayLength(this string val, char separator, int len, out bool validate)
        {
            var ret = val.Split(separator);
            validate = ret.Length == len;
            return ret;
        }

        /// <summary>
        /// 判断Decimal类型小数点后面的长度
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static int GetDigtialLength(this decimal val)
        {
            var arr = val.ToString().Split('.');
            return arr.Length == 2 ? arr[1].Length : 0;
        }
        /// <summary>
        /// Converts string from one encoding to another.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="srcEncoding"></param>
        /// <param name="dstEncoding"></param>
        /// <returns></returns>
        public static string Convert(this string value, Encoding srcEncoding, Encoding dstEncoding)
        {
            var bytes = srcEncoding.GetBytes(value);
            var newBytes = Encoding.Convert(srcEncoding, dstEncoding, bytes);
            return dstEncoding.GetString(newBytes);
        }
    }
    public static class DoubleExtensions
    {
        public static int Ceiling(this double val)
        {
            return (int)Math.Ceiling(val);
        }
        public static int Floor(this double val)
        {
            return (int)Math.Floor(val);
        }
        public static int Days(this TimeSpan span)
        {
            return span.TotalDays >= 0 ? Ceiling(span.TotalDays) : Floor(span.TotalDays);
        }
        public static int Step(this TimeSpan span)
        {
            return Math.Abs(Days(span));
        }
    }

    public static class TypeExtension
    {
        public static Decimal Round(this decimal val, int decimals)
        {
            return System.Decimal.Round(val, decimals);
        }
        public static T AsType<T>(this object obj)
        {
            if (obj == null) return default(T);

            if (obj is T)
                return (T)obj;

            var sourceType = obj.GetType();
            var targetType = typeof(T);

            if (targetType.IsEnum)
                return (T)Enum.Parse(targetType, obj.ToString(), true);

            if (sourceType.GetInterface(typeof(IConvertible).FullName) != null &&
                targetType.GetInterface(typeof(IConvertible).FullName) != null)
                return (T)Convert.ChangeType(obj, targetType);

            var converter = TypeDescriptor.GetConverter(obj);
            if (converter != null && converter.CanConvertTo(targetType))
                return (T)converter.ConvertTo(obj, targetType);

            converter = TypeDescriptor.GetConverter(targetType);
            if (converter != null && converter.CanConvertFrom(sourceType))
                return (T)converter.ConvertFrom(obj);

            throw new ApplicationException("convert error.");
        }

        public static void ForEach<T>(this IEnumerable<T> enumberable, Action<T> action)
        {
            if (enumberable == null) throw new ArgumentNullException("enumberable");
            foreach (T item in enumberable)
                action(item);
        }
        public static void ForEach(this IEnumerable enumberable, Action<object> action)
        {
            if (enumberable == null) throw new ArgumentNullException("enumberable");
            foreach (object item in enumberable)
                action(item);
        }
        /// <summary>
        /// Get's the name of the assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>The assembly's name.</returns>
        public static string GetAssemblyName(this Assembly assembly)
        {
            return assembly.FullName.Remove(assembly.FullName.IndexOf(','));
        }
        /// <summary>
        /// Gets all the attributes of a particular type.
        /// </summary>
        /// <typeparam name="T">The type of attributes to get.</typeparam>
        /// <param name="member">The member to inspect for attributes.</param>
        /// <param name="inherit">Whether or not to search for inherited attributes.</param>
        /// <returns>The list of attributes found.</returns>
        public static IEnumerable<T> GetAttributes<T>(this MemberInfo member, bool inherit)
        {
            return Attribute.GetCustomAttributes(member, inherit).OfType<T>();
        }
        /// <summary>
        /// Converts an expression into a <see cref="MemberInfo"/>.
        /// </summary>
        /// <param name="expression">The expression to convert.</param>
        /// <returns>The member info.</returns>
        public static MemberInfo GetMemberInfo(this Expression expression)
        {
            var lambda = (LambdaExpression)expression;

            MemberExpression memberExpression;
            if (lambda.Body is UnaryExpression)
            {
                var unaryExpression = (UnaryExpression)lambda.Body;
                memberExpression = (MemberExpression)unaryExpression.Operand;
            }
            else
            {
                memberExpression = (MemberExpression)lambda.Body;
            }

            return memberExpression.Member;
        }
        /// <summary>
        /// 获取实体属性名称集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetPropertyNames(this Type entityType)
        {
            foreach (var property in entityType.GetProperties())
                yield return property.Name;
        }
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
               (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> keys = new HashSet<TKey>();
            foreach (TSource element in source)
                if (keys.Add(keySelector(element)))
                    yield return element;
        }
        public static bool HasAttribute<TAttribute>(this MemberInfo member, bool inherit) where TAttribute : Attribute
        {
            return member.GetAttributes<TAttribute>(inherit).Count() > 0;
        }
    }

    public static class EnumExtesion
    {

        /// <summary>
        /// Converts string value to enum type
        /// </summary>
        /// <typeparam name="TEnum">The System.Type of the enumeration.</typeparam>
        /// <param name="value"> A string containing the name or value to convert.</param>
        /// <returns>An object of type enumType whose value is represented by value.</returns>
        public static TEnum AsEnum<TEnum>(this string value) where TEnum : struct
        {
            return (TEnum)Enum.Parse(typeof(TEnum), value);
        }
        /// <summary>
        /// Converts string value to enum type
        /// </summary>
        /// <typeparam name="TEnum">The System.Type of the enumeration.</typeparam>
        /// <param name="value"> A string containing the name or value to convert.</param>
        /// <param name="ignoreCase"> If true, ignore case; otherwise, regard case.</param>
        /// <returns>An object of type enumType whose value is represented by value.</returns>
        public static TEnum AsEnum<TEnum>(this string value, bool ignoreCase) where TEnum : struct
        {
            return (TEnum)Enum.Parse(typeof(TEnum), value, ignoreCase);
        }
        /// <summary>
        /// Gets enum description
        /// If DescriptionAttribute is null then returns the enum string value
        /// </summary>
        /// <typeparam name="TEnum">The System.Type of the enumeration.</typeparam>
        /// <param name="enum"></param>
        /// <returns>if DescriptionAttribute is null then returns the enum string value</returns>
        public static string GetEnumDescription<TEnum>(this TEnum @enum) where TEnum : struct
        {
            var enumName = @enum.ToString();
            var fieldInfo = @enum.GetType().GetField(enumName);
            if (fieldInfo == null) return enumName;
            var attributes = fieldInfo.GetAttributes<DescriptionAttribute>(false).ToArray();
            if (attributes.Length < 1) return enumName;
            return attributes[0].Description;
        }
    }
    public static class HttpContextExtension
    {
        /// <summary>
        /// download file from asp.net pipline.
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <param name="fileName">specified filename with absoulte path</param>
        public static void DownloadFile(this HttpContext context, string fileName)
        {
            DownloadFile(context, fileName, Encoding.UTF8);
        }
        /// <summary>
        /// download file from asp.net pipline.
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <param name="fileName">specified filename with absoulte path</param>
        /// <param name="encoding">encoding type</param>
        public static void DownloadFile(this HttpContext context, string fileName, Encoding encoding)
        {
            if (!File.Exists(fileName))
                throw new FileNotFoundException(fileName);
            FileInfo file = new FileInfo(fileName);
            context.Response.Clear();
            context.Response.Buffer = true;
            context.Response.ContentEncoding = encoding;
            context.Response.ContentType = "application/x-msdownload";
            context.Response.Charset = encoding.HeaderName;
            context.Response.AddHeader("Content-Disposition", "attachment;filename=" + file.Name);
            context.Response.AddHeader("Content-Length", file.Length.ToString());
            context.Response.WriteFile(file.FullName);
            context.Response.Flush();
            context.Response.End();
        }
        /// <summary>
        /// download file from asp.net pipline.
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <param name="content">the file content</param>
        /// <param name="downloadFileName">download file name</param>
        public static void DownloadFile(this HttpContext context, string content, string downloadFileName)
        {
            DownloadFile(context, content, Encoding.UTF8, downloadFileName);
        }
        /// <summary>
        /// download file from asp.net pipline.
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <param name="content">the file content</param>
        /// <param name="encoding">encoding type,default encoding is utf-8 </param>
        /// <param name="downloadFileName">download file name</param>
        public static void DownloadFile(this HttpContext context, string content, Encoding encoding, string downloadFileName)
        {
            byte[] bytes = encoding.GetBytes(content);
            DownloadFile(context, bytes, encoding, downloadFileName);
        }
        /// <summary>
        /// download file from asp.net pipline.
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <param name="bytes">the file bytes array</param>
        /// <param name="encoding">encoding type,default encoding is utf-8</param>
        /// <param name="downloadFileName">download file name</param>
        public static void DownloadFile(this HttpContext context, byte[] bytes, Encoding encoding, string downloadFileName)
        {
            context.Response.Clear();
            context.Response.Buffer = true;
            context.Response.ContentEncoding = encoding;
            context.Response.ContentType = "application/x-msdownload";
            context.Response.Charset = encoding.HeaderName;
            context.Response.AddHeader("Content-Disposition", "attachment;filename=" + downloadFileName);
            context.Response.AddHeader("Content-Length", bytes.Length.ToString());
            context.Response.BinaryWrite(bytes);
            context.Response.Flush();
            context.Response.End();
        }
    }
}
