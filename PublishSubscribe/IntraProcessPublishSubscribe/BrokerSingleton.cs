using PublishSubscribe.IPublishSubscribe;

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
