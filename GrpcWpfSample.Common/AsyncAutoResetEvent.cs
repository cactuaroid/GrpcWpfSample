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

        public Task<T> WaitAsync()
        {
            return m_source.Task;
        }

        public void Set(T value)
        {
            // Reset m_source before calling SetResult() because SetResult() might call WaitAsync().
            var source = m_source;
            m_source = new TaskCompletionSource<T>();

            source.SetResult(value);
        }
    }
}
