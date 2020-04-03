using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GrpcWpfSample.Client.Model
{
    public static class IAsyncStreamReaderExtensions
    {
        /// <summary>
        /// Convert IAsyncStreamReader to IAsyncEnumerable.
        /// </summary>
        /// <remarks>https://github.com/grpc/grpc/issues/6776</remarks>
        /// <typeparam name="T">sequence type</typeparam>
        /// <param name="asyncStreamReader">source IAsyncStreamReader</param>
        /// <returns>converted IAsyncEnumerable</returns>
        public static IAsyncEnumerable<T> ToAsyncEnumerable<T>(this IAsyncStreamReader<T> asyncStreamReader)
        {
            if (asyncStreamReader is null) { throw new ArgumentNullException(nameof(asyncStreamReader)); }

            return new ToAsyncEnumerableEnumerable<T>(asyncStreamReader);
        }

        private sealed class ToAsyncEnumerableEnumerable<T> : IAsyncEnumerable<T>
        {
            public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
                => new ToAsyncEnumeratorEnumerator<T>(_asyncStreamReader, cancellationToken);

            private readonly IAsyncStreamReader<T> _asyncStreamReader;

            public ToAsyncEnumerableEnumerable(IAsyncStreamReader<T> asyncStreamReader)
            {
                _asyncStreamReader = asyncStreamReader;
            }

            private sealed class ToAsyncEnumeratorEnumerator<T> : IAsyncEnumerator<T>
            {
                public T Current => _asyncStreamReader.Current;

                public async ValueTask<bool> MoveNextAsync() => await _asyncStreamReader.MoveNext(_cancellationToken);

                public ValueTask DisposeAsync() => default;

                private readonly IAsyncStreamReader<T> _asyncStreamReader;
                private readonly CancellationToken _cancellationToken;

                public ToAsyncEnumeratorEnumerator(IAsyncStreamReader<T> asyncStreamReader, CancellationToken cancellationToken)
                {
                    _asyncStreamReader = asyncStreamReader;
                    _cancellationToken = cancellationToken;
                }
            }
        }
    }
}
