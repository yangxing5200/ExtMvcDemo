/*************************************************************************************
     * CLR 版本：       4.0.30319.34014
     * 类 名 称：       SortBy
     * 机器名称：       JASON
     * 命名空间：       X.Framework.Common.Collections
     * 文 件 名：       SortBy
     * 创建时间：       2014/11/6 16:47:49
	 * 计算机名：		Yang
     * 作    者：       Jason.Yang(yangxing1002@gmail.com)
     * 说    明： 
     * 修改时间：
     * 修 改 人：
**************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using YX.Framework.Common.Utility;

namespace YX.Framework.Common.Collections
{
    [Serializable]
    [DataContract]
    public class SortBy
    {
        public SortBy()
        {

        }

        public SortBy(string property, bool ascending)
        {
            Property = property;
            Ascending = ascending;
        }

        public SortBy(string property)
            : this(property, true)
        {
        }

        public string ToSqlString()
        {
            return " " + Property + (Ascending ? " asc " : " desc ");
        }

        public string ToHqlString()
        {
            return " " + Property + (Ascending ? " asc " : " desc ");
        }

        [DataMember]
        public string Property { get; set; }

        [DataMember]
        public bool Ascending { get; set; }

        public virtual void MappingName(Dictionary<string, string> fieldMapping)
        {
            if (fieldMapping.ContainsKey(Property))
            {
                Property = fieldMapping[Property];
            }
        }
    }

    public static partial class Util
    {
        public static void MappingName(this IEnumerable<SortBy> sorts, Dictionary<string, string> fieldMapping)
        {
            if (sorts == null)
            {
                return;
            }
            sorts.Each(p => p.MappingName(fieldMapping));
        }
    }
}