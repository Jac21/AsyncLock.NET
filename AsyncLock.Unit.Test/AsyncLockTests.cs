using System.Threading.Tasks;
using NUnit.Framework;

namespace AsyncLock.Unit.Test
{
    public class AsyncLockTests
    {
        [SetUp]
        public void SetUp()
        {
            AsyncLock.MaxCount = 1;
        }

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

        [Test]
        public async Task AsyncLock_SetMaxCount_ExecuteWithLock_Async_Success_Test()
        {
            // arrange
            AsyncLock.MaxCount = 5;

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