using System;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncLock
{
    public static class AsyncLock
    {
        /// <summary>
        /// Set limit to a maximum of a single lock
        /// </summary>
        private static readonly SemaphoreSlim Lock = new SemaphoreSlim(1, 1);

        public static async Task ExecuteWithLock<T>(Task<Func<T>> f)
        {
            // grab the single lock
            await Lock.WaitAsync();

            try
            {
                await f;
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