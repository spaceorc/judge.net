using System.IO;
using System.Threading;
using Judge.LimitedRunner;
using Judge.RunnerInterface;
using NUnit.Framework;

namespace Judge.Tests.LimitedRunnerTests
{
    [TestFixture]
    public class LimitedRunnerServiceTests
    {
        private readonly string _workingDirectory = Path.Combine(TestContext.CurrentContext.TestDirectory, "WorkingDirectory");

        [SetUp]
        public void SetUp()
        {
            if (!Directory.Exists(_workingDirectory))
                Directory.CreateDirectory(_workingDirectory);

            using (CreateFile("input.txt"))
            {
            }
        }

        [Test]
        public void RunCmdTest()
        {
            var service = new LimitedRunnerService();

            var configuration = new Configuration("cmd", _workingDirectory, 1000, 10 * 1024 * 1024)
            {
                InputFile = "input.txt",
                OutputFile = "output.txt"
            };

            service.Run(configuration);
        }

        [Test]
        public void TimeLimitSolutionTest()
        {
            var service = new LimitedRunnerService();

            var fileName = Path.Combine(TestContext.CurrentContext.TestDirectory, @"TestSolutions\TL.exe");
            var configuration = new Configuration(fileName, _workingDirectory, 100, 10 * 1024 * 1024)
            {
                InputFile = "input.txt",
                OutputFile = "output.txt"
            };

            var result = service.Run(configuration);

            Assert.That(result.RunStatus, Is.EqualTo(RunStatus.TimeLimitExceeded));
        }

        [Test]
        public void IdleSolutionTest()
        {
            var service = new LimitedRunnerService();

            var fileName = Path.Combine(TestContext.CurrentContext.TestDirectory, @"TestSolutions\IdleTest.exe");
            var configuration = new Configuration(fileName, _workingDirectory, 500, 10 * 1024 * 1024)
            {
                InputFile = "input.txt",
                OutputFile = "output.txt"
            };

            var result = service.Run(configuration);

            Assert.That(result.RunStatus, Is.EqualTo(RunStatus.IdlenessLimitExceeded));
        }

        [Test]
        public void MemoryLimitTest()
        {
            var service = new LimitedRunnerService();

            var fileName = Path.Combine(TestContext.CurrentContext.TestDirectory, @"TestSolutions\TL.exe");

            var configuration = new Configuration(fileName, _workingDirectory, 1000, 10)
            {
                InputFile = "input.txt",
                OutputFile = "output.txt"
            };

            var result = service.Run(configuration);

            Assert.That(result.RunStatus, Is.EqualTo(RunStatus.MemoryLimitExceeded));
        }

        [Test]
        public void InvalidCodeSolutionTest()
        {
            var service = new LimitedRunnerService();

            var fileName = Path.Combine(TestContext.CurrentContext.TestDirectory, @"TestSolutions\InvalidReturnCode.exe");
            var configuration = new Configuration(fileName, _workingDirectory, 1000, 10 * 1024 * 1024)
            {
                InputFile = "input.txt",
                OutputFile = "output.txt"
            };

            var result = service.Run(configuration);

            Assert.That(result.RunStatus, Is.EqualTo(RunStatus.RuntimeError));
        }

        [Test]
        public void RuntimeErrorSolutionTest()
        {
            var service = new LimitedRunnerService();

            var fileName = Path.Combine(TestContext.CurrentContext.TestDirectory, @"TestSolutions\RuntimeError.exe");
            var configuration = new Configuration(fileName, _workingDirectory, 1000, 10 * 1024 * 1024)
            {
                InputFile = "input.txt",
                OutputFile = "output.txt"
            };

            var result = service.Run(configuration);

            Assert.That(result.RunStatus, Is.EqualTo(RunStatus.RuntimeError));
        }

        [Test]
        public void UseFilesTest()
        {
            using (var input = CreateFile("input.txt"))
            {
                input.Write("1 2");
            }

            var service = new LimitedRunnerService();

            var fileName = Path.Combine(TestContext.CurrentContext.TestDirectory, @"TestSolutions\AB.exe");
            var configuration = new Configuration(fileName, _workingDirectory, 1000, 10 * 1024 * 1024)
            {
                InputFile = Path.Combine(_workingDirectory, "input.txt"),
                OutputFile = Path.Combine(_workingDirectory, "output.txt")
            };

            var result = service.Run(configuration);

            Assert.That(result.RunStatus, Is.EqualTo(RunStatus.Success));
        }

        [TearDown]
        public void TearDown()
        {
            DeleteFile(Path.Combine(_workingDirectory, "input.txt"));
            DeleteFile(Path.Combine(_workingDirectory, "output.txt"));
        }

        private StreamWriter CreateFile(string fileName)
        {
            fileName = Path.Combine(_workingDirectory, fileName);
            DeleteFile(fileName);

            return new StreamWriter(fileName);
        }

        private static void DeleteFile(string fileName)
        {
            if (File.Exists(fileName))
            {
                while (true)
                {
                    try
                    {
                        File.Delete(fileName);
                        break;
                    }
                    catch (IOException)
                    {
                        Thread.Sleep(50);
                    }
                }
            }
        }
    }
}
