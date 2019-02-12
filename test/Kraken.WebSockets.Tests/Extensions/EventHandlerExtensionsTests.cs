using Kraken.WebSockets.Events;
using Kraken.WebSockets.Messages;
using System;
using Kraken.WebSockets.Extensions;
using Xunit;
using System.Diagnostics.CodeAnalysis;

namespace Kraken.WebSockets.Tests.Extensions
{
    [ExcludeFromCodeCoverage]
    public class EventHandlerExtensionsTests
    {
        private EventEmitter eventEmitter;

        public EventHandlerExtensionsTests()
        {
            eventEmitter = new EventEmitter();
        }

        #region InvokeAll

        [Fact]
        public void InvokeAll_CanExecuteWithNoEventHandlerAttached()
        {
            eventEmitter.InvokeTestEvent(new TestMessage());
        }

        [Fact]
        public void InvokeAll_WithSingleEventHandlerAttached_CanExecute()
        {
            var executed = false;
            TestMessage eventMessage = null; 
            eventEmitter.TestEvent += (sender, eventArgs) => {
                executed = true;
                eventMessage = eventArgs.Message;
            };
            TestMessage testMessage = new TestMessage();
            eventEmitter.InvokeTestEvent(testMessage);
            Assert.True(executed);
            Assert.Equal(testMessage, eventMessage);
        }

        #endregion

        #region EventEmitter & TestMessage

        private class TestMessage : IKrakenMessage
        {
            public TestMessage()
            {
            }

            public string Event => "Test";
        }

        private class EventEmitter
        {
            public event EventHandler<KrakenMessageEventArgs<TestMessage>> TestEvent;

            public void InvokeTestEvent(TestMessage testMessage)
            {
                TestEvent.InvokeAll(new object(), testMessage);
            }
        }

        #endregion
    }
}
