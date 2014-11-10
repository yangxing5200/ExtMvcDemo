/*************************************************************************************
     * CLR 版本：       4.0.30319.34014
     * 类 名 称：       MatchMode
     * 机器名称：       JASON
     * 命名空间：       X.Framework.Common.Collections
     * 文 件 名：       MatchMode
     * 创建时间：       2014/11/6 10:46:49
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
    public enum MatchMode
    {
        [EnumMember]
        Anywhere,
        [EnumMember]
        End,
        [EnumMember]
        Exact,
        [EnumMember]
        Start
    }
}
