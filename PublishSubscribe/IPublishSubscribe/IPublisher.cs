using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublishSubscribe
{
    /// <summary>
    /// Defines a Publisher that can be used to notify Subscribers when changes to a Subject occur.
    /// </summary>
    /// <typeparam name="T">Subject Identifier type.</typeparam>
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
