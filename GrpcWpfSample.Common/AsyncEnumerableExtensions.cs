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

        // from https://github.com/maca88/AsyncGenerator/issues/94#issuecomment-385286972
        public static IAsyncEnumerable<TResult> SelectAsync<T, TResult>(this IEnumerable<T> enumerable, Func<T, Task<TResult>> selector)
        {
            return AsyncEnumerable.CreateEnumerable(() =>
            {
                var enumerator = enumerable.GetEnumerator();
                var current = default(TResult);
                return AsyncEnumerable.CreateEnumerator(async c =>
                    {
                        var moveNext = enumerator.MoveNext();
                        current = moveNext
                            ? await selector(enumerator.Current).ConfigureAwait(false)
                            : default(TResult);
                        return moveNext;
                    },
                    () => current,
                    () => enumerator.Dispose());
            });
        }
    }
}
