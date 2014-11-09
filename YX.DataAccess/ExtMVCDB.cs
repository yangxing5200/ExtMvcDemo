#region 作者和版权
/*************************************************************************************
      * CLR 版本:       4.0.30319.18063
      * 类 名 称:       ExtMVCDB
      * 机器名称:       PC201405241137
      * 命名空间:       YX.DataAccess
      * 文 件 名:       ExtMVCDB
      * 创建时间:       2014/11/9 13:28:47
      * 作    者:       杨廷兴 Jason
      * 版    权:       ExtMVCDB说明：本代码版权归杨廷兴所有，使用时必须带上常伟华网站地址 All Rights Reserved (C) 2014 - 2020
      * 签    名:        to be or not to be, that's a question. !
      * 网    站:       http://user.qzone.qq.com/418505093
      * 邮    箱:       yangxing1002@gmail.com  
      * 唯一标识：      c7805e06-22bc-47ef-9a2d-04fb6d2de06c  
      *
      * 登录用户:       Administrator
      * 所 属 域:       PC201405241137
  
      * 创建年份:       2014
      * 修改时间:
      * 修 改 人:
      * 
      ************************************************************************************/
#endregion
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YX.DataAccess.Models;

namespace YX.DataAccess
{
   public class ExtMVCDB:DbContext
    {
       public ExtMVCDB()
           : base("name=DefaultConnection")
       {
           this.Database.CreateIfNotExists();
       }

       public IDbSet<Users> Users { get; set; }
       public IDbSet<Major> Majors { get; set; }
    }
}
