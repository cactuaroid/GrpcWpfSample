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

        public AwaitableEventInvoked(object sender)
        {
            Sender = sender;
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

        public AwaitableEventInvoked(object sender, T args)
        {
            Sender = sender;
            Args = args;
        }

        public override string ToString() => $"{{{nameof(Sender)} = {Sender}, {nameof(Args)} = {Args}}}";

    }
}
