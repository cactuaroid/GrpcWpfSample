using System.Threading;
using System.Threading.Tasks;

namespace GrpcWpfSample.Common
{
    public class AwaitableEvent<T>
    {
        private TaskCompletionSource<AwaitableEventInvoked<T>> m_source = new TaskCompletionSource<AwaitableEventInvoked<T>>();

        public Task<AwaitableEventInvoked<T>> Invoked
        {
            get { return m_source.Task; }
        }

        public void Invoke(T value)
        {
            Invoke(null, value);
        }

        public void Invoke(object sender, T value)
        {
            // Get current m_source and reset it atomically
            var source = Interlocked.Exchange(ref m_source, new TaskCompletionSource<AwaitableEventInvoked<T>>());

            source.SetResult(new AwaitableEventInvoked<T>(sender, value));
        }
    }
}
