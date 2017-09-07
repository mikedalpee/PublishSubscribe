/// <summary>
/// Provides an abstract definition of the Observer pattern.  In this definition, a Subscriber is notified of changes to a Subject that is managed by a Publisher.
/// The associations between Subscribers, Subjects, and Publishers is maintained by a Broker.  A Publisher registers with the Broker to announce the availability of Subjects.
/// Subscribers use the Broker to subscribe to Subjects of interest.  When the Publisher changes the Subject, it uses the Broker to publish Subject change notifications
/// to all current Subscribers.
/// The Broker decouples Subscribers from having to locate specific Publishers in order to subscribe to a Subject. The Publisher assigns each Subject it manages a unique
/// Subject Identifer, which can be an arbitrary Comparable type.  The Subject Identifier is subsequently used by Subscriber's to create a subscription. This approach
/// thus supports a completely location-transparent implementation, where Publishers and Subscribers can reside in the same process, in different processes, or on different
/// computers. Such implementations will become increasingly complex, ultimately involving the need to assign communication endpoints to Subscribers and Publishers, as well as
/// marshaling Subjects for delivery into separate address spaces.
/// The application is responsible for creating Brokers and providing a mechanism for locating them.  This mechanism could be as simple as a global singleton Broker reference,
/// or it could be a more complex scheme based on searching a distributed repository of Brokers identified by a federated naming convention.
/// </summary>
namespace PublishSubscribe
{
}
