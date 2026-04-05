using System.Threading;
using AsyncLockNet;
using NUnit.Framework;

namespace AsyncLock.Unit.Test;

public class AsyncLockTests
{
    [Test]
    public async Task ExecuteAsync_ReturnsSynchronousResult()
    {
        using var asyncLock = new global::AsyncLockNet.AsyncLock();

        var result = await asyncLock.ExecuteAsync(() => "Test");

        Assert.That(result, Is.EqualTo("Test"));
    }

    [Test]
    public async Task ExecuteAsync_ReturnsAsynchronousResult()
    {
        using var asyncLock = new global::AsyncLockNet.AsyncLock();

        var result = await asyncLock.ExecuteAsync(async () =>
        {
            await Task.Delay(25);
            return "Test";
        });

        Assert.That(result, Is.EqualTo("Test"));
    }

    [Test]
    public void Constructor_RejectsInvalidConcurrency()
    {
        Assert.That(() => new global::AsyncLockNet.AsyncLock(0), Throws.TypeOf<ArgumentOutOfRangeException>());
    }

    [Test]
    public async Task LockAsync_CanBeCancelled()
    {
        using var asyncLock = new global::AsyncLockNet.AsyncLock();
        using var heldLease = await asyncLock.LockAsync();
        using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));

        var act = async () => await asyncLock.LockAsync(cts.Token);

        Assert.That(act, Throws.InstanceOf<OperationCanceledException>());
    }

    [Test]
    public void LockAsync_ThrowsWhenDisposed()
    {
        var asyncLock = new global::AsyncLockNet.AsyncLock();
        asyncLock.Dispose();

        var act = async () => await asyncLock.LockAsync();

        Assert.That(act, Throws.InstanceOf<ObjectDisposedException>());
    }
}
