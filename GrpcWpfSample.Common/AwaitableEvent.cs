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
            // Reset m_source before calling SetResult() because SetResult() might call Awaiter.
            var source = m_source;
            m_source = new TaskCompletionSource<T>();

            source.SetResult(value);
        }
    }
}
