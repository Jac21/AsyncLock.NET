![logo](https://raw.githubusercontent.com/Jac21/AsyncLock.NET/master/media/logo.png?token=AALB33ZYWW6VHBOH2VVGMYDBW24UY)

[![MIT Licence](https://badges.frapsoft.com/os/mit/mit.svg?v=103)](https://opensource.org/licenses/mit-license.php)
[![donate](https://img.shields.io/badge/%24-Buy%20me%20a%20coffee-ff69b4.svg?style=flat)](https://www.buymeacoffee.com/jac21) 

🔒 .NET Nuget for a basic asynchronous locking recipe

## Installation

Find it on [nuget](https://www.nuget.org/packages/AsyncLock.NET/)!

```
PM> Install-Package AsyncLock.NET -Version 1.0.0
```

## API 

### Example usage

```csharp
// synchronous operation
await AsyncLock.ExecuteWithLock(() => "Test");

// asynchronous operation
await AsyncLock.ExecuteWithLock(async () =>
{
    await Task.Delay(1000);

    return "Test";
});
```
