/*************************************************************************************
     * CLR 版本：       4.0.30319.34014
     * 类 名 称：       ManagerBase
     * 机器名称：       JASON
     * 命名空间：       X.Framework.DataAccess
     * 文 件 名：       ManagerBase
     * 创建时间：       2014/11/6 15:06:50
	 * 计算机名：		Yang
     * 作    者：       Jason.Yang(yangxing1002@gmail.com)
     * 说    明： 
     * 修改时间：
     * 修 改 人：
**************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using YX.Framework.Common.Collections;
using YX.Framework.Common.Utility;

namespace YX.Framework.DataAccess
{
    public abstract class ManagerBase<T, TKey> : IDisposable where T : BusinessBase<TKey>
    {
        /// <summary>
        /// Query the specified sql query or views.
        /// </summary>
        /// <typeparam name="TResult">type of the JList element,belongs to data transfer object.</typeparam>
        /// <param name="sql">sql query or views</param>
        /// <param name="pagingOption">paging settings</param>
        /// <param name="filters">query condition</param>
        /// <returns>the collection result</returns>
        public JList<TResult> GetByNativeSql<TResult>(string sql, PagingOptions pagingOption, List<Filter> filters)
            where TResult : class
        {
            var aliases = CachedEntities.Instance[typeof(TResult)];
            var strSql = string.Format("select {0} ", aliases.ToString(","));
            string fromAndWhere = string.Format(" from ({0}) t where 1=1 ", sql);
            if (filters != null && filters.Count > 0)
            {
                fromAndWhere += " and " + filters.ToSqlString();
            }
            string sqlOrder = " order by ";
            if (pagingOption.SortBy != null && pagingOption.SortBy.Count > 0)
            {
                sqlOrder = sqlOrder + pagingOption.SortBy.ToString(sort => sort.ToSqlString(), ",");
            }
            else
            {
                sqlOrder = sqlOrder + " RowNum asc";
            }
            return GetByNativeSql<TResult>(strSql, fromAndWhere, sqlOrder, pagingOption, aliases);
        }
        /// <summary>
        /// 根据Native SQL返回数据集(结果集不是Group By)
        /// </summary>
        /// <param name="sqlSelect">类似于“Select StdItemID, itemcode”字符串</param>
        /// <param name="sqlFromAndWhere">类似于“FROM BA_StdItem a Where StdItemID>100”字符串</param>
        /// <param name="sqlDefaultOrderby">缺省的OrderBy，类似于“Order by itemcode”字符串</param>
        /// <param name="pagingOptions">pagingOptions，分页信息（包括排序信息）</param>
        /// <param name="aliases">返回结果的别名的字符数组</param>
        public JList<TResult> GetByNativeSql<TResult>(string sqlSelect, string sqlFromAndWhere, string sqlDefaultOrderby, PagingOptions pagingOptions, string[] aliases)
        {
            return GetByNativeSql<TResult>(sqlSelect, sqlFromAndWhere, false, sqlDefaultOrderby, pagingOptions, aliases);
        }

        /// <summary>
        /// 根据Native SQL返回数据集
        /// </summary>
        /// <param name="sqlSelect">类似于“Select StdItemID, itemcode”字符串</param>
        /// <param name="sqlFromAndWhere">类似于“FROM BA_StdItem a Where StdItemID>100”字符串</param>
        /// <param name="isGroupby">结果集是否为Group By</param>
        /// <param name="sqlDefaultOrderby">缺省的OrderBy，类似于“Order by itemcode”字符串</param>
        /// <param name="pagingOptions">pagingOptions，分页信息（包括排序信息）</param>
        /// <param name="aliases">返回结果的别名的字符数组</param>
        public JList<TResult> GetByNativeSql<TResult>(string sqlSelect, string sqlFromAndWhere, bool isGroupby, string sqlDefaultOrderby, PagingOptions pagingOptions, string[] aliases)
        {
            if (string.IsNullOrEmpty(sqlSelect) || string.IsNullOrEmpty(sqlFromAndWhere) || aliases.Length == 0)
            {

                throw new System.Exception();
            }

            if (pagingOptions.FetchTotalRecordCount)
            {
                if (string.IsNullOrEmpty(sqlDefaultOrderby))
                {

                    throw new System.Exception();
                }
                return GetAllResult<TResult>(sqlSelect, sqlFromAndWhere, isGroupby, sqlDefaultOrderby, pagingOptions, aliases);
            }

            if (pagingOptions.PageSize > 0)
            {
                if (string.IsNullOrEmpty(sqlDefaultOrderby))
                {

                    throw new System.Exception();
                }
                return GetPagingResult<TResult>(sqlSelect, sqlFromAndWhere, sqlDefaultOrderby, pagingOptions, aliases);
            }

            JList<TResult> easList = new JList<TResult>();
            if (pagingOptions.SortBy != null && pagingOptions.SortBy.Count > 0)
            {
                sqlDefaultOrderby = ConvertSortByToHqlConditon(pagingOptions.SortBy);
            }
            easList.Items = GetByNativeSql<TResult>(sqlSelect + " " + sqlFromAndWhere + " " + sqlDefaultOrderby, aliases);
            return easList;
        }
        /// <summary>
        /// 根据Native SQL返回数据集(不分页)
        /// </summary>
        /// <param name="sql">查询的sql,类似于“Select ... from ... where...group by... order by ... ”</param>
        /// <param name="aliases">返回结果的别名的字符数组</param>
        public List<T> GetByNativeSql<T>(string sql, string[] aliases)
        {
            SqlDataReader sqldataread = SqlHelper.ExecuteReader(SqlHelper.GetConnection(), CommandType.Text, sql);
            var list = new List<T>();
            while (sqldataread.Read())
            {
                list.Add(AliasToBeanResult.Transform<T>(sqldataread, aliases));
            }
            return list;
        }
        ///// <summary>
        ///// do sql query and return result if list<T>!
        ///// </summary>
        //public List<T> GetByNativeSql<T>(ISQLQuery sqlQuery, string[] aliases)
        //{
        //    IList rows = sqlQuery.List();

        //    List<T> result = new List<T>();
        //    foreach (object[] o in rows)
        //        result.Add((U)AliasToBeanResult.Transform<T>(o, aliases));

        //    SqlDataReader sqldataread = SqlHelper.ExecuteReader(SqlHelper.GetConnection(), CommandType.Text, "");
        //    return DataReaderSerializable.SqlDataReaderToList<T>(sqldataread);


        //    return result;
        //    //return TransferBeans<T>(aliases, rows);
        //}

        ///// <summary>
        ///// Gets sql query!
        ///// </summary>
        //public ISQLQuery GetSQLQuery(string sql)
        //{
        //    return Session.GetISession().CreateSQLQuery(sql);
        //}
        /// <summary>
        /// 返回TotalCount，和分页结果集
        /// </summary>
        private JList<T> GetAllResult<T>(string sqlSelect, string sqlFromAndWhere, bool isGroupby, string sqlDefaultOrderby, PagingOptions pagingOptions, string[] aliases)
        {
            //1			WITH SearchResults AS (SELECT (SELECT COUNT(*) 
            //2			FROM BA_StdItem --动态
            //3			) AS "TotalCount", ROW_NUMBER() OVER (ORDER BY 
            //4			StdItemID  --动态
            //5			) AS "RowNum",
            //6			a.* 
            //7			FROM BA_StdItem a --动态
            //11		) select * FROM SearchResults
            //12		WHERE "RowNum" BETWEEN 
            //13		1  --动态
            //14		AND 
            //15		10 --动态
            //16		order by "RowNum"

            StringBuilder tempStr = new StringBuilder();
            tempStr.Append("WITH SearchResults AS (SELECT (SELECT COUNT(*) ");  //1

            if (isGroupby)
            {
                tempStr.Append(" from ( select count(1) as totalcount " + sqlFromAndWhere + ") mm ");  //2
            }
            else
            {
                tempStr.Append(sqlFromAndWhere);                                    //2
            }

            tempStr.Append(" ) AS \"TotalCount\", ROW_NUMBER() OVER ( ");			//3

            //Process sort by
            if (pagingOptions != null && pagingOptions.SortBy != null && pagingOptions.SortBy.Count > 0)	//4
            {
                tempStr.Append(ConvertSortByToHqlConditon(pagingOptions.SortBy));
            }
            else
            {
                tempStr.Append(sqlDefaultOrderby);
            }
            tempStr.Append(" ) AS \"RowNum\",");									//5
            int pos = sqlSelect.IndexOf("select", StringComparison.OrdinalIgnoreCase) + "select".Length;
            tempStr.Append(sqlSelect.Substring(pos));							//6
            tempStr.Append(" ");
            tempStr.Append(sqlFromAndWhere);									//7

            tempStr.Append(") select * FROM SearchResults ");								//11
            if (pagingOptions.PageSize > 0)
            {
                tempStr.Append(" WHERE \"RowNum\" BETWEEN ");						//12
                //1 based page index used to calculate the first record to return
                int pageIndex = pagingOptions.PageNumber <= 0 ? 1 : pagingOptions.PageNumber - 1;
                int firstResult = pageIndex * pagingOptions.PageSize;

                tempStr.Append(firstResult + 1);									//13
                tempStr.Append(" AND ");										//14
                tempStr.Append((firstResult + pagingOptions.PageSize).ToString());	//15
            }
            tempStr.Append(" order by \"RowNum\" ");								//16

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetConnection(), CommandType.Text, tempStr.ToString());


            JList<T> easList = new JList<T>();
            if (pagingOptions.FetchTotalRecordCount && ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow tobjs = (ds.Tables[0].Rows[0]);
                easList.TotalRecordCount = Convert.ToInt32(tobjs[0]); //设置TotalRecordCount


                foreach (object[] o in ds.Tables[0].Rows)
                {
                    var oneobj = new object[o.Length - 2]; //去除TotalCount、RowNum
                    for (int i = 2; i < o.Length; i++)
                    {
                        oneobj[i - 2] = o[i];
                    }
                    easList.Items.Add(AliasToBeanResult.Transform<T>(oneobj, aliases)); //转换成需要的对象类型（U）
                }
            }

            return easList;
        }

        /// <summary>
        /// 返回分页结果集，不包含TotalCount信息
        /// </summary>
        private JList<TResult> GetPagingResult<TResult>(string sqlSelect, string sqlFromAndWhere, string sqlDefaultOrderby, PagingOptions pagingOptions, string[] aliases)
        {
            //1			WITH SearchResults AS (SELECT 
            //3			ROW_NUMBER() OVER (ORDER BY 
            //4			StdItemID  --动态
            //5			) AS "RowNum",
            //6			a.* 
            //7			FROM BA_StdItem a --动态
            //11		) select * FROM SearchResults
            //12		WHERE "RowNum" BETWEEN 
            //13		1  --动态
            //14		AND 
            //15		10 --动态
            //16		order by "RowNum"

            StringBuilder tempStr = new StringBuilder();
            tempStr.Append("WITH SearchResults AS (SELECT ");  //1

            tempStr.Append(" ROW_NUMBER() OVER ( ");			//3

            //Process sort by
            if (pagingOptions != null && pagingOptions.SortBy != null && pagingOptions.SortBy.Count > 0)	//4
            {
                tempStr.Append(ConvertSortByToHqlConditon(pagingOptions.SortBy));
            }
            else
            {
                tempStr.Append(sqlDefaultOrderby);
            }
            tempStr.Append(" ) AS \"RowNum\",");									//5
            int pos = sqlSelect.IndexOf("select", StringComparison.OrdinalIgnoreCase) + "select".Length;
            tempStr.Append(sqlSelect.Substring(pos));							//6
            tempStr.Append(" ");
            tempStr.Append(sqlFromAndWhere);									//7

            tempStr.Append(") select * FROM SearchResults ");								//11

            tempStr.Append(" WHERE \"RowNum\" BETWEEN ");						//12
            //1 based page index used to calculate the first record to return
            int pageIndex = pagingOptions.PageNumber <= 0 ? 1 : pagingOptions.PageNumber - 1;
            int firstResult = pageIndex * pagingOptions.PageSize;

            tempStr.Append(firstResult + 1);									//13
            tempStr.Append(" AND ");										//14
            tempStr.Append((firstResult + pagingOptions.PageSize).ToString());	//15

            tempStr.Append(" order by \"RowNum\" ");								//16

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetConnection(), CommandType.Text, tempStr.ToString());


            var easList = new JList<TResult>();

            foreach (object[] o in ds.Tables[0].Rows)
            {
                var oneobj = new object[o.Length - 1]; //去除RowNum
                for (int i = 1; i < o.Length; i++)
                {
                    oneobj[i - 1] = o[i];
                }
                easList.Items.Add(AliasToBeanResult.Transform<TResult>(oneobj, aliases));  //转换成需要的对象类型（U）
            }

            return easList;
        }

        public string ConvertSortByToHqlConditon(List<SortBy> sortBys)
        {
            if (sortBys == null || sortBys.Count == 0)
                return null;

            StringBuilder sb = new StringBuilder();
            sb.Append(" order by ");
            for (int i = 0; i < sortBys.Count; i++)
            {
                sb.Append(sortBys[i].ToHqlString());
                if (i != sortBys.Count - 1)
                {
                    sb.Append(", ");
                }
            }

            return sb.ToString();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
