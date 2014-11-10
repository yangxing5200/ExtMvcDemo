/*************************************************************************************
     * CLR 版本：       4.0.30319.34014
     * 类 名 称：       SqlDataReaderToList
     * 机器名称：       JASON
     * 命名空间：       X.Framework.Common.Collections
     * 文 件 名：       SqlDataReaderToList
     * 创建时间：       2014/11/6 20:50:44
	 * 计算机名：		Yang
     * 作    者：       Jason.Yang(yangxing1002@gmail.com)
     * 说    明： 
     * 修改时间：
     * 修 改 人：
**************************************************************************************/

namespace YX.Framework.Common.Collections
{
   
    public class DataReaderSerializable
    {
        ///// <summary>
        ///// 判断SqlDataReader是否存在某列
        ///// </summary>
        ///// <param name="dr">SqlDataReader</param>
        ///// <param name="columnName">列名</param>
        ///// <returns></returns>
        //private static bool readerExists(SqlDataReader dr, string columnName)
        //{

        //    dr.GetSchemaTable().DefaultView.RowFilter = "ColumnName= '" + columnName + "'";

        //    return (dr.GetSchemaTable().DefaultView.Count > 0);

        //}

        ///// <summary>
        ///// 利用反射和泛型将SqlDataReader转换成List模型
        ///// </summary>
        ///// <param name="reader"></param>
        ///// <returns></returns>
        //public static IList<T> SqlDataReaderToList<T>(SqlDataReader reader, string[] aliases) where T : new()
        //{
        //    if (reader.HasRows)
        //        {
        //            IList<T> list = new List<T>();
        //            while (reader.Read())
        //            {
        //                T t = new T();

        //                PropertyInfo[] propertys = t.GetType().GetProperties();

        //                foreach (PropertyInfo pi in propertys)
        //                {
        //                    string tempName = pi.Name;

        //                    if (readerExists(reader, tempName))
        //                    {
        //                        if (!pi.CanWrite)
        //                        {
        //                            continue;
        //                        }
                                
        //                        var value = reader[tempName];

        //                        if (value != DBNull.Value)
        //                        {
        //                            pi.SetValue(t, value, null);
        //                        }

        //                    }
        //                }

        //                list.Add(t);

        //            }
        //            return list;
        //        }
            
        //    return null;
        //}
    }
}
