using SK.TinyScheduler.API;
using System.Diagnostics;

namespace SK.TinyScheduler.Core
{

    [ExecutableStepType("cmd")]
    public class CommandStep : ExecutableStepBase
    {
        private const string PROCESS_FILENAME = "cmd.exe";

        public CommandStep(IJobContext context, IExecutableStepConfiguration stepInstance) : base(context, stepInstance) { }

        protected override async Task ExecutePayloadAsync(CancellationToken cancellationToken)
        {
            using (Process process = new Process())
            {
                process.StartInfo.FileName = PROCESS_FILENAME;
                process.StartInfo.Arguments = $"/c {_configuration.Script}";
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;

                process.Start();
                await process.WaitForExitAsync();

                int exitCode = process.ExitCode;
                if (exitCode != 0) 
                    throw new Exception($"The process ended with a failure code \"{exitCode}\"");
            }
        }
    }

    [ExecutableStepType("sql")]
    public class SQLCommandStep : ExecutableStepBase
    {
        public SQLCommandStep(IJobContext context, IExecutableStepConfiguration stepInstance) : base(context, stepInstance) { }

        protected override Task ExecutePayloadAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
