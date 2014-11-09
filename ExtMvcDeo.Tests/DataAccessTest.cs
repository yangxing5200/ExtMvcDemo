#region 作者和版权
/*************************************************************************************
      * CLR 版本:       4.0.30319.18063
      * 类 名 称:       DataAccessTest
      * 机器名称:       PC201405241137
      * 命名空间:       ExtMvcDeo.Tests
      * 文 件 名:       DataAccessTest
      * 创建时间:       2014/11/9 16:51:34
      * 作    者:       杨廷兴 Jason
      * 版    权:       DataAccessTest说明：本代码版权归杨廷兴所有，使用时必须带上常伟华网站地址 All Rights Reserved (C) 2014 - 2020
      * 签    名:        to be or not to be, that's a question. !
      * 网    站:       http://user.qzone.qq.com/418505093
      * 邮    箱:       yangxing1002@gmail.com  
      * 唯一标识：      a29093ae-3adb-4e4b-8495-8c08844d7e05  
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YX.DataAccess;
using YX.DataAccess.Models;

namespace ExtMvcDeo.Tests
{
  public  class DataAccessTest
    {
      [TestMethod]
      public void TestSave()
      {
          var user = new Users {UserName = "Jason", CreateDate = DateTime.Now, Creator = 1, Age = 24};

          var db=new ExtMVCDB();
          db.Users.Add(user);
          db.SaveChanges();

      }
    }
}
