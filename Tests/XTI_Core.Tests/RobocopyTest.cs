using NUnit.Framework;
using XTI_Processes;

namespace XTI_Core.Tests;

internal sealed class RobocopyTest
{
    [Test]
    public void ShouldBuildRobocopyCommand()
    {
        var robocopy = new RobocopyProcess("c:\\src", "c:\\tgt")
            .MultiThreaded(32)
            .NumberOfRetries(1)
            .WaitTimeBetweenRetries(1);
        var commandText = robocopy.CommandText();
        Console.WriteLine(commandText);
    }
}
