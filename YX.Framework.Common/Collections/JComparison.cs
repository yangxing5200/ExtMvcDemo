/*************************************************************************************
     * CLR 版本：       4.0.30319.34014
     * 类 名 称：       JComparison
     * 机器名称：       JASON
     * 命名空间：       X.Framework.Common.Collections
     * 文 件 名：       JComparison
     * 创建时间：       2014/11/6 10:32:24
	 * 计算机名：		Yang
     * 作    者：       Jason.Yang(yangxing1002@gmail.com)
     * 说    明： 
     * 修改时间：
     * 修 改 人：
**************************************************************************************/

using System;
using System.Runtime.Serialization;

namespace YX.Framework.Common.Collections
{
    [Serializable]
    [DataContract]
    public enum JComparison
    {
        [EnumMember]
        Equal = 10,
        [EnumMember]
        NotEqual = 11,

        [EnumMember]
        GreaterThan = 12,
        [EnumMember]
        GreaterEqualThan = 13,

        [EnumMember]
        LessThan = 14,
        [EnumMember]
        LessEqualThan = 15,

        [EnumMember]
        In = 20,
        [EnumMember]
        Like = 21,

        [EnumMember]
        EqualProperty = 30,
        [EnumMember]
        NotEqualProperty = 31,

        [EnumMember]
        GreaterThanProperty = 32,
        [EnumMember]
        GreaterEqualThanProperty = 33,

        [EnumMember]
        LessThanProperty = 34,
        [EnumMember]
        LessEqualThanProperty = 35,

        [EnumMember]
        And = 90,
        [EnumMember]
        OR = 91,
        [EnumMember]
        Not = 92,

        [EnumMember]
        IsNull = 100,

        [EnumMember]
        Sql = 200,

    }
}