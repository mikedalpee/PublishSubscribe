using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublishSubscribe
{
    /// <summary>
    /// Defines a Topic that is managed by a Publisher. It is uniquely identified by a Topic Identifier that is embedded withing the Topic.
     /// </summary>
    /// <typeparam name="T">Topic Identifier type..</typeparam>
    public interface ITopic<T> where T : IComparable<T>
    {
        /// <summary>
        /// Gets the Topic Identifier.
        /// </summary>
        /// <value>
        /// The Topic Identifier.
        /// </value>
        T Identifier
        {
            get;
        }
    }
}
