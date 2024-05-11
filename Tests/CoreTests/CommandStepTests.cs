using Moq;
using SK.TinyScheduler.API;
using SK.TinyScheduler.Core;

namespace CoreTests
{
    public class CommandStepTests
    {
        [Fact(Skip = "Platform-specific test")]
        public async Task CommandStep_Test() 
        {
            var _mockJobContext = new Mock<IJobContext>();
            var _mockStepConfig = new Mock<IExecutableStepConfiguration>();
            _mockStepConfig.Setup(x => x.Script).Returns("echo OK");

            var step = new CommandStep(_mockJobContext.Object, _mockStepConfig.Object);
            await step.ExecutePayloadAsync(CancellationToken.None);

            Assert.True(true); // If no exceptions happened that's ok for now.
        }
    }
}
