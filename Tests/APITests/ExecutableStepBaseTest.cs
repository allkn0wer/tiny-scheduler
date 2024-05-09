using Moq;
using SK.TinyScheduler.API;

namespace APITests
{
    public class ExecutableStepBaseTest
    {
        private Mock<IJobContext> _mockJobContext;
        private Mock<IExecutableStepConfiguration> _mockConfiguration;
        private Mock<ExecutableStepBase> _mockStep;

        public ExecutableStepBaseTest()
        {
            // Arrange
            _mockJobContext = new Mock<IJobContext>();
            _mockConfiguration = new Mock<IExecutableStepConfiguration>();
            _mockConfiguration.Setup(x => x.Retries).Returns(3);
            _mockStep = new Mock<ExecutableStepBase>(_mockJobContext.Object, _mockConfiguration.Object);
            _mockStep.CallBase = true;
        }

        [Fact]
        public async Task ExecuteAsync_1SuccessTest()
        {
            // Arrange
            _mockStep.Setup(x => x.ExecutePayloadAsync(CancellationToken.None)).Returns(Task.CompletedTask);

            // Act
            await _mockStep.Object.ExecuteAsync(CancellationToken.None);

            // Assert
            _mockStep.Verify(x => x.ExecutePayloadAsync(It.IsAny<CancellationToken>()), Times.Once());
            _mockJobContext.Verify(x => x.StepStarted(_mockConfiguration.Object), Times.Once());
            _mockJobContext.Verify(x => x.StepCompleted(_mockConfiguration.Object), Times.Once());
            _mockJobContext.Verify(x => x.StepFailed(_mockConfiguration.Object, It.IsAny<Exception>()), Times.Never());
        }

        [Fact]
        public async Task ExecuteAsync_UnsuccessfulRetriesTest()
        {
            // Arrange
            _mockStep.Setup(x => x.ExecutePayloadAsync(CancellationToken.None)).Throws<Exception>();

            // Act
            await _mockStep.Object.ExecuteAsync(CancellationToken.None);

            // Assert
            _mockStep.Verify(x => x.ExecutePayloadAsync(It.IsAny<CancellationToken>()), Times.Exactly(4));
            _mockJobContext.Verify(x => x.StepStarted(_mockConfiguration.Object), Times.Once());
            _mockJobContext.Verify(x => x.StepCompleted(_mockConfiguration.Object), Times.Never());
            _mockJobContext.Verify(x => x.StepFailed(_mockConfiguration.Object, It.IsAny<Exception>()), Times.Exactly(4));
        }

        [Fact]
        public async Task ExecuteAsync_SuccessfulRetriesTest()
        {
            int turn = 0;
            // Arrange
            _mockStep.Setup(x => x.ExecutePayloadAsync(CancellationToken.None)).Callback((CancellationToken token) => 
            {
                if (turn++ < 2)
                    throw new Exception();
            });

            // Act
            await _mockStep.Object.ExecuteAsync(CancellationToken.None);

            // Assert
            _mockStep.Verify(x => x.ExecutePayloadAsync(It.IsAny<CancellationToken>()), Times.Exactly(3));
            _mockJobContext.Verify(x => x.StepStarted(_mockConfiguration.Object), Times.Once());
            _mockJobContext.Verify(x => x.StepCompleted(_mockConfiguration.Object), Times.Once());
            _mockJobContext.Verify(x => x.StepFailed(_mockConfiguration.Object, It.IsAny<Exception>()), Times.Exactly(2));
        }

        [Fact]
        public async Task ExecuteAsync_BreakOnErrorTest()
        {
            // Arrange
            _mockStep.Setup(x => x.ExecutePayloadAsync(CancellationToken.None)).Throws<Exception>();
            _mockConfiguration.Setup(x => x.BreakOnError).Returns(true);

            // Act
            await Assert.ThrowsAnyAsync<Exception>(async () => await _mockStep.Object.ExecuteAsync(CancellationToken.None));

            // Assert
            _mockStep.Verify(x => x.ExecutePayloadAsync(It.IsAny<CancellationToken>()), Times.Once());
            _mockJobContext.Verify(x => x.StepStarted(_mockConfiguration.Object), Times.Once());
            _mockJobContext.Verify(x => x.StepCompleted(_mockConfiguration.Object), Times.Never());
            _mockJobContext.Verify(x => x.StepFailed(_mockConfiguration.Object, It.IsAny<Exception>()), Times.Once());
        }
    }
}