using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrpcWpfSample.Common
{
    public static class AsyncEnumerableExtensions
    {
        /// <summary>
        /// ForEachAsync for async lambda.
        /// </summary>
        /// <remarks>
        /// https://github.com/dotnet/reactive/issues/144
        /// </remarks>
        /// <typeparam name="TSource">source type</typeparam>
        /// <param name="source">source sequence</param>
        /// <param name="funcTask">foreach body</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
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

        /// <summary>
        /// Projects each element of a sequence into a new form of IAsyncEnumerable.
        /// </summary>
        /// <remarks>
        /// https://github.com/maca88/AsyncGenerator/issues/94#issuecomment-385286972
        /// </remarks>
        /// <typeparam name="T">source type</typeparam>
        /// <typeparam name="TResult">result type</typeparam>
        /// <param name="enumerable">A sequence of values to invoke a transform function on</param>
        /// <param name="selector">A transform function to apply to each element</param>
        /// <returns>An IAsyncEnumerable whose elements are the result of invoking the transform function on each element of source.</returns>
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
