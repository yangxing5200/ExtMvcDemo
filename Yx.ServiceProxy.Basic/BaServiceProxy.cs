using System;
using System.Threading.Tasks;

using Yx.ServiceProxy.Basic.ServiceReference1;


namespace Yx.ServiceProxy.Basic
{
    public class BaServiceProxy : IBasicService
    {
        public string GetData(int value)
        {
            BasicServiceClient client = new BasicServiceClient();
            return client.GetData(value);
        }


        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            throw new NotImplementedException();
        }



        public Task<string> GetDataAsync(int value)
        {
            throw new NotImplementedException();
        }

        public Task<CompositeType> GetDataUsingDataContractAsync(CompositeType composite)
        {
            throw new NotImplementedException();
        }
    }
}
