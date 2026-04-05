![logo](https://github.com/Jac21/AsyncLock.NET/blob/master/media/logo.png?raw=true)

[![NuGet Status](http://img.shields.io/nuget/v/AsyncLock.NET.svg?style=flat)](https://www.nuget.org/packages/AsyncLock.NET/)
[![MIT Licence](https://badges.frapsoft.com/os/mit/mit.svg?v=103)](https://opensource.org/licenses/mit-license.php)
[![donate](https://img.shields.io/badge/%24-Buy%20me%20a%20coffee-ff69b4.svg?style=flat)](https://www.buymeacoffee.com/jac21)

Lightweight async mutual exclusion and bounded concurrency primitives for modern .NET applications.

## Installation

Find it on [NuGet](https://www.nuget.org/packages/AsyncLock.NET/).

```powershell
PM> Install-Package AsyncLock.NET
```

## API

`AsyncLock.NET` now centers on an instance-based `AsyncLockNet.AsyncLock` type so callers can scope locking explicitly, compose it with dependency injection, and opt into cancellation.

### Lease pattern

```csharp
using AsyncLockNet;

var asyncLock = new AsyncLock();

using var releaser = await asyncLock.LockAsync(cancellationToken);
await connection.StartAsync(cancellationToken);
```

### Execute work with a returned result

```csharp
using AsyncLockNet;

var asyncLock = new AsyncLock();

var connection = await asyncLock.ExecuteAsync(
    () => GetConnectionAsync("https://that.place.on.the.internet/hub"),
    cancellationToken);
```

### Bounded concurrency

```csharp
using AsyncLockNet;

var asyncLock = new AsyncLock(maxConcurrency: 5);

await asyncLock.ExecuteAsync(async () =>
{
    await Task.Delay(1000, cancellationToken);
}, cancellationToken);
```

## Notes

- The library targets `net8.0` and `netstandard2.0`.
- `LockAsync` supports `CancellationToken`.
- `ExecuteAsync` overloads preserve return values for both synchronous and asynchronous delegates.
