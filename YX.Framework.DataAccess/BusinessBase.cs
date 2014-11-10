/*************************************************************************************
     * CLR 版本：       4.0.30319.34014
     * 类 名 称：       BusinessBase
     * 机器名称：       JASON
     * 命名空间：       X.Framework.DataAccess
     * 文 件 名：       BusinessBase
     * 创建时间：       2014/11/7 18:12:34
	 * 计算机名：		Yang
     * 作    者：       Jason.Yang(yangxing1002@gmail.com)
     * 说    明： 
     * 修改时间：
     * 修 改 人：
**************************************************************************************/

namespace YX.Framework.DataAccess
{
    public abstract class BusinessBase<T>
    {
        #region Declarations

        private T _id = default(T);

        #endregion

        #region Methods

        public abstract override int GetHashCode();
        public override bool Equals(object obj)
        {
            return (obj != null)                                                    // 1) Object is not null.
                && (obj.GetType() == this.GetType())                                // 2) Object is of same Type.
                && (MatchingIds((BusinessBase<T>)obj) || MatchingHashCodes(obj));   // 3) Ids or Hashcodes match.
        }
        private bool MatchingIds(BusinessBase<T> obj)
        {
            return (this.Id != null && !this.Id.Equals(default(T)))                 // 1) this.Id is not null/default.
                && (obj.Id != null && !obj.Id.Equals(default(T)))                   // 1.5) obj.Id is not null/default.
                && (this.Id.Equals(obj.Id));                                        // 2) Ids match.
        }
        private bool MatchingHashCodes(object obj)
        {
            return this.GetHashCode().Equals(obj.GetHashCode());                    // 1) Hashcodes match.
        }

        #endregion

        #region Properties

        public virtual T Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public virtual bool NeedVersionControl
        {
            get { return true; }
        }
        #endregion
    }
}
