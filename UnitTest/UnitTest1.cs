using Microsoft.VisualStudio.TestTools.UnitTesting;
using PublishSubscribe;
using PublishSubscribe.IntraProcessPublishSubscribe;
using PublishSubscribe.IPublishSubscribe;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        public class TestSubject : Subject<string>
        {
            public string Data { get; private set; }
            public int Notifications { get; set; } = 0;

            public TestSubject(string identifier, string data) : base(identifier)
            {
                 Data = data;
            }
        }
        public class TestSubscriber : Subscriber<string>
        {
            public TestSubscriber(string name) : base(name)
            {
            }

            public override void Notify(ISubject<string> subject, IPublisher<string> notifier)
            {
                TestSubject testSubject = (TestSubject)subject;

                Debug.WriteLine(
                    "Subscriber " + Name +
                    " Received Notification from Publisher " + notifier.Name +
                    " for Subject (" + testSubject.Identifier + "," + testSubject.Data + ")");

                testSubject.Notifications++;

            }
        }
        public static string GetCallSite(
            [CallerMemberName]string callerMemberName = "",
            [CallerFilePath]string callerFilePath = "",
            [CallerLineNumber]int callerLineNumber = 0)
        {
            return
                callerMemberName + ": " + callerFilePath + "@" + callerLineNumber;
        }

        public class MyBroker
        {
            private static readonly Lazy<BrokerSingleton<string>> s_instance = new Lazy<BrokerSingleton<string>>(() => new BrokerSingleton<string>(MyBroker.Name));

            public static string Name
            {
                get
                {
                    return "MyBroker";
                }
          }

            public static Broker<string> Instance
            {
                get
                {
                    return s_instance.Value.Instance(MyBroker.Name);
                }
            }

            public static void Reset()
            {
                s_instance.Value.Instance(MyBroker.Name).Unsubscribe();
                s_instance.Value.Instance(MyBroker.Name).Unregister();
            }
        }
        [TestMethod]
        public void BrokerSingletonTest()
        {
            Assert.AreEqual(MyBroker.Instance.Name, "MyBroker", false);
        }
        [TestMethod]
        public void PublisherNameTest()
        {
            string name = "Publisher";
            Publisher<string> publisher = new Publisher<string>(name);
            string propertyName = publisher.Name;
            Assert.AreEqual(name, propertyName, false);
        }
        [TestMethod]
        public void SubscriberNameTest()
        {
            string name = "Subscriber";
            Subscriber<string> subscriber = new TestSubscriber(name);
            string propertyName = subscriber.Name;
            Assert.AreEqual(name, propertyName, false);
        }
        [TestMethod]
        public void RegisterUnregisterTest()
        {
            MyBroker.Reset();

            string subjectIdentifier1 = "Subject1";
            string subjectIdentifier2 = "Subject2";

            // Create publisher1

            Publisher<string> publisher1 = new Publisher<string>("Publisher1");

            // Ensure publisher1 is not registered to Subject1 nor any other publishers are registered

            Assert.IsFalse(MyBroker.Instance.IsRegistered(publisher1, subjectIdentifier1));
            Assert.IsTrue(MyBroker.Instance.PublisherCount(subjectIdentifier1) == 0);
            Assert.IsTrue(MyBroker.Instance.PublisherCount() == 0);
            Assert.IsTrue(MyBroker.Instance.Publishers(subjectIdentifier1).Count == 0);
            Assert.IsTrue(MyBroker.Instance.Publishers().Count == 0);

            // Register publisher1 for subjectIdentifier1

            MyBroker.Instance.Register(publisher1, subjectIdentifier1);

            // Ensure publisher1 was registered

            Assert.IsTrue(MyBroker.Instance.IsRegistered(publisher1, subjectIdentifier1));
            Assert.IsTrue(MyBroker.Instance.PublisherCount(subjectIdentifier1) == 1);
            Assert.IsTrue(MyBroker.Instance.PublisherCount() == 1);
            Assert.IsTrue(MyBroker.Instance.Publishers(subjectIdentifier1).Count == 1);
            Assert.IsTrue(MyBroker.Instance.Publishers().Count == 1);

            // Ensure re-registering publisher1 has no effect

            MyBroker.Instance.Register(publisher1, subjectIdentifier1);

            Assert.IsTrue(MyBroker.Instance.IsRegistered(publisher1, subjectIdentifier1));
            Assert.IsTrue(MyBroker.Instance.PublisherCount(subjectIdentifier1) == 1);
            Assert.IsTrue(MyBroker.Instance.PublisherCount() == 1);
            Assert.IsTrue(MyBroker.Instance.Publishers(subjectIdentifier1).Count == 1);
            Assert.IsTrue(MyBroker.Instance.Publishers().Count == 1);
            // Create publisher2

            Publisher<string> publisher2 = new Publisher<string>("Publisher2");

            // Ensure this publisher2 is not registered to Subject2

            Assert.IsFalse(MyBroker.Instance.IsRegistered(publisher2, subjectIdentifier2));
            Assert.IsTrue(MyBroker.Instance.PublisherCount(subjectIdentifier2) == 0);

            // Register publisher2 for subjectIdentifier1 and subjectIdentifier2

            MyBroker.Instance.Register(publisher2, subjectIdentifier1);
            MyBroker.Instance.Register(publisher2, subjectIdentifier2);

            // Ensure publisher2 was registered

            Assert.IsTrue(MyBroker.Instance.IsRegistered(publisher2, subjectIdentifier1));
            Assert.IsTrue(MyBroker.Instance.IsRegistered(publisher2, subjectIdentifier2));
            Assert.IsTrue(MyBroker.Instance.PublisherCount(subjectIdentifier1) == 2);
            Assert.IsTrue(MyBroker.Instance.PublisherCount(subjectIdentifier2) == 1);
            Assert.IsTrue(MyBroker.Instance.PublisherCount() == 2);
            Assert.IsTrue(MyBroker.Instance.Publishers(subjectIdentifier1).Count == 2);
            Assert.IsTrue(MyBroker.Instance.Publishers(subjectIdentifier2).Count == 1);
            Assert.IsTrue(MyBroker.Instance.Publishers().Count == 2);

            // Unregister publisher1 form subjectIdentifier1

            MyBroker.Instance.Unregister(publisher1, subjectIdentifier1);

            // Ensure pubisher1 was unregistered

            Assert.IsFalse(MyBroker.Instance.IsRegistered(publisher1, subjectIdentifier1));
            Assert.IsTrue(MyBroker.Instance.PublisherCount(subjectIdentifier1) == 1);
            Assert.IsTrue(MyBroker.Instance.PublisherCount() == 1);
            Assert.IsTrue(MyBroker.Instance.Publishers(subjectIdentifier1).Count == 1);
            Assert.IsTrue(MyBroker.Instance.Publishers().Count == 1);

            // Unregister publisher2 form subjectIdentifier1

            MyBroker.Instance.Unregister(publisher2, subjectIdentifier1);

            // Ensure pubisher2 was unregistered

            Assert.IsFalse(MyBroker.Instance.IsRegistered(publisher2, subjectIdentifier1));
            Assert.IsTrue(MyBroker.Instance.PublisherCount(subjectIdentifier1) == 0);
            Assert.IsTrue(MyBroker.Instance.PublisherCount() == 1);
            Assert.IsTrue(MyBroker.Instance.Publishers(subjectIdentifier1).Count == 0);
            Assert.IsTrue(MyBroker.Instance.Publishers().Count == 1);

            // Unregister publisher2 form subjectIdentifier2

            MyBroker.Instance.Unregister(publisher2, subjectIdentifier2);

            // Ensure pubisher2 was unregistered

            Assert.IsFalse(MyBroker.Instance.IsRegistered(publisher2, subjectIdentifier2));
            Assert.IsTrue(MyBroker.Instance.PublisherCount(subjectIdentifier2) == 0);
            Assert.IsTrue(MyBroker.Instance.PublisherCount() == 0);
            Assert.IsTrue(MyBroker.Instance.Publishers(subjectIdentifier2).Count == 0);
            Assert.IsTrue(MyBroker.Instance.Publishers().Count == 0);

            // Reregister publisher1 and 2 as before

            MyBroker.Instance.Register(publisher1, subjectIdentifier1);
            MyBroker.Instance.Register(publisher2, subjectIdentifier1);
            MyBroker.Instance.Register(publisher2, subjectIdentifier2);

            // Ensure publisher 1 and publisher2 are registered

            Assert.IsTrue(MyBroker.Instance.IsRegistered(publisher1, subjectIdentifier1));
            Assert.IsTrue(MyBroker.Instance.IsRegistered(publisher2, subjectIdentifier1));
            Assert.IsTrue(MyBroker.Instance.IsRegistered(publisher2, subjectIdentifier2));
            Assert.IsTrue(MyBroker.Instance.PublisherCount(subjectIdentifier1) == 2);
            Assert.IsTrue(MyBroker.Instance.PublisherCount(subjectIdentifier2) == 1);
            Assert.IsTrue(MyBroker.Instance.PublisherCount() == 2);
            Assert.IsTrue(MyBroker.Instance.Publishers(subjectIdentifier1).Count == 2);
            Assert.IsTrue(MyBroker.Instance.Publishers(subjectIdentifier2).Count == 1);
            Assert.IsTrue(MyBroker.Instance.Publishers().Count == 2);

            // Unregister publisher2 from all subjects

            MyBroker.Instance.Unregister(publisher2);

            // Ensure publisher2 was unregistered

            Assert.IsTrue(MyBroker.Instance.IsRegistered(publisher1, subjectIdentifier1));
            Assert.IsFalse(MyBroker.Instance.IsRegistered(publisher2, subjectIdentifier1));
            Assert.IsFalse(MyBroker.Instance.IsRegistered(publisher2, subjectIdentifier2));
            Assert.IsTrue(MyBroker.Instance.PublisherCount(subjectIdentifier1) == 1);
            Assert.IsTrue(MyBroker.Instance.PublisherCount(subjectIdentifier2) == 0);
            Assert.IsTrue(MyBroker.Instance.PublisherCount() == 1);
            Assert.IsTrue(MyBroker.Instance.Publishers(subjectIdentifier1).Count == 1);
            Assert.IsTrue(MyBroker.Instance.Publishers(subjectIdentifier2).Count == 0);
            Assert.IsTrue(MyBroker.Instance.Publishers().Count == 1);

            // Unregister publisher1 from all subjects

            MyBroker.Instance.Unregister(publisher1);

            // Ensure publisher1 was unregistered

            Assert.IsFalse(MyBroker.Instance.IsRegistered(publisher1, subjectIdentifier1));
            Assert.IsTrue(MyBroker.Instance.PublisherCount(subjectIdentifier1) == 0);
            Assert.IsTrue(MyBroker.Instance.PublisherCount() == 0);
            Assert.IsTrue(MyBroker.Instance.Publishers(subjectIdentifier1).Count == 0);
            Assert.IsTrue(MyBroker.Instance.Publishers().Count == 0);

            // Reregister publisher1 and 2 as before

            MyBroker.Instance.Register(publisher1, subjectIdentifier1);
            MyBroker.Instance.Register(publisher2, subjectIdentifier1);
            MyBroker.Instance.Register(publisher2, subjectIdentifier2);

            // Ensure publisher 1 and publisher2 are registered

            Assert.IsTrue(MyBroker.Instance.IsRegistered(publisher1, subjectIdentifier1));
            Assert.IsTrue(MyBroker.Instance.IsRegistered(publisher2, subjectIdentifier1));
            Assert.IsTrue(MyBroker.Instance.IsRegistered(publisher2, subjectIdentifier2));
            Assert.IsTrue(MyBroker.Instance.PublisherCount(subjectIdentifier1) == 2);
            Assert.IsTrue(MyBroker.Instance.PublisherCount(subjectIdentifier2) == 1);
            Assert.IsTrue(MyBroker.Instance.PublisherCount() == 2);
            Assert.IsTrue(MyBroker.Instance.Publishers(subjectIdentifier1).Count == 2);
            Assert.IsTrue(MyBroker.Instance.Publishers(subjectIdentifier2).Count == 1);
            Assert.IsTrue(MyBroker.Instance.Publishers().Count == 2);

            // Unregister publisher1 and publisher2 from all subjects

            MyBroker.Instance.Unregister();

            // Ensure publisher1 and publisher2 were unregistered

            Assert.IsFalse(MyBroker.Instance.IsRegistered(publisher1, subjectIdentifier1));
            Assert.IsFalse(MyBroker.Instance.IsRegistered(publisher2, subjectIdentifier1));
            Assert.IsFalse(MyBroker.Instance.IsRegistered(publisher2, subjectIdentifier2));
            Assert.IsTrue(MyBroker.Instance.PublisherCount(subjectIdentifier1) == 0);
            Assert.IsTrue(MyBroker.Instance.PublisherCount(subjectIdentifier2) == 0);
            Assert.IsTrue(MyBroker.Instance.PublisherCount() == 0);
            Assert.IsTrue(MyBroker.Instance.Publishers(subjectIdentifier1).Count == 0);
            Assert.IsTrue(MyBroker.Instance.Publishers(subjectIdentifier2).Count == 0);
            Assert.IsTrue(MyBroker.Instance.Publishers().Count == 0);
        }
        [TestMethod]
        public void SubscribeUnsubscribeTest()
        {
            MyBroker.Reset();

            string subjectIdentifier1 = "Subject1";
            string subjectIdentifier2 = "Subject2";

            // Create publishers

            Publisher<string> publisher1 = new Publisher<string>("Publisher1");
            Publisher<string> publisher2 = new Publisher<string>("Publisher2");

            // Register publishers

            MyBroker.Instance.Register(publisher1, subjectIdentifier1);

            // Create subscribers

            Subscriber<string> subscriber1 = new TestSubscriber("Subscriber1");
            Subscriber<string> subscriber2 = new TestSubscriber("Subscriber2");
            Subscriber<string> subscriber3 = new TestSubscriber("Subscriber3");
            Subscriber<string> subscriber4 = new TestSubscriber("Subscriber4");

            // Ensure nothing is subscribed to the subjects

            Assert.IsFalse(MyBroker.Instance.IsSubscribed());

            Assert.IsFalse(MyBroker.Instance.IsSubscribed(subjectIdentifier1));
            Assert.IsFalse(MyBroker.Instance.IsSubscribed(subjectIdentifier2));

            Assert.IsFalse(MyBroker.Instance.IsSubscribed(subscriber1));
            Assert.IsFalse(MyBroker.Instance.IsSubscribed(subscriber2));
            Assert.IsFalse(MyBroker.Instance.IsSubscribed(subscriber3));
            Assert.IsFalse(MyBroker.Instance.IsSubscribed(subscriber4));

            Assert.IsFalse(MyBroker.Instance.IsSubscribed(subscriber1, subjectIdentifier1));
            Assert.IsFalse(MyBroker.Instance.IsSubscribed(subscriber1, subjectIdentifier2));
            Assert.IsFalse(MyBroker.Instance.IsSubscribed(subscriber2, subjectIdentifier1));
            Assert.IsFalse(MyBroker.Instance.IsSubscribed(subscriber2, subjectIdentifier2));
            Assert.IsFalse(MyBroker.Instance.IsSubscribed(subscriber3, subjectIdentifier1));
            Assert.IsFalse(MyBroker.Instance.IsSubscribed(subscriber3, subjectIdentifier2));
            Assert.IsFalse(MyBroker.Instance.IsSubscribed(subscriber4, subjectIdentifier1));
            Assert.IsFalse(MyBroker.Instance.IsSubscribed(subscriber4, subjectIdentifier2));

            // Subscribe subscriber1 to subjectIdentifier1

            MyBroker.Instance.Subscribe(subscriber1, subjectIdentifier1);

            // Ensure subscriber1 was subscribed to subjectIdentifier1

            Assert.IsTrue(MyBroker.Instance.IsSubscribed());

            Assert.IsTrue(MyBroker.Instance.IsSubscribed(subjectIdentifier1));
            Assert.IsFalse(MyBroker.Instance.IsSubscribed(subjectIdentifier2));

            Assert.IsTrue(MyBroker.Instance.IsSubscribed(subscriber1));
            Assert.IsFalse(MyBroker.Instance.IsSubscribed(subscriber2));
            Assert.IsFalse(MyBroker.Instance.IsSubscribed(subscriber3));
            Assert.IsFalse(MyBroker.Instance.IsSubscribed(subscriber4));

            Assert.IsTrue(MyBroker.Instance.IsSubscribed(subscriber1, subjectIdentifier1));
            Assert.IsFalse(MyBroker.Instance.IsSubscribed(subscriber1, subjectIdentifier2));
            Assert.IsFalse(MyBroker.Instance.IsSubscribed(subscriber2, subjectIdentifier1));
            Assert.IsFalse(MyBroker.Instance.IsSubscribed(subscriber2, subjectIdentifier2));
            Assert.IsFalse(MyBroker.Instance.IsSubscribed(subscriber3, subjectIdentifier1));
            Assert.IsFalse(MyBroker.Instance.IsSubscribed(subscriber3, subjectIdentifier2));
            Assert.IsFalse(MyBroker.Instance.IsSubscribed(subscriber4, subjectIdentifier1));
            Assert.IsFalse(MyBroker.Instance.IsSubscribed(subscriber4, subjectIdentifier2));

            MyBroker.Instance.Subscribe(subscriber2, subjectIdentifier1);
            MyBroker.Instance.Subscribe(subscriber2, subjectIdentifier2);
            MyBroker.Instance.Subscribe(subscriber3, subjectIdentifier1);
            MyBroker.Instance.Subscribe(subscriber3, subjectIdentifier2);
            MyBroker.Instance.Subscribe(subscriber4, subjectIdentifier2);

            Assert.IsTrue(MyBroker.Instance.SubscriberCount() == 4);
        }

        [TestMethod]
        public void PublishSubscribeTest()
        {
            MyBroker.Reset();

            string identifier1 = "subject1";
            string identifier2 = "subject2";

            Publisher<string> publisher1 = new Publisher<string>("Publisher1");
            Publisher<string> publisher2 = new Publisher<string>("Publisher2");

            Subscriber<string> subscriber1 = new TestSubscriber("Subscriber1");
            Subscriber<string> subscriber2 = new TestSubscriber("Subscriber2");
            Subscriber<string> subscriber3 = new TestSubscriber("Subscriber3");
            Subscriber<string> subscriber4 = new TestSubscriber("Subscriber4");

            MyBroker.Instance.Register(publisher1, identifier1);
            MyBroker.Instance.Register(publisher1, identifier2);
            MyBroker.Instance.Register(publisher2, identifier2);

            MyBroker.Instance.Subscribe(subscriber1, identifier1);
            MyBroker.Instance.Subscribe(subscriber2, identifier2);
            MyBroker.Instance.Subscribe(subscriber3, identifier1);
            MyBroker.Instance.Subscribe(subscriber3, identifier2);
            MyBroker.Instance.Subscribe(subscriber4, identifier2);

            TestSubject testSubject1;
            TestSubject testSubject2;

            MyBroker.Instance.Publish(publisher1, testSubject1 = new TestSubject(identifier1, "(testSubject1)"+GetCallSite()));
            MyBroker.Instance.Publish(publisher2, testSubject2 = new TestSubject(identifier2, "(testSubject2)"+GetCallSite()));

            Assert.IsTrue(testSubject1.Notifications == 2);
            Assert.IsTrue(testSubject2.Notifications == 3);

            MyBroker.Instance.Unsubscribe();

            MyBroker.Instance.Subscribe(subscriber1, identifier1, publisher1);
            MyBroker.Instance.Subscribe(subscriber1, identifier2, publisher1);
            MyBroker.Instance.Subscribe(subscriber2, identifier1, publisher2);
            MyBroker.Instance.Subscribe(subscriber3, identifier2, publisher1);
            MyBroker.Instance.Subscribe(subscriber4, identifier2, publisher2);

            MyBroker.Instance.Publish(publisher1, testSubject1 = new TestSubject(identifier1, "(testSubject1)" + GetCallSite()));

            Assert.IsTrue(testSubject1.Notifications == 1);

            MyBroker.Instance.Publish(publisher1, testSubject2 = new TestSubject(identifier2, "(testSubject2)" + GetCallSite()));

            Assert.IsTrue(testSubject2.Notifications == 2);

            MyBroker.Instance.Publish(publisher2, testSubject2 = new TestSubject(identifier2, "(testSubject2)" + GetCallSite()));

            Assert.IsTrue(testSubject2.Notifications == 1);
        }
    }
}
