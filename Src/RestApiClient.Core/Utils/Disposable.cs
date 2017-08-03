using System;
using System.Threading.Tasks;

namespace RestApiClient.Core.Utils
{
    public static class Disposable
    {
        public static TResult Using<TDisposable, TResult>(Func<TDisposable> factory, Func<TDisposable, TResult> fn) where TDisposable : IDisposable
        {
            using (var disposable = factory())
            {
                return fn(disposable);
            }
        }

        public static async Task<TResult> UsingAsync<TDisposable, TResult>(Func<Task<TDisposable>> factory, Func<TDisposable, Task<TResult>> fn)
            where TDisposable : IDisposable
        {
            using (var disposable = await factory().ConfigureAwait(false))
            {
                return await fn(disposable).ConfigureAwait(false);
            }
        }
    }
}
