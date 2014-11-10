/*************************************************************************************
     * CLR 版本：       4.0.30319.34014
     * 类 名 称：       LINQUtil
     * 机器名称：       JASON
     * 命名空间：       X.Framework.Common.Utility
     * 文 件 名：       LINQUtil
     * 创建时间：       2014/11/6 10:54:44
	 * 计算机名：		Yang
     * 作    者：       Jason.Yang(yangxing1002@gmail.com)
     * 说    明： 
     * 修改时间：
     * 修 改 人：
**************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;

namespace YX.Framework.Common.Utility
{
    public static class LINQUtil
    {

        #region DefaultValue
        /// <summary>
        /// Define the default value if the object equals null.
        /// </summary>
        /// <typeparam name="TSource">The type of source.</typeparam>
        /// <typeparam name="TResult">The type of the value returned by the function.</typeparam>
        /// <param name="source">an object to get value from</param>
        /// <param name="func">a function to get value from the source</param>
        /// <param name="defaultValue">the default value to return if the sourec equals null</param>
        /// <returns>the value of the source</returns>
        public static TResult DefaultValue<TSource, TResult>(this TSource source, Func<TSource, TResult> func, TResult defaultValue)
        {
            return source.DefaultValue(func, default(TSource), defaultValue);
        }

        /// <summary>
        /// Define the default value if the object equals default object.
        /// </summary>
        /// <typeparam name="TSource">The type of source.</typeparam>
        /// <typeparam name="TResult">The type of the value returned by the function.</typeparam>
        /// <param name="source">an object to get value from</param>
        /// <param name="func">a function to get value from the source</param>
        /// <param name="defaultSource">the default source to campare</param>
        /// <param name="defaultValue">the default value to return if the sourec equals null</param>
        /// <returns>the value of the source</returns>
        public static TResult DefaultValue<TSource, TResult>(this TSource source, Func<TSource, TResult> func, TSource defaultSource, TResult defaultValue)
        {
            return object.Equals(source, defaultSource) ? defaultValue : func(source);
        }
        #endregion

        #region DefaultFunc

        /// <summary>
        /// call the special function for the object but call default function if the object equals null
        /// </summary>
        /// <typeparam name="TSource">The type of source.</typeparam>
        /// <typeparam name="TResult">The type of the value returned by the function.</typeparam>
        /// <param name="source">an object to call function</param>
        /// <param name="func">a function to call to the object</param>
        /// <param name="defaultFunc">a function for the null</param>
        /// <returns>the rusult of the function</returns>
        public static TResult DefaultFunc<TSource, TResult>(this TSource source, Func<TSource, TResult> func, Func<TResult> defaultFunc)
        {
            return object.Equals(source, default(TSource)) ? defaultFunc() : func(source);
        }

        /// <summary>
        /// call the special function for the object but call default function if the object default object.
        /// </summary>
        /// <typeparam name="TSource">The type of source.</typeparam>
        /// <typeparam name="TResult">The type of the value returned by the function.</typeparam>
        /// <param name="source">an object to call function</param>
        /// <param name="func">a function to call to the object</param>
        /// <param name="defaultFunc">a function for the null</param>
        /// <param name="defaultSource">the default source to campare</param>
        /// <returns>the rusult of the function</returns>
        public static TResult DefaultFunc<TSource, TResult>(this TSource source, Func<TSource, TResult> func, Func<TResult> defaultFunc, TSource defaultSource)
        {
            return object.Equals(source, defaultSource) ? defaultFunc() : func(source);
        }

        /// <summary>
        /// do the special function for the object but do default action if the object equals null
        /// </summary>
        /// <typeparam name="TSource">The type of source.</typeparam>
        /// <param name="source">an object to do action</param>
        /// <param name="action">an action for the object</param>
        /// <param name="defaultAction">an action for the null</param>
        public static void DefaultFunc<TSource>(this TSource source, Action<TSource> action, Action defaultAction)
        {
            DefaultFunc(source, action, defaultAction, default(TSource));
        }

        /// <summary>
        /// do the special function for the object but do default action if the object equals default object.
        /// </summary>
        /// <typeparam name="TSource">The type of source.</typeparam>
        /// <param name="source">an object to do action</param>
        /// <param name="action">an action for the object</param>
        /// <param name="defaultAction">an action for the null</param>
        /// <param name="defaultSource">the default source to campare</param>
        public static void DefaultFunc<TSource>(this TSource source, Action<TSource> action, Action defaultAction, TSource defaultSource)
        {
            if (object.Equals(source, defaultSource))
                defaultAction();
            else
                action(source);
        }
        #endregion

        #region IEnumerable.ToString
        /// <summary>
        /// Join the every element in the collection 
        /// </summary>
        /// <param name="source">the collection containt the elements to join</param>
        /// <param name="separator">A string as separator</param>
        /// <returns>A string object consisting of the strings in value joined by separator.</returns>
        public static string ToString(this IEnumerable<string> source, string separator)
        {
            return string.Join(separator, source.ToArray());
        }

        /// <summary>
        /// Join the every element in the collection 
        /// </summary>
        /// <typeparam name="TSource">the type of the elements in the collection</typeparam>
        /// <param name="source">the collection containt the elements to join</param>
        /// <param name="func">A function to translate every element to string</param>
        /// <param name="separator">A string as separator</param>
        /// <returns>A string object consisting of the strings in value joined by separator.</returns>
        public static string ToString<TSource>(this IEnumerable<TSource> source, Func<TSource, string> func, string separator)
        {
            return string.Join(separator, source.Select(func).ToArray());
        }

        /// <summary>
        /// Join the every element in the collection 
        /// </summary>
        /// <typeparam name="TSource">the type of the elements in the collection</typeparam>
        /// <param name="source">the collection containt the elements to join</param>
        /// <param name="func">A function to translate every element to string</param>
        /// <param name="separator">A string as separator</param>
        /// <param name="startIndex">The first element in collection to use.</param>
        /// <param name="count">The number of elements in collection to use.</param>
        /// <returns>A string object consisting of the strings in value joined by separator.</returns>
        public static string ToString<TSource>(this IEnumerable<TSource> source, Func<TSource, string> func, string separator, int startIndex, int count)
        {
            return string.Join(separator, source.Select(func).ToArray(), startIndex, count);
        }

        /// <summary>
        /// Join the every element in the collection 
        /// </summary>
        /// <typeparam name="TSource">the type of the elements in the collection</typeparam>
        /// <param name="source">the collection containt the elements to join</param>
        /// <param name="func">A function to translate every element to string</param>
        /// <param name="separator">A string as separator</param>
        /// <returns>A string object consisting of the strings in value joined by separator.</returns>
        public static string ToString<TSource>(this IEnumerable<TSource> source, Func<TSource, int, string> func, string separator)
        {
            return string.Join(separator, source.Select(func).ToArray());
        }

        /// <summary>
        /// Join the every element in the collection 
        /// </summary>
        /// <typeparam name="TSource">the type of the elements in the collection</typeparam>
        /// <param name="source">the collection containt the elements to join</param>
        /// <param name="func">A function to translate every element to string</param>
        /// <param name="separator">A string as separator</param>
        /// <param name="startIndex">The first element in collection to use.</param>
        /// <param name="count">The number of elements in collection to use.</param>
        /// <returns>A string object consisting of the strings in value joined by separator.</returns>
        public static string ToString<TSource>(this IEnumerable<TSource> source, Func<TSource, int, string> func, string separator, int startIndex, int count)
        {
            return string.Join(separator, source.Select(func).ToArray(), startIndex, count);
        }
        #endregion

        #region DoAction
        /// <summary>
        /// Do some action for each element in the collection
        /// </summary>
        /// <typeparam name="TSource">the type of the elements in the collection</typeparam>
        /// <param name="source">the collection containt the elements to do the action</param>
        /// <param name="action">the action to do with each element</param>
        public static void Each<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
        {
            foreach (var item in source)
            {
                action(item);
            }
        }


        /// <summary>
        /// Do some action for each element in the collection
        /// </summary>
        /// <typeparam name="TSource">the type of the elements in the collection</typeparam>
        /// <param name="source">the collection containt the elements to do the action</param>
        /// <param name="action">the action to do with each element and the index of element</param>
        public static void Each<TSource>(this IEnumerable<TSource> source, Action<TSource, int> action)
        {
            int i = 0;
            foreach (var item in source)
            {
                action(item, i);
                i++;
            }
        }
        #endregion

        #region Parents and Chinldren
        public static IEnumerable<TSource> AllChildren<TSource>(this TSource source, Func<TSource, IEnumerable<TSource>> func)
        {
            var chidren = func(source);

            return chidren.Count() == 0 ? Enumerable.Empty<TSource>() : chidren.Concat(chidren.SelectMany(p => p.AllChildren(func)));
        }

        public static IEnumerable<TSource> AllParents<TSource>(this TSource source, Func<TSource, TSource> func) where TSource : class
        {
            var parent = func(source);


            if (parent != null)
            {
                var parents = parent.AllParents(func);
                return parents.Concat(Enumerable.Repeat(source, 1));
            }
            else
            {
                return Enumerable.Repeat(source, 1);
            }
        }

        #endregion

        #region Copy Data
        public static T CopyData<T>(this T source)
            where T : class, new()
        {
            T result = new T();
            CopyData(source, result);
            return result;
        }


        public static TResult CopyData<TSource, TResult>(this TSource source)
            where TResult : class, new()
            where TSource : class
        {
            TResult result = new TResult();
            CopyData(source, result);
            return result;
        }


        public static void CopyData<TSource, TResult>(this TSource source, TResult result)
            where TSource : class
            where TResult : class
        {
            if (source == null || result == null)
            {
                return;
            }

            Type sType = typeof(TSource);
            Type rType = typeof(TResult);

            var sProperties = sType.GetProperties().Where(p => p.CanRead);
            var rProperties = rType.GetProperties().Where(p => p.CanWrite);

            var joinProperties = sProperties.Join(rProperties,
                             p => new { p.Name, p.PropertyType },
                             p => new { p.Name, p.PropertyType },
                             (p, q) => new { SProperty = p, RProperty = q });

            joinProperties.Each(p => p.RProperty.SetValue(result, p.SProperty.GetValue(source, null), null));
        }

        #endregion
    }
}
