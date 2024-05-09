using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK.TinyScheduler.API
{
    /// <summary>
    /// Represents an executable operation that can be executed asynchronously.
    /// </summary>
    public interface IExecutable
    {
        /// <summary>
        /// Executes the operation asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}
