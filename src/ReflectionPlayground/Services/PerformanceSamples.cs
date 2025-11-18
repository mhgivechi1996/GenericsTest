using System.Diagnostics;
using System.Threading;
using ReflectionPlayground.Attributes;

namespace ReflectionPlayground.Services;

public class PerformanceSamples
{
    [LogExecution("Fast operation")]
    public void QuickTask()
    {
        SpinWait.SpinUntil(() => false, 50);
    }

    [LogExecution("I/O simulation")]
    public void SlowTask()
    {
        Thread.Sleep(150);
    }

    [LogExecution]
    public int SumRange(int max)
    {
        var total = 0;
        for (var i = 0; i <= max; i++)
        {
            total += i;
        }

        return total;
    }
}
