namespace GrpcWpfSample.Common
{
    public class AwaitableEventInvoked<T>
    {
        public object Sender { get; }
        public T Args { get; }

        public AwaitableEventInvoked(object sender, T args)
        {
            Sender = sender;
            Args = args;
        }

        public override string ToString()
        {
            return $"{{{nameof(Sender)} = {Sender}, {nameof(Args)} = {Args}}}";
        }
    }
}
