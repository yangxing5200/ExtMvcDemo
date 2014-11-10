/*************************************************************************************
     * CLR 版本：       4.0.30319.34014
     * 类 名 称：       Filter
     * 机器名称：       JASON
     * 命名空间：       X.Framework.Common.Collections
     * 文 件 名：       Filter
     * 创建时间：       2014/11/6 10:30:57
	 * 计算机名：		Yang
     * 作    者：       Jason.Yang(yangxing1002@gmail.com)
     * 说    明： 
     * 修改时间：
     * 修 改 人：
**************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using YX.Framework.Common.Utility;

namespace YX.Framework.Common.Collections
{
    [Serializable]
    [DataContract]
    [KnownType(typeof(int[]))]
    [KnownType(typeof(string[]))]
    [KnownType(typeof(double[]))]
    [KnownType(typeof(float[]))]
    [KnownType(typeof(short[]))]
    [KnownType(typeof(DateTime[]))]
    [KnownType(typeof(bool[]))]
    [KnownType(typeof(byte[]))]
    [KnownType(typeof(MatchMode))]
    public class Filter
    {
        #region Constructors
        private Filter() { }

        private Filter(string name, JComparison comparison)
            : this(name, comparison, null)
        {
        }

        private Filter(string name, JComparison comparison, object value)
        {
            Name = name;
            Comparison = comparison;
            Value = value;
            Option = null;
        }

        private Filter(string name, JComparison comparison, object value, object option)
        {
            Name = name;
            Comparison = comparison;
            Value = value;
            Option = option;
        }
        #endregion

        #region proporties
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public JComparison Comparison { get; set; }

        [DataMember]
        public object Value { get; set; }

        [DataMember]
        public object Option { get; set; }

        [DataMember]
        public Filter LFilter { get; set; }

        [DataMember]
        public Filter RFilter { get; set; }

        #endregion

        public virtual void MappingName(Dictionary<string, string> fieldMapping)
        {
            if (fieldMapping.ContainsKey(Name))
            {
                Name = fieldMapping[Name];
            }

            if (LFilter != null)
            {
                LFilter.MappingName(fieldMapping);
            }

            if (RFilter != null)
            {
                RFilter.MappingName(fieldMapping);
            }
        }

        #region Static Methods

        public static Filter Equal(string name, object value)
        {
            return new Filter(name, JComparison.Equal, value);
        }

        public static Filter NotEqual(string name, object value)
        {
            return new Filter(name, JComparison.NotEqual, value);
        }

        public static Filter GreaterThan(string name, object value)
        {
            return new Filter(name, JComparison.GreaterThan, value);
        }

        public static Filter LessThan(string name, object value)
        {
            return new Filter(name, JComparison.LessThan, value);
        }

        public static Filter GreaterEqualThan(string name, object value)
        {
            return new Filter(name, JComparison.GreaterEqualThan, value);
        }

        public static Filter LessEqualThan(string name, object value)
        {
            return new Filter(name, JComparison.LessEqualThan, value);
        }

        public static Filter In(string name, ICollection value)
        {
            return new Filter(name, JComparison.In, value);
        }

        public static Filter Like(string name, string value)
        {
            return Like(name, value, MatchMode.Anywhere);
        }

        public static Filter Like(string name, string value, MatchMode matchMode)
        {
            return new Filter(name, JComparison.Like, value, matchMode);
        }

        public static Filter EqualProperty(string name, string property)
        {
            return new Filter(name, JComparison.EqualProperty, property);
        }

        public static Filter NotEqualProperty(string name, string property)
        {
            return new Filter(name, JComparison.NotEqualProperty, property);
        }

        public static Filter GreaterThanProperty(string name, string property)
        {
            return new Filter(name, JComparison.GreaterThanProperty, property);
        }

        public static Filter LessThanProperty(string name, string property)
        {
            return new Filter(name, JComparison.LessThanProperty, property);
        }

        public static Filter GreaterEqualThanProperty(string name, string property)
        {
            return new Filter(name, JComparison.GreaterEqualThanProperty, property);
        }

        public static Filter LessEqualThanProperty(string name, string property)
        {
            return new Filter(name, JComparison.LessEqualThanProperty, property);
        }

        public static Filter And(Filter lFilter, Filter rFilter)
        {
            return new Filter { Comparison = JComparison.And, LFilter = lFilter, RFilter = rFilter, Name = string.Empty, Option = null, Value = null };
        }

        public static Filter And(Filter lFilter, Filter rFilter, params Filter[] filters)
        {
            return filters.Aggregate(And(lFilter, rFilter), And);
        }

        public static Filter Or(Filter lFilter, Filter rFilter)
        {
            return new Filter { Comparison = JComparison.OR, LFilter = lFilter, RFilter = rFilter, Name = string.Empty, Option = null, Value = null };
        }

        public static Filter Or(Filter lFilter, Filter rFilter, params Filter[] filters)
        {
            return filters.Aggregate(Or(lFilter, rFilter), Or);
        }

        public static Filter Not(Filter lFilter)
        {
            return new Filter { Comparison = JComparison.Not, LFilter = lFilter, RFilter = null, Name = string.Empty, Option = null, Value = null };
        }

        public static Filter IsNull(string name)
        {
            return new Filter(name, JComparison.IsNull);
        }

        public static Filter Sql(string sql)
        {
            return new Filter("", JComparison.Sql, sql);
        }
        #endregion

        #region To Sql
        public string ToSqlString()
        {
            if (Comparison == JComparison.Like)
            {
                string format;
                if (Option != null)
                {
                    switch ((MatchMode)Option)
                    {
                        case MatchMode.Anywhere:
                            format = " {0} like N'%{1}%' ";
                            break;
                        case MatchMode.End:
                            format = " {0} like N'%{1}' ";
                            break;
                        case MatchMode.Exact:
                            format = " {0} = N'{1}' ";
                            break;
                        case MatchMode.Start:
                            format = " {0} like N'{1}%' ";
                            break;
                        default:
                            format = " {0} like N'%{1}%' ";
                            break;
                    }
                }
                else
                {
                    format = " {0} like N'%{1}%' ";
                }
                //modify by vigoss 08/28 2013 
                return string.Format(format, Name, Value == null ? null : Value.ToString().Replace("'", "''").Replace("\"", "''"));
            }
            if (Comparison == JComparison.And)
            {
                return string.Format("({0} AND {1})", LFilter.ToSqlString(), RFilter.ToSqlString());
            }
            if (Comparison == JComparison.OR)
            {
                return string.Format("({0} OR {1})", LFilter.ToSqlString(), RFilter.ToSqlString());
            }
            if (Comparison == JComparison.Not)
            {
                return string.Format("(NOT {0})", LFilter.ToSqlString());
            }
            if (Comparison == JComparison.Sql)
            {
                return string.Format("({0})", Value);
            }

            return ToHqlString();
        }
        #endregion

        #region ToHql

        public string ToHqlString()
        {
            object conditionValue = Value;
            string hqlParameter = null;

            if (Value is DateTime)
            {
                // The following code is just for Oracle
                string dateFormat = "yyyy/MM/dd HH:mm:ss";
                string oracleDateFormat = "YYYY/MM/DD HH24:MI:SS";
                DateTime conditionDate = (DateTime)Value;

                hqlParameter = string.Format("to_date('{0}', '{1}')", conditionDate.ToString(dateFormat), oracleDateFormat);
                conditionValue = hqlParameter;
            }
            else
            {
                hqlParameter = ToHqlParameter(conditionValue);
            }

            string hqlCondition;
            switch (Comparison)
            {
                case JComparison.Equal:
                    hqlCondition = string.Format(" {0} = {1} ", Name, hqlParameter);
                    break;
                case JComparison.NotEqual:
                    hqlCondition = string.Format(" {0} != {1} ", Name, hqlParameter);
                    break;
                case JComparison.GreaterThan:
                    hqlCondition = string.Format(" {0} > {1} ", Name, hqlParameter);
                    break;
                case JComparison.GreaterEqualThan:
                    hqlCondition = string.Format(" {0} >= {1} ", Name, hqlParameter);
                    break;
                case JComparison.LessThan:
                    hqlCondition = string.Format(" {0} < {1} ", Name, hqlParameter);
                    break;
                case JComparison.LessEqualThan:
                    hqlCondition = string.Format(" {0} <= {1} ", Name, hqlParameter);
                    break;
                case JComparison.In:
                    hqlCondition = string.Format(" {0} in {1} ", Name, ToHqlParameter((ICollection)conditionValue));
                    break;
                case JComparison.Like:
                    string format;
                    if (Option != null)
                    {
                        switch ((MatchMode)Option)
                        {
                            case MatchMode.Anywhere:
                                format = " {0} like '%{1}%' ";
                                break;
                            case MatchMode.End:
                                format = " {0} like '%{1}' ";
                                break;
                            case MatchMode.Exact:
                                format = " {0} = '{1}' ";
                                break;
                            case MatchMode.Start:
                                format = " {0} like '{1}%' ";
                                break;
                            default:
                                format = " {0} like '%{1}%' ";
                                break;
                        }
                    }
                    else
                    {
                        format = " {0} like '%{1}%' ";
                    }

                    hqlCondition = string.Format(format, Name, hqlParameter);
                    break;
                case JComparison.EqualProperty:
                    hqlCondition = string.Format(" {0} = {1} ", Name, conditionValue);
                    break;
                case JComparison.NotEqualProperty:
                    hqlCondition = string.Format(" {0} != {1} ", Name, conditionValue);
                    break;
                case JComparison.GreaterThanProperty:
                    hqlCondition = string.Format(" {0} > {1} ", Name, conditionValue);
                    break;
                case JComparison.GreaterEqualThanProperty:
                    hqlCondition = string.Format(" {0} >= {1} ", Name, conditionValue);
                    break;
                case JComparison.LessThanProperty:
                    hqlCondition = string.Format(" {0} < {1} ", Name, conditionValue);
                    break;
                case JComparison.LessEqualThanProperty:
                    hqlCondition = string.Format(" {0} <= {1} ", Name, conditionValue);
                    break;
                case JComparison.And:
                    hqlCondition = string.Format("({0} AND {1})", LFilter.ToHqlString(), RFilter.ToHqlString());
                    break;
                case JComparison.OR:
                    hqlCondition = string.Format("({0} OR {1})", LFilter.ToHqlString(), RFilter.ToHqlString());
                    break;
                case JComparison.Not:
                    hqlCondition = string.Format("(NOT {0})", LFilter.ToHqlString());
                    break;
                case JComparison.IsNull:
                    hqlCondition = string.Format("({0} is null)", Name);
                    break;
                case JComparison.Sql:
                    hqlCondition = string.Format("({0})", conditionValue);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return hqlCondition;
        }

        private static string ToHqlParameter(object value)
        {
            if (!(value is string))
            {
                return value.ToString();
            }

            var s = value as string;

            return string.Format("'{0}'", value.ToString().Replace("'", "''"));
        }

        private string ToHqlParameter(ICollection value)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("(");

            foreach (var item in value)
            {
                sb.Append(" " + ToHqlParameter(item) + " ,");
            }
            sb.Append(") ");

            return sb.ToString().Replace(",)", ")");
        }

        #endregion
    }

    public static partial class Util
    {
        public static void MappingName(this IEnumerable<Filter> filters, Dictionary<string, string> fieldMapping)
        {
            if (filters == null)
            {
                return;
            }
            filters.Each(p => p.MappingName(fieldMapping));
        }

        public static string ToSqlString(this IEnumerable<Filter> filters)
        {
            if (filters == null || !filters.Any())
            {
                return " 1=1 ";
            }
            return filters.ToString(p => p.ToSqlString(), " AND ");
        }

        public static string ToHqlString(this IEnumerable<Filter> filters)
        {
            if (filters == null || !filters.Any())
            {
                return " 1=1 ";
            }
            return filters.ToString(p => p.ToHqlString(), " AND ");
        }

        public static string ToString(ICollection value)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in value)
            {
                sb.Append(item + ",");
            }
            return sb.ToString().TrimEnd(',');
        }
    }
}