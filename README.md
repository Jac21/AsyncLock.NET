![logo](https://github.com/Jac21/AsyncLock.NET/blob/master/media/logo.png?raw=true)

[![NuGet Status](http://img.shields.io/nuget/v/AsyncLock.NET.svg?style=flat)](https://www.nuget.org/packages/AsyncLock.NET/)
[![Build Status](https://app.travis-ci.com/Jac21/AsyncLock.NET.svg?branch=master)](https://app.travis-ci.com/Jac21/AsyncLock.NET)
[![MIT Licence](https://badges.frapsoft.com/os/mit/mit.svg?v=103)](https://opensource.org/licenses/mit-license.php)
[![donate](https://img.shields.io/badge/%24-Buy%20me%20a%20coffee-ff69b4.svg?style=flat)](https://www.buymeacoffee.com/jac21) 

🔒 .NET Nuget for a basic asynchronous locking recipe

## Installation

Find it on [nuget](https://www.nuget.org/packages/AsyncLock.NET/)!

```
PM> Install-Package AsyncLock.NET -Version 6.0.0
```

## API 

### Example usage

```csharp
// example of an asynchronous operation setting a SignalR connection
HubConnection connection;

await AsyncLock.ExecuteWithLock(async () =>
{
    connection = await GetConnection("https://that.place.on.the.internet/hub");
});

await connection.StartAsync();
```

```csharp
// example of setting the maximum number of requests for the semaphore 
// that can be granted concurrently which defaults to one
AsyncLock.MaxCount = 5;

// act
await AsyncLock.ExecuteWithLock(async () =>
{
    await Task.Delay(1000);

    return "Test";
});
```
