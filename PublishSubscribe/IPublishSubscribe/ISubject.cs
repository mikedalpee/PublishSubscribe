using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublishSubscribe
{
    /// <summary>
    /// Defines a Subject that is managed by a Publisher. It is uniquely identified by a Subject Identifier that is embedded withing the Subject.
     /// </summary>
    /// <typeparam name="T">Subject Identifier type..</typeparam>
    public interface ISubject<T> where T : IComparable<T>
    {
        /// <summary>
        /// Gets the Subject Identifier.
        /// </summary>
        /// <value>
        /// The Subject Identifier.
        /// </value>
        T Identifier
        {
            get;
        }
    }
}
