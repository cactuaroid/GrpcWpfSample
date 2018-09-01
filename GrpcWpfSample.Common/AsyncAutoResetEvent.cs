using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrpcWpfSample.Common
{
    public class AsyncAutoResetEvent<T>
    {
        private TaskCompletionSource<T> m_source = new TaskCompletionSource<T>();
        private readonly object m_lock = new object();

        public Task<T> WaitAsync()
        {
            lock (m_lock)
            {
                return m_source.Task;
            }
        }

        public void Set(T value)
        {
            TaskCompletionSource<T> source;
            lock (m_lock)
            {
                // Reset m_source before calling SetResult() because SetResult() might call WaitAsync().
                source = m_source;
                m_source = new TaskCompletionSource<T>();
            }

            source.SetResult(value);
        }
    }
}
