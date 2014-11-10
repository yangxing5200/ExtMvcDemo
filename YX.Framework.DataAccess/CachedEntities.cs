/*************************************************************************************
     * CLR 版本：       4.0.30319.34014
     * 类 名 称：       CachedEntities
     * 机器名称：       JASON
     * 命名空间：       X.Framework.Common.Utility
     * 文 件 名：       CachedEntities
     * 创建时间：       2014/11/6 16:50:18
	 * 计算机名：		Yang
     * 作    者：       Jason.Yang(yangxing1002@gmail.com)
     * 说    明： 
     * 修改时间：
     * 修 改 人：
**************************************************************************************/

using System;
using System.Collections;
using System.Linq;
using YX.Framework.Common.Utility;

namespace YX.Framework.DataAccess
{
    internal sealed class CachedEntities
    {
        CachedEntities() { }
        public static CachedEntities Instance
        {
            get
            {
                return Nested.CachedEntities;
            }
        }
        Hashtable dict = new Hashtable();
        public string[] this[Type entityType]
        {
            get
            {
                lock (dict.SyncRoot)
                {
                    if (dict.ContainsKey(entityType.FullName))
                    {
                        return dict[entityType.FullName] as string[];
                    }
                    else
                    {
                        var propertyNames = entityType.GetPropertyNames().ToArray();
                        dict[entityType.FullName] = propertyNames;
                        return propertyNames;
                    }
                }
            }
        }
        #region Nested Class
        /// <summary>
        /// Fully lazy initialization of the static member: triggered by the 
        /// first reference. Implementation has performance benefits, and is 
        /// fully lazy.
        /// </summary>
        /// <remarks>
        /// http://www.yoda.arachsys.com/csharp/singleton.html
        /// </remarks>
        private class Nested
        {
            /// <summary>
            /// read only instance of JobEngine
            /// </summary>
            internal static CachedEntities CachedEntities = new CachedEntities();

            /// <summary>
            /// Initializes static members of the Nested class
            /// </summary>
            static Nested()
            {
                // Explicit static constructor to tell C# compiler to not mark type as beforefieldinit
            }
        }
        #endregion
    }
}
