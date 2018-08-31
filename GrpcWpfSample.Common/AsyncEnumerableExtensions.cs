using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrpcWpfSample.Common
{
    public static class AsyncEnumerableExtensions
    {
        // refer https://github.com/dotnet/reactive/issues/144
        public static async Task ForEachTaskAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task> funcTask)
        {
            using (var e = source.GetEnumerator())
            {
                while (await e.MoveNext().ConfigureAwait(false))
                {
                    await funcTask(e.Current).ConfigureAwait(false);
                }
            }
        }
    }
}
