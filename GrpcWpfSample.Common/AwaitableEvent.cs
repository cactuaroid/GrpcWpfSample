using System.Threading;
using System.Threading.Tasks;

namespace GrpcWpfSample.Common
{
    /// <summary>
    /// Awaitable event
    /// </summary>
    public class AwaitableEvent
    {
        private TaskCompletionSource<AwaitableEventInvoked> m_source = new TaskCompletionSource<AwaitableEventInvoked>();
        private readonly object m_lock = new object();

        /// <summary>
        /// A task represents next Invoke() called event.
        /// </summary>
        public Task<AwaitableEventInvoked> Next => m_source.Task;

        /// <summary>
        /// Invoke a event.
        /// </summary>
        public void Invoke() => Invoke(null);

        /// <summary>
        /// Invoke a event with sender.
        /// </summary>
        /// <param name="sender">sender object</param>
        public void Invoke(object sender)
        {
            // Get current m_source and reset it atomically
            lock (m_lock)
            {
                var source = m_source;
                m_source = new TaskCompletionSource<AwaitableEventInvoked>();

                source.SetResult(new AwaitableEventInvoked(sender, m_source.Task));
            }
        }
    }

    /// <summary>
    /// Awaitable event with argument
    /// </summary>
    /// <typeparam name="T">Event argument type</typeparam>
    public class AwaitableEvent<T>
    {
        private TaskCompletionSource<AwaitableEventInvoked<T>> m_source = new TaskCompletionSource<AwaitableEventInvoked<T>>();
        private readonly object m_lock = new object();

        /// <summary>
        /// A task represents next Invoke() called event.
        /// </summary>
        public Task<AwaitableEventInvoked<T>> Next => m_source.Task;

        /// <summary>
        /// Invoke a event with argument.
        /// </summary>
        /// <param name="value">event argument</param>
        public void Invoke(T value) => Invoke(null, value);

        /// <summary>
        /// Invoke a event with sender and argument.
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="value">event argument</param>
        public void Invoke(object sender, T value)
        {
            // Get current m_source and reset it atomically
            lock (m_lock)
            {
                var source = m_source;
                m_source = new TaskCompletionSource<AwaitableEventInvoked<T>>();

                source.SetResult(new AwaitableEventInvoked<T>(sender, value, m_source.Task));
            }
        }
    }
}
