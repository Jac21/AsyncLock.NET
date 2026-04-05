using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncLockNet;

/// <summary>
/// Provides an async-compatible mutual exclusion lock for coordinating access to shared state.
/// </summary>
public sealed class AsyncLock : IDisposable
{
    private readonly SemaphoreSlim _semaphore;
    private bool _disposed;

    /// <summary>
    /// Initializes a new <see cref="AsyncLock"/> instance.
    /// </summary>
    /// <param name="maxConcurrency">
    /// The maximum number of concurrent callers allowed to enter the lock.
    /// Defaults to <c>1</c>, which behaves as a mutex.
    /// </param>
    public AsyncLock(int maxConcurrency = 1)
    {
        if (maxConcurrency <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maxConcurrency), "The concurrency limit must be greater than zero.");
        }

        MaxConcurrency = maxConcurrency;
        _semaphore = new SemaphoreSlim(maxConcurrency, maxConcurrency);
    }

    /// <summary>
    /// Gets the configured concurrency limit for this lock instance.
    /// </summary>
    public int MaxConcurrency { get; }

    /// <summary>
    /// Waits asynchronously to enter the lock and returns a lease that releases it when disposed.
    /// </summary>
    public async Task<Releaser> LockAsync(CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        await _semaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
        return new Releaser(_semaphore);
    }

    /// <summary>
    /// Executes an action while holding the lock.
    /// </summary>
    public async Task ExecuteAsync(Action action, CancellationToken cancellationToken = default)
    {
        if (action is null)
        {
            throw new ArgumentNullException(nameof(action));
        }

        using var releaser = await LockAsync(cancellationToken).ConfigureAwait(false);
        action();
    }

    /// <summary>
    /// Executes a function while holding the lock and returns its result.
    /// </summary>
    public async Task<T> ExecuteAsync<T>(Func<T> action, CancellationToken cancellationToken = default)
    {
        if (action is null)
        {
            throw new ArgumentNullException(nameof(action));
        }

        using var releaser = await LockAsync(cancellationToken).ConfigureAwait(false);
        return action();
    }

    /// <summary>
    /// Executes an asynchronous action while holding the lock.
    /// </summary>
    public async Task ExecuteAsync(Func<Task> action, CancellationToken cancellationToken = default)
    {
        if (action is null)
        {
            throw new ArgumentNullException(nameof(action));
        }

        using var releaser = await LockAsync(cancellationToken).ConfigureAwait(false);
        await action().ConfigureAwait(false);
    }

    /// <summary>
    /// Executes an asynchronous function while holding the lock and returns its result.
    /// </summary>
    public async Task<T> ExecuteAsync<T>(Func<Task<T>> action, CancellationToken cancellationToken = default)
    {
        if (action is null)
        {
            throw new ArgumentNullException(nameof(action));
        }

        using var releaser = await LockAsync(cancellationToken).ConfigureAwait(false);
        return await action().ConfigureAwait(false);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _semaphore.Dispose();
        _disposed = true;
    }

    private void ThrowIfDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(AsyncLock));
        }
    }

    /// <summary>
    /// Releases a held <see cref="AsyncLock"/> lease when disposed.
    /// </summary>
    public readonly struct Releaser : IDisposable
    {
        private readonly SemaphoreSlim? _semaphore;

        internal Releaser(SemaphoreSlim semaphore)
        {
            _semaphore = semaphore;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _semaphore?.Release();
        }
    }
}
