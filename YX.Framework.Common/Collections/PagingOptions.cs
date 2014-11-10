/*************************************************************************************
     * CLR 版本：       4.0.30319.34014
     * 类 名 称：       PagingOptions
     * 机器名称：       JASON
     * 命名空间：       X.Framework.Common.Collections
     * 文 件 名：       PagingOptions
     * 创建时间：       2014/11/6 16:47:20
	 * 计算机名：		Yang
     * 作    者：       Jason.Yang(yangxing1002@gmail.com)
     * 说    明： 
     * 修改时间：
     * 修 改 人：
**************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace YX.Framework.Common.Collections
{
    /// <summary>
    /// Paging option frontend passes in to get a paged collection.
    /// </summary>
    [Serializable]
    [DataContract]
    public class PagingOptions
    {
        /// <summary>
        /// 1-based page number
        /// </summary>
        [DataMember]
        public int PageNumber;

        /// <summary>
        /// Size of the page
        /// </summary>
        [DataMember]
        public int PageSize;

        /// <summary>
        /// Whether the query will return the total number of records
        /// </summary>
        [DataMember]
        public bool FetchTotalRecordCount;

        ///// <summary>
        ///// Column name for sorting, assuming that the value passed from frontend will match the column on the domain object.
        ///// </summary>
        //[DataMember]
        //public string SortBy;

        ///// <summary>
        ///// Whether the collection should be sort descending
        ///// </summary>
        //[DataMember]
        //public bool SortDescending;

        [DataMember]
        public List<SortBy> SortBy = new List<SortBy>();

        /// <summary>
        /// A default PagingOption with FetchTotalRecordCount=false, PageNumber=1, and PageSize=12
        /// </summary>
        public static PagingOptions Default
        {
            get
            {
                return new PagingOptions
                {
                    FetchTotalRecordCount = false,
                    PageNumber = 1,
                    PageSize = 12
                };
            }
        }


        /// <summary>
        /// Another default PagingOption with FetchTotalRecordCount=true, PageNumber=1, and PageSize=12
        /// </summary>
        public static PagingOptions FetchRecordCount
        {
            get
            {
                return new PagingOptions
                {
                    FetchTotalRecordCount = true,
                    PageNumber = 1,
                    PageSize = 12
                };
            }
        }

        /// <summary>
        /// Another default PagingOption with FetchTotalRecordCount=true, PageNumber=0.
        /// Only a select count(*) sql will be issued.
        /// </summary>
        public static PagingOptions FetchRecordCountOnly
        {
            get
            {
                return new PagingOptions
                {
                    FetchTotalRecordCount = true,
                    PageNumber = 0
                };
            }
        }

        /// <summary>
        /// Another default PagingOption with FetchTotalRecordCount=true, PageNumber=0.
        /// Only a select count(*) sql will be issued.
        /// </summary>
        public static PagingOptions AllRecord
        {
            get
            {
                return new PagingOptions
                {
                    FetchTotalRecordCount = true,
                    PageSize = 0
                };
            }
        }
    }
}
