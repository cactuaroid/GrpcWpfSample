using System.Threading;
using System.Threading.Tasks;

namespace GrpcWpfSample.Common
{
    public class AwaitableEvent<T>
    {
        private TaskCompletionSource<T> m_source = new TaskCompletionSource<T>();

        public Task<T> Invoked
        {
            get { return m_source.Task; }
        }

        public void Invoke(T value)
        {
            // Get current m_source and reset it atomically
            var source = Interlocked.Exchange(ref m_source, new TaskCompletionSource<T>());

            source.SetResult(value);
        }
    }
}
