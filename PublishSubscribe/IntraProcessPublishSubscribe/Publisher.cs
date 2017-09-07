using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublishSubscribe.IntraProcessPublishSubscribe
{
    public class Publisher<T> : IPublisher<T> where T : IComparable<T>
    {
        public Publisher(string name)
        {
            Name = name;
         }
        public string Name
        {
            get;
            private set;
        }
     }
}
