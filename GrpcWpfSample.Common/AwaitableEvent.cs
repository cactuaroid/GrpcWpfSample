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

        /// <summary>
        /// 'await' this property to await Invoke() called.
        /// </summary>
        public Task<AwaitableEventInvoked> Invoked => m_source.Task;

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
            var source = Interlocked.Exchange(ref m_source, new TaskCompletionSource<AwaitableEventInvoked>());

            source.SetResult(new AwaitableEventInvoked(sender));
        }
    }

    /// <summary>
    /// Awaitable event with argument
    /// </summary>
    /// <typeparam name="T">Event argument type</typeparam>
    public class AwaitableEvent<T>
    {
        private TaskCompletionSource<AwaitableEventInvoked<T>> m_source = new TaskCompletionSource<AwaitableEventInvoked<T>>();

        /// <summary>
        /// 'await' this property to await Invoke() called.
        /// </summary>
        public Task<AwaitableEventInvoked<T>> Invoked => m_source.Task;

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
            var source = Interlocked.Exchange(ref m_source, new TaskCompletionSource<AwaitableEventInvoked<T>>());

            source.SetResult(new AwaitableEventInvoked<T>(sender, value));
        }
    }
}
