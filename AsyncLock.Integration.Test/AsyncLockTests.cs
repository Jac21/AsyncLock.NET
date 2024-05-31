#nullable enable
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using NUnit.Framework;
using Shouldly;

namespace AsyncLock.Integration.Test
{
    public class AsyncLockTests
    {
        // Internal State
        private HubConnection? _connection;

        public async Task<HubConnection?> GetConnection(string url)
        {
            if (_connection == null)
            {
                try
                {
                    // We check the _connection again after the lock.
                    // We do this since another call can create it.
                    if (_connection != null)
                    {
                        return _connection;
                    }

                    _connection = new HubConnectionBuilder()
                        .WithUrl(url)
                        .Build();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);

                    throw;
                }
            }

            return _connection;
        }

        [Test]
        public async Task AsyncLock_GetConnection_Success_Test()
        {
            // arrange
            HubConnection? connection = null;

            // act
            await AsyncLock.ExecuteWithLock(async () =>
            {
                connection = await GetConnection("https://that.place.on.the.internet/hub");
            });

            // assert
            connection.ShouldBeNull();
        }
    }
}