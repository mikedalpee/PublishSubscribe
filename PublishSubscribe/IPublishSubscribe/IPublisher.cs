using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublishSubscribe
{
    /// <summary>
    /// Defines a Publisher that can be used to notify Subscribers when changes to a Topic occur.
    /// </summary>
    /// <typeparam name="T">Topic Identifier type.</typeparam>
    public interface IPublisher<T> where T : IComparable<T>
    {
        /// <summary>
        /// Gets the Publisher's name.
        /// </summary>
        /// <value>
        /// The Name.
        /// </value>
        string Name
        {
            get;
        }
    }
}
