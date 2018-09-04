using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcWpfSample.Common
{
    /// <summary>
    /// IAsyncEnumerable version of event.
    /// </summary>
    public class AsyncEnumerableEvent : IAsyncEnumerable<AsyncEnumerableEventInvoked>
    {
        private TaskCompletionSource<AsyncEnumerableEventInvoked> m_source = new TaskCompletionSource<AsyncEnumerableEventInvoked>(TaskCreationOptions.RunContinuationsAsynchronously);
        private readonly object m_lock = new object();

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
                m_source = new TaskCompletionSource<AsyncEnumerableEventInvoked>(TaskCreationOptions.RunContinuationsAsynchronously);

                source.SetResult(new AsyncEnumerableEventInvoked(sender, m_source.Task));
            }
        }

        /// <summary>
        /// IAsyncEnumerable implementation
        /// </summary>
        /// <returns>IAsyncEnumerator</returns>
        public IAsyncEnumerator<AsyncEnumerableEventInvoked> GetEnumerator()
        {
            AsyncEnumerableEventInvoked current = null;
            Task<AsyncEnumerableEventInvoked> next = m_source.Task;
            return AsyncEnumerable.CreateEnumerator(async c =>
                {
                    var invoked = await next.ConfigureAwait(false);
                    next = invoked.Next;
                    current = invoked;
                    return true;
                },
                () => current,
                () => { });
        }
    }

    /// <summary>
    /// IAsyncEnumerable version of event.
    /// </summary>
    /// <typeparam name="T">Event argument type</typeparam>
    public class AsyncEnumerableEvent<T> : IAsyncEnumerable<AsyncEnumerableEventInvoked<T>>
    {
        private TaskCompletionSource<AsyncEnumerableEventInvoked<T>> m_source = new TaskCompletionSource<AsyncEnumerableEventInvoked<T>>(TaskCreationOptions.RunContinuationsAsynchronously);
        private readonly object m_lock = new object();

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
                m_source = new TaskCompletionSource<AsyncEnumerableEventInvoked<T>>(TaskCreationOptions.RunContinuationsAsynchronously);

                source.SetResult(new AsyncEnumerableEventInvoked<T>(sender, value, m_source.Task));
            }
        }

        /// <summary>
        /// IAsyncEnumerable implementation
        /// </summary>
        /// <returns>IAsyncEnumerator</returns>
        public IAsyncEnumerator<AsyncEnumerableEventInvoked<T>> GetEnumerator()
        {
            AsyncEnumerableEventInvoked<T> current = null;
            Task<AsyncEnumerableEventInvoked<T>> next = m_source.Task;
            return AsyncEnumerable.CreateEnumerator(async c =>
                {
                    var invoked = await next.ConfigureAwait(false);
                    next = invoked.Next;
                    current = invoked;
                    return true;
                },
                () => current,
                () => { });
        }
    }
}
