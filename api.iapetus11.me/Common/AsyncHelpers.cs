using System.Runtime.ExceptionServices;

namespace api.iapetus11.me.Common;

public static class AsyncHelpers
{
    public static async Task<T?> FirstNotNull<T>(params Task<T?>[] tasks)
    {
        var remainingTasks = tasks.ToList();
        var exceptions = new List<Exception>();

        while (remainingTasks.Any())
        {
            using var task = await Task.WhenAny(remainingTasks);
            remainingTasks.Remove(task);

            try
            {
                var result = await task;
                if (result is not null) return result;
            }
            catch (Exception e)
            {
                exceptions.Add(e);
            }
        }

        if (exceptions.Count == 1)
            ExceptionDispatchInfo.Capture(exceptions[0]).Throw();
        else if (exceptions.Count > 1)
            throw new AggregateException(exceptions);

        return default;
    }
}