using System.Threading.Tasks;

namespace GrpcWpfSample.Common
{
    /// <summary>
    /// Argument of AsyncEnumerableEvent.Invoked
    /// </summary>
    public class AsyncEnumerableEventInvoked
    {
        /// <summary>
        /// Sender object if specified when invoked; otherwise null.
        /// </summary>
        public object Sender { get; }

        /// <summary>
        /// A task represents next event invocation.
        /// </summary>
        public Task<AsyncEnumerableEventInvoked> Next { get; }

        public AsyncEnumerableEventInvoked(object sender, Task<AsyncEnumerableEventInvoked> next)
        {
            Sender = sender;
            Next = next;
        }

        public override string ToString() => $"{{{nameof(Sender)} = {Sender}}}";
    }

    /// <summary>
    /// Argument of AsyncEnumerableEvent.Invoked
    /// </summary>
    /// <typeparam name="T">Event argument type</typeparam>
    public class AsyncEnumerableEventInvoked<T>
    {
        /// <summary>
        /// Sender object if specified when invoked; otherwise null.
        /// </summary>
        public object Sender { get; }

        /// <summary>
        /// Event argument object if specified when invoked; otherwise default(T).
        /// </summary>
        public T Args { get; }

        /// <summary>
        /// A task represents next event invocation.
        /// </summary>
        public Task<AsyncEnumerableEventInvoked<T>> Next { get; }

        public AsyncEnumerableEventInvoked(object sender, T args, Task<AsyncEnumerableEventInvoked<T>> next)
        {
            Sender = sender;
            Args = args;
            Next = next;
        }

        public override string ToString() => $"{{{nameof(Sender)} = {Sender}, {nameof(Args)} = {Args}}}";

    }
}
