using System.Threading.Tasks;
using NUnit.Framework;

namespace AsyncLock.Unit.Test
{
    public class AsyncLockTests
    {
        [Test]
        public async Task AsyncLock_ExecuteWithLock_Success_Test()
        {
            // arrange

            // act
            await AsyncLock.ExecuteWithLock(() => "Test");

            // assert
            Assert.Pass();
        }

        [Test]
        public async Task AsyncLock_ExecuteWithLock_Async_Success_Test()
        {
            // arrange

            // act
            await AsyncLock.ExecuteWithLock(async () =>
            {
                await Task.Delay(1000);

                return "Test";
            });

            // assert
            Assert.Pass();
        }
    }
}