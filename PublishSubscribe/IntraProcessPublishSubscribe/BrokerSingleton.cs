using PublishSubscribe.IPublishSubscribe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublishSubscribe.IntraProcessPublishSubscribe
{
    public class BrokerSingleton<T> : ISingleton<Broker<T>> where T : System.IComparable<T>
    {
        public BrokerSingleton(string name)
        :
            base(name,(x)=>new Broker<T>(x))
        {
        }
    }
}
