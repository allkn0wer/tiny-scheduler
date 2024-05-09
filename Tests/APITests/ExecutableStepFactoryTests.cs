using Moq;
using SK.TinyScheduler.API;

namespace APITests
{
    public class ExecutableStepFactoryTests
    {
        private Mock<IJobContext> _mockJobContext;
        private Mock<IExecutableStepConfiguration> _mockConfiguration;

        public ExecutableStepFactoryTests() 
        {
            _mockJobContext = new Mock<IJobContext>();
            _mockConfiguration = new Mock<IExecutableStepConfiguration>();
        }

        [Fact]
        public void Create_Test()
        {
            // Arrange
            _mockConfiguration.Setup(x => x.Type).Returns("test");

            // Act
            var instance = ExecutableStepFactory.Create(_mockJobContext.Object, _mockConfiguration.Object);

            // Assert
            Assert.IsType<TestExecutableStep>(instance);
        }

        [Fact]
        public void CreateUnknown_Test()
        {
            // Arrange
            _mockConfiguration.Setup(x => x.Type).Returns("unknown");

            // Act + Assert
            Assert.Throws<NotImplementedException>(() => ExecutableStepFactory.Create(_mockJobContext.Object, _mockConfiguration.Object));
        }
    }

    [ExecutableStepType("test")]
    public class TestExecutableStep : ExecutableStepBase
    {
        public TestExecutableStep(IJobContext context, IExecutableStepConfiguration configuration) : base(context, configuration) { }

        protected internal override Task ExecutePayloadAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
