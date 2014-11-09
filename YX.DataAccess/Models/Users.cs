#region 作者和版权
/*************************************************************************************
      * CLR 版本:       4.0.30319.18063
      * 类 名 称:       Users
      * 机器名称:       PC201405241137
      * 命名空间:       YX.DataAccess.Models
      * 文 件 名:       Users
      * 创建时间:       2014/11/9 10:37:33
      * 作    者:       杨廷兴 Jason
      * 版    权:       Users说明：本代码版权归杨廷兴所有，使用时必须带上常伟华网站地址 All Rights Reserved (C) 2014 - 2020
      * 签    名:        to be or not to be, that's a question. !
      * 网    站:       http://user.qzone.qq.com/418505093
      * 邮    箱:       yangxing1002@gmail.com  
      * 唯一标识：      66eab834-c52a-478f-a7ba-770a06d82764  
      *
      * 登录用户:       Administrator
      * 所 属 域:       PC-Jason
  
      * 创建年份:       2014
      * 修改时间:
      * 修 改 人:
      * 
      ************************************************************************************/
#endregion
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YX.DataAccess.Models
{
    public class Users : BaseModel
    {
       public string UserName { get; set; }
       public string FullName { get; set; }
       public int Age { get; set; }
       public int MajorId { get; set; }
       public DateTime? Birthday { get; set; }
       public string Desc { get; set; }

        [ForeignKey("MajorId")]
       public virtual Major Major { get; set; }
    }
}
