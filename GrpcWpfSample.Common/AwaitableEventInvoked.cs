using System.Threading.Tasks;

namespace GrpcWpfSample.Common
{
    /// <summary>
    /// Argument of AwaitableEvent.Invoked
    /// </summary>
    public class AwaitableEventInvoked
    {
        /// <summary>
        /// Sender object if specified when invoked; otherwise null.
        /// </summary>
        public object Sender { get; }

        /// <summary>
        /// A task represents next event invocation.
        /// </summary>
        public Task<AwaitableEventInvoked> Next { get; }

        public AwaitableEventInvoked(object sender, Task<AwaitableEventInvoked> next)
        {
            Sender = sender;
            Next = next;
        }

        public override string ToString() => $"{{{nameof(Sender)} = {Sender}}}";
    }

    /// <summary>
    /// Argument of AwaitableEvent.Invoked
    /// </summary>
    /// <typeparam name="T">Event argument type</typeparam>
    public class AwaitableEventInvoked<T>
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
        public Task<AwaitableEventInvoked<T>> Next { get; }

        public AwaitableEventInvoked(object sender, T args, Task<AwaitableEventInvoked<T>> next)
        {
            Sender = sender;
            Args = args;
            Next = next;
        }

        public override string ToString() => $"{{{nameof(Sender)} = {Sender}, {nameof(Args)} = {Args}}}";

    }
}
