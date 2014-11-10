/*************************************************************************************
     * CLR 版本：       4.0.30319.34014
     * 类 名 称：       JList
     * 机器名称：       JASON
     * 命名空间：       X.Framework.Common.Collections
     * 文 件 名：       JList
     * 创建时间：       2014/11/6 10:27:21
	 * 计算机名：		Yang
     * 作    者：       Jason.Yang(yangxing1002@gmail.com)
     * 说    明： 
     * 修改时间：
     * 修 改 人：
**************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace YX.Framework.Common.Collections
{
  
        /// <summary>
        /// Implements a customerized List that contains the total number of records. This should be primarily used to return a paged collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        [DataContract]
        public class JList<T>
        {
            /// <summary>
            /// Get or set 
            /// </summary>
            [DataMember]
            public int TotalRecordCount
            {
                get;
                set;
            }

            /// <summary>
            /// Get or set 
            /// </summary>
            [DataMember]
            public Decimal Total
            {
                get;
                set;
            }
            [DataMember]
            public Dictionary<string, decimal> Statistics { get; set; }

            /// <summary>
            /// Wrap the underlining list.
            /// </summary>
            [DataMember]
            public List<T> Items
            {
                get;
                set;
            }

            [DataMember]
            public int Key
            {
                get;
                set;
            }

            /// <summary>
            /// Default constructor.
            /// </summary>
            public JList()
            {
                Statistics = new Dictionary<string, decimal>();
                Items = new List<T>();
            }

            public JList(List<T> list)
            {
                Statistics = new Dictionary<string, decimal>();
                Items = list;
            }

            /// <summary>
            /// Convert to new JList of another type
            /// </summary>
            /// <typeparam name="TResult"></typeparam>
            /// <param name="func"></param>
            /// <returns></returns>
            public JList<TResult> ToJList<TResult>(Func<T, TResult> func)
            {
                JList<TResult> result = new JList<TResult>(Items.Select(func).ToList());
                result.TotalRecordCount = TotalRecordCount;
                return result;
            }
            public decimal GetStatisticsValue(string key)
            {
                return Statistics.ContainsKey(key) ? Statistics[key] : 0m;
            }
        }
    }
