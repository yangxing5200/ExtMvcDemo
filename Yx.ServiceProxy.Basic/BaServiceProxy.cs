using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yx.ServiceProxy.Basic.BasicService;

namespace Yx.ServiceProxy.Basic
{
    public class BaServiceProxy:IBasicService
    {
        public string GetData(int value)
        {
            throw new NotImplementedException();
        }

 
        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            throw new NotImplementedException();
        }

    }
}
