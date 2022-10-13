using System;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncLock
{
    public static class AsyncLock
    {
        /// <summary>
        /// The maximum number of requests for the semaphore that can be granted concurrently.
        /// Defaults to 1.
        /// </summary>
        public static int MaxCount { get; set; } = 1;

        /// <summary>
        /// Set lock limit to a configured maximum
        /// </summary>
        private static readonly SemaphoreSlim Lock = new(1, MaxCount);

        public static async Task ExecuteWithLock<T>(Func<T> f)
        {
            // grab the single lock
            await Lock.WaitAsync();

            try
            {
                f();
            }
            catch (Exception ex)
            {
                ExceptionDispatchInfo.Capture(ex);

                throw;
            }
            finally
            {
                Lock.Release();
            }
        }

        public static async Task ExecuteWithLock<T>(Func<Task<T>> f)
        {
            // grab the single lock
            await Lock.WaitAsync();

            try
            {
                await f();
            }
            catch (Exception ex)
            {
                ExceptionDispatchInfo.Capture(ex);

                throw;
            }
            finally
            {
                Lock.Release();
            }
        }
    }
}