using System.Collections.Concurrent;
using System.Threading;
using AsyncLockNet;
using NUnit.Framework;

namespace AsyncLock.Integration.Test;

public class AsyncLockTests
{
    [Test]
    public async Task ExecuteAsync_WithDefaultConcurrency_AllowsOnlyOneCallerAtATime()
    {
        using var asyncLock = new global::AsyncLockNet.AsyncLock();

        var activeCount = 0;
        var maxActiveCount = 0;

        var tasks = Enumerable.Range(0, 8).Select(async _ =>
        {
            await asyncLock.ExecuteAsync(async () =>
            {
                var current = Interlocked.Increment(ref activeCount);
                Interlocked.Exchange(ref maxActiveCount, Math.Max(maxActiveCount, current));

                await Task.Delay(50);

                Interlocked.Decrement(ref activeCount);
            });
        });

        await Task.WhenAll(tasks);

        Assert.That(maxActiveCount, Is.EqualTo(1));
    }

    [Test]
    public async Task ExecuteAsync_WithConfiguredConcurrency_HonorsLimit()
    {
        using var asyncLock = new global::AsyncLockNet.AsyncLock(maxConcurrency: 3);

        var activeCount = 0;
        var maxActiveCount = 0;

        var tasks = Enumerable.Range(0, 12).Select(async _ =>
        {
            await asyncLock.ExecuteAsync(async () =>
            {
                var current = Interlocked.Increment(ref activeCount);
                Interlocked.Exchange(ref maxActiveCount, Math.Max(maxActiveCount, current));

                await Task.Delay(50);

                Interlocked.Decrement(ref activeCount);
            });
        });

        await Task.WhenAll(tasks);

        Assert.That(maxActiveCount, Is.LessThanOrEqualTo(3));
        Assert.That(maxActiveCount, Is.EqualTo(3));
    }

    [Test]
    public async Task LockAsync_LeasePattern_ReleasesForSubsequentCallers()
    {
        using var asyncLock = new global::AsyncLockNet.AsyncLock();
        var order = new ConcurrentQueue<string>();

        var first = Task.Run(async () =>
        {
            using var releaser = await asyncLock.LockAsync();
            order.Enqueue("first-enter");
            await Task.Delay(75);
            order.Enqueue("first-exit");
        });

        var second = Task.Run(async () =>
        {
            await Task.Delay(10);
            using var releaser = await asyncLock.LockAsync();
            order.Enqueue("second-enter");
        });

        await Task.WhenAll(first, second);

        Assert.That(order.ToArray(), Is.EqualTo(new[] { "first-enter", "first-exit", "second-enter" }));
    }
}
