using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DivvyUp.Web.Tests
{
    public static class MockDbSetExtensions
    {
        public static Mock<DbSet<T>> ToMockDbSet<T>(this IEnumerable<T> source) where T : class
        {
            var queryable = source.AsQueryable();
            var mockDbSet = new Mock<DbSet<T>>();

            mockDbSet.As<IQueryable<T>>().Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<T>(queryable.Provider));
            mockDbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());
            mockDbSet.As<IAsyncEnumerable<T>>()
                .Setup(m => m.GetAsyncEnumerator(default))
                .Returns(new TestAsyncEnumerator<T>(queryable.GetEnumerator()));

            return mockDbSet;
        }

        private class TestAsyncQueryProvider<T> : IAsyncQueryProvider
        {
            private readonly IQueryProvider _inner;

            public TestAsyncQueryProvider(IQueryProvider inner)
            {
                _inner = inner;
            }

            public IQueryable CreateQuery(Expression expression)
            {
                return new TestAsyncEnumerable<T>(expression);
            }

            public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
            {
                return new TestAsyncEnumerable<TElement>(expression);
            }

            public object Execute(Expression expression)
            {
                return _inner.Execute(expression);
            }

            public TResult Execute<TResult>(Expression expression)
            {
                return _inner.Execute<TResult>(expression);
            }

            public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
            {
                if (typeof(TResult).IsGenericType && typeof(TResult).GetGenericTypeDefinition() == typeof(Task<>))
                {
                    var resultType = typeof(TResult).GetGenericArguments()[0];
                    var executionResult = _inner.Execute(expression);

                    return (TResult)typeof(Task)
                        .GetMethod(nameof(Task.FromResult))
                        ?.MakeGenericMethod(resultType)
                        .Invoke(null, new[] { executionResult });
                }

                throw new InvalidOperationException("Invalid async execution");
            }
        }

        private class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
        {
            public TestAsyncEnumerable(IEnumerable<T> enumerable) : base(enumerable)
            {
            }

            public TestAsyncEnumerable(Expression expression) : base(expression)
            {
            }

            public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
            {
                return new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
            }
        }

        private class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
        {
            private readonly IEnumerator<T> _inner;

            public TestAsyncEnumerator(IEnumerator<T> inner)
            {
                _inner = inner;
            }

            public T Current => _inner.Current;

            public ValueTask DisposeAsync()
            {
                _inner.Dispose();
                return ValueTask.CompletedTask;
            }

            public ValueTask<bool> MoveNextAsync()
            {
                return new ValueTask<bool>(_inner.MoveNext());
            }
        }
    }
}