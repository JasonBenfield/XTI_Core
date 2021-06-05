using NUnit.Framework;
using System;
using System.IO;
using System.Threading.Tasks;

namespace XTI_Processes.Tests
{
    public sealed class WinProcessTest
    {
        [Test]
        public async Task ShouldRunProcess()
        {
            var textFileName = getTextFileName();
            tryDeleteTextFile(textFileName);
            var batFileName = getBatFileName();
            var batFileContents = $@"
(
  echo Test
) > {textFileName}
";
            writeBatFile(batFileName, batFileContents);
            var process = new WinProcess(batFileName);
            await process.Run();
            Assert.That
            (
                File.Exists(textFileName),
                Is.True,
                $"Should run '{batFileName}' to create file '{textFileName}'"
            );
            tryDeleteTextFile(textFileName);
        }

        [Test]
        public async Task ShouldGetExitCodeFromProcess()
        {
            var batFileName = getBatFileName();
            const int exitCode = 999;
            var batFileContents = $@"
exit {exitCode}
";
            writeBatFile(batFileName, batFileContents);
            var process = new WinProcess(batFileName);
            var result = await process.Run();
            Assert.That
            (
                result.ExitCode,
                Is.EqualTo(exitCode),
                $"Should get exit code {exitCode} after running '{batFileName}'"
            );
        }

        [Test]
        public async Task ShouldGetOutputFromProcess()
        {
            var batFileName = getBatFileName();
            const string output = "Test";
            var batFileContents = $@"
echo {output}
";
            writeBatFile(batFileName, batFileContents);
            var process = new WinProcess(batFileName);
            var result = await process.Run();
            Assert.That(result.OutputLines.Length, Is.EqualTo(2), "Should output 2 lines");
            Assert.That
            (
                result.OutputLines[1],
                Is.EqualTo(output),
                $"Should get output '{output}' after running '{batFileName}'"
            );
        }

        [Test]
        public async Task ShouldGetErrorFromProcess()
        {
            var batFileName = getBatFileName();
            const string error = "Test Error";
            var batFileContents = $@"
ECHO {error} 1>&2
";
            writeBatFile(batFileName, batFileContents);
            var process = new WinProcess(batFileName);
            var result = await process.Run();
            Assert.That(result.ErrorLines.Length, Is.EqualTo(1), "Should output 2 error lines");
            Assert.That
            (
                result.ErrorLines[0],
                Is.EqualTo(error),
                $"Should get error '{error}' after running '{batFileName}'"
            );
        }

        [Test]
        public async Task ShouldPassArgumentToProcess()
        {
            var batFileName = getBatFileName();
            const int arg = 123;
            var batFileContents = $@"
exit %1
";
            writeBatFile(batFileName, batFileContents);
            var process = new WinProcess(batFileName);
            process.AddArgument(arg.ToString());
            var result = await process.Run();
            Assert.That
            (
                result.ExitCode,
                Is.EqualTo(arg),
                $"Should pass argument {arg} to '{batFileName}"
            );
        }

        [Test]
        public async Task ShouldPassNamedArgumentToProcess()
        {
            var batFileName = getBatFileName();
            const string argName = "arg1";
            const string argValue = "value1";
            var batFileContents = $@"
echo %1 %2
";
            writeBatFile(batFileName, batFileContents);
            var process = new WinProcess(batFileName);
            process
                .UseArgumentNameDelimiter("-")
                .UseArgumentValueDelimiter(" ")
                .AddArgument(argName, argValue);
            var result = await process.Run();
            var outputLines = result.OutputLines;
            Assert.That
            (
                outputLines[1],
                Is.EqualTo($"-{argName} {argValue}"),
                $"Should pass argument {argName} to '{batFileName}"
            );
        }

        [Test]
        public async Task ShouldPassMultipleArgumentsToProcess()
        {
            var batFileName = getBatFileName();
            const string arg1 = "arg1";
            const string arg2Name = "arg2";
            const string arg2Value = "value2";
            var batFileContents = $@"
echo off
echo %1
echo %2 %3
";
            writeBatFile(batFileName, batFileContents);
            var process = new WinProcess(batFileName);
            process
                .UseArgumentNameDelimiter("")
                .AddArgument(arg1)
                .UseArgumentNameDelimiter("-")
                .UseArgumentValueDelimiter(" ")
                .AddArgument(arg2Name, arg2Value);
            var result = await process.Run();
            var outputLines = result.OutputLines;
            Assert.That
            (
                outputLines[1],
                Is.EqualTo(arg1),
                $"Should pass argument {arg1} to '{batFileName}"
            );
            Assert.That
            (
                outputLines[2],
                Is.EqualTo($"-{arg2Name} {arg2Value}"),
                $"Should pass argument {arg2Name} to '{batFileName}"
            );
        }

        private static string getTextFileName()
            => Path.Combine(TestContext.CurrentContext.WorkDirectory, "test.txt");

        private static void tryDeleteTextFile(string textFileName)
        {
            if (File.Exists(textFileName))
            {
                File.Delete(textFileName);
            }
        }

        private static string getBatFileName()
            => Path.Combine(TestContext.CurrentContext.WorkDirectory, "test.bat");

        private static void writeBatFile(string batFileName, string batFileContents)
        {
            using var writer = new StreamWriter(batFileName, false);
            writer.Write(batFileContents);
        }

    }
}