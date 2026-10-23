using System.Threading;
using System.Threading.Tasks;

namespace VaultSharp.Core
{
#if !NET6_0_OR_GREATER
    internal static class TaskWaitExtensions
    {
        public static async Task<TResult> WaitAsync<TResult>(this Task<TResult> task, CancellationToken cancellationToken)
        {
            if (!cancellationToken.CanBeCanceled || task.IsCompleted)
            {
                return await task.ConfigureAwait(false);
            }

            var cancellation = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

            using (cancellationToken.Register(() => cancellation.TrySetResult(true)))
            {
                if (await Task.WhenAny(task, cancellation.Task).ConfigureAwait(false) != task)
                {
                    return await Task.FromCanceled<TResult>(cancellationToken).ConfigureAwait(false);
                }
            }

            return await task.ConfigureAwait(false);
        }
    }
#endif
}
