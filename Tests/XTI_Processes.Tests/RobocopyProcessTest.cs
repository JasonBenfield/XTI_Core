using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTI_Processes.Tests
{
    public sealed class RobocopyProcessTest
    {
        [Test]
        public async Task ShouldCopyFromSourceToTarget()
        {
            var sourceDir = Path.Combine(TestContext.CurrentContext.WorkDirectory, "Source1");
            var targetDir = Path.Combine(TestContext.CurrentContext.WorkDirectory, "Target1");
            tryDeleteSourceAndTarget(sourceDir, targetDir);
            Directory.CreateDirectory(sourceDir);
            foreach (var i in Enumerable.Range(1, 5))
            {
                var fileName = Path.Combine(sourceDir, $"file{i}.txt");
                using var writer = new StreamWriter(fileName);
                writer.WriteLine($"Test {i}");
            }
            var process = new RobocopyProcess(sourceDir, targetDir);
            await process.Run();
            var sourceFiles = Directory.GetFiles(sourceDir).Select(f => Path.GetFileName(f));
            var targetFiles = Directory.Exists(targetDir)
                ? Directory.GetFiles(targetDir).Select(f => Path.GetFileName(f))
                : new string[] { };
            Assert.That(targetFiles, Is.EquivalentTo(sourceFiles), "Should copy source to target");
        }

        [Test]
        public async Task ShouldCopyFromSourceToTargetByPattern()
        {
            var sourceDir = Path.Combine(TestContext.CurrentContext.WorkDirectory, "Source2");
            var targetDir = Path.Combine(TestContext.CurrentContext.WorkDirectory, "Target2");
            tryDeleteSourceAndTarget(sourceDir, targetDir);
            Directory.CreateDirectory(sourceDir);
            foreach (var i in Enumerable.Range(1, 5))
            {
                var fileName = Path.Combine(sourceDir, $"file{i}.txt");
                using var writer = new StreamWriter(fileName);
                writer.WriteLine($"Test {i}");
            }
            foreach (var i in Enumerable.Range(6, 5))
            {
                var fileName = Path.Combine(sourceDir, $"file{i}.dat");
                using var writer = new StreamWriter(fileName);
                writer.WriteLine($"Test {i}");
            }
            var process = new RobocopyProcess(sourceDir, targetDir);
            process.Pattern("*.dat");
            await process.Run();
            var sourceFiles = Directory.GetFiles(sourceDir, "*.dat").Select(f => Path.GetFileName(f));
            var targetFiles = Directory.Exists(targetDir)
                ? Directory.GetFiles(targetDir).Select(f => Path.GetFileName(f))
                : new string[] { };
            Assert.That(targetFiles, Is.EquivalentTo(sourceFiles), "Should copy source to target by pattern");
        }

        [Test]
        public async Task ShouldPurgeFilesFromTargetThatNoLongerExistInSource()
        {
            var sourceDir = Path.Combine(TestContext.CurrentContext.WorkDirectory, "Source3");
            var targetDir = Path.Combine(TestContext.CurrentContext.WorkDirectory, "Target3");
            tryDeleteSourceAndTarget(sourceDir, targetDir);
            Directory.CreateDirectory(sourceDir);
            foreach (var i in Enumerable.Range(1, 5))
            {
                var fileName = Path.Combine(sourceDir, $"file{i}.txt");
                using var writer = new StreamWriter(fileName);
                writer.WriteLine($"Test {i}");
            }
            var process = new RobocopyProcess(sourceDir, targetDir);
            process.Purge();
            await process.Run();
            var sourceFiles = Directory.GetFiles(sourceDir);
            File.Delete(sourceFiles[0]);
            await process.Run();
            sourceFiles = Directory.GetFiles(sourceDir).Select(f => Path.GetFileName(f)).ToArray();
            var targetFiles = Directory.Exists(targetDir)
                ? Directory.GetFiles(targetDir).Select(f => Path.GetFileName(f))
                : new string[] { };
            Assert.That(targetFiles, Is.EquivalentTo(sourceFiles), "Should purge files from target that no longer exist in source");
        }

        [Test]
        public async Task ShouldMakeTargetFilesReadonly()
        {
            var sourceDir = Path.Combine(TestContext.CurrentContext.WorkDirectory, "Source4");
            var targetDir = Path.Combine(TestContext.CurrentContext.WorkDirectory, "Target4");
            tryDeleteSourceAndTarget(sourceDir, targetDir);
            Directory.CreateDirectory(sourceDir);
            foreach (var i in Enumerable.Range(1, 5))
            {
                var fileName = Path.Combine(sourceDir, $"file{i}.txt");
                using var writer = new StreamWriter(fileName);
                writer.WriteLine($"Test {i}");
            }
            var process = new RobocopyProcess(sourceDir, targetDir);
            process.AddReadonlyAttributeToTarget();
            await process.Run();
            var targetFiles = Directory.Exists(targetDir)
                ? Directory.GetFiles(targetDir)
                : new string[] { };
            Assert.That(targetFiles.Length, Is.EqualTo(5), "Should copy source files to target");
            Assert.That
            (
                targetFiles.Select(f => new FileInfo(f).IsReadOnly),
                Has.All.True,
                "Should add readonly attribute to files in the target directory"
            );
        }

        [Test]
        public async Task ShouldMoveFilesFromSourceToTarget()
        {
            var sourceDir = Path.Combine(TestContext.CurrentContext.WorkDirectory, "Source5");
            var targetDir = Path.Combine(TestContext.CurrentContext.WorkDirectory, "Target5");
            tryDeleteSourceAndTarget(sourceDir, targetDir);
            Directory.CreateDirectory(sourceDir);
            foreach (var i in Enumerable.Range(1, 5))
            {
                var fileName = Path.Combine(sourceDir, $"file{i}.txt");
                using var writer = new StreamWriter(fileName);
                writer.WriteLine($"Test {i}");
            }
            var originalSourceFiles = Directory.GetFiles(sourceDir).Select(f => Path.GetFileName(f));
            var process = new RobocopyProcess(sourceDir, targetDir);
            process.MoveFiles();
            await process.Run();
            var sourceFiles = Directory.GetFiles(sourceDir);
            Assert.That(sourceFiles.Length, Is.EqualTo(0), "Should remove files from source");
            var targetFiles = Directory.Exists(targetDir)
                ? Directory.GetFiles(targetDir).Select(f => Path.GetFileName(f))
                : new string[] { };
            Assert.That(targetFiles, Is.EquivalentTo(originalSourceFiles), "Should move files from source to target");
        }

        private static void tryDeleteSourceAndTarget(string sourceDir, string targetDir)
        {
            if (Directory.Exists(sourceDir))
            {
                Directory.Delete(sourceDir, true);
            }
            if (Directory.Exists(targetDir))
            {
                Directory.Delete(targetDir, true);
            }
        }
    }
}
