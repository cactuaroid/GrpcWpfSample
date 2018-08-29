using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrpcWpfSample.Common
{
    public class AsyncAutoResetEvent<T>
    {
        private readonly List<TaskCompletionSource<T>> m_signalSources = new List<TaskCompletionSource<T>>();
        private readonly object m_lock = new object();

        public T Current { get; private set; }

        public Task<T> WaitAsync()
        {
            lock (m_lock)
            {
                var source = new TaskCompletionSource<T>();
                m_signalSources.Add(source);

                return source.Task;
            }
        }

        public void Set(T value)
        {
            lock (m_lock)
            {
                Current = value;

                // Clear m_signalSources before calling SetResult() which synchronously restarts awaiters of WhenSet().
                var sources = m_signalSources.ToArray();
                m_signalSources.Clear();

                foreach (var source in sources)
                {
                    source.SetResult(value);
                }
            }
        }
    }
}
