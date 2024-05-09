using Moq;
using SK.TinyScheduler.API;
using SK.TinyScheduler.Core;
using SK.TinyScheduler.Database.Entities;

namespace CoreTests
{
    public class ExecutableJobInstanceTest
    {
        Mock<JobInstance> _mockJobInstance;
        Mock<Action<UpdateStateArgs>> _mockUpdateAction;

        public ExecutableJobInstanceTest() 
        {
            _mockJobInstance = new Mock<JobInstance>();
            _mockUpdateAction = new Mock<Action<UpdateStateArgs>>();
        }

        [Fact]
        public void CreateInstance_Test()
        {
            _mockJobInstance.Setup(x => x.Steps).Returns(new[]
            {
                new StepInstance
                {
                    Id = 1,
                    Order = 2,
                    Type = "sequential",
                    Steps = new[]
                    {
                        new StepInstance { Id = 3, Order = 3, Type = "sql", ParentStepId = 1 },
                        new StepInstance { Id = 4, Order = 1, Type = "sql", ParentStepId = 1 },
                        new StepInstance { Id = 5, Order = 2, Type = "sql", ParentStepId = 1 }
                    }
                },
                new StepInstance { Id = 2, Order = 1, Type = "cmd", },
                new StepInstance { Id = 3, Order = 3, Type = "sql", ParentStepId = 1 },
                new StepInstance { Id = 4, Order = 1, Type = "sql", ParentStepId = 1 },
                new StepInstance { Id = 5, Order = 2, Type = "sql", ParentStepId = 1 }
            });

            var instance = new ExecutableJobInstance(_mockJobInstance.Object, _mockUpdateAction.Object);

            Assert.NotNull(instance);
            Assert.Equal(2, instance.Steps.Count);
        }

        [Fact]
        public async Task ExecuteAsync_Test()
        {
            var mockStep = new Mock<IExecutable>();
            mockStep.Setup(x => x.ExecuteAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var mockExecutableJobInstance = new Mock<ExecutableJobInstance>(_mockJobInstance.Object, _mockUpdateAction.Object);
            mockExecutableJobInstance.Object.Steps = new List<IExecutable>{ mockStep.Object, mockStep.Object };
            mockExecutableJobInstance.CallBase = true;

            await mockExecutableJobInstance.Object.ExecuteAsync(CancellationToken.None);

            _mockUpdateAction.Verify(x => x(It.IsAny<UpdateStateArgs.UpdateJobStateArgs>()), Times.Exactly(2));
            mockStep.Verify(x => x.ExecuteAsync(It.IsAny<CancellationToken>()), Times.Exactly(2));
        }
    }
}