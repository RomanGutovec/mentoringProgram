using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace FileSystemVisitorLib.Tests
{
    [TestFixture]
    public class FileSystemVisitorTests
    {
        private readonly string testFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Test");

        [SetUp]
        public void Setup()
        {
            Directory.CreateDirectory(testFolderPath);
            string[] testDirs = {
                MakePath(testFolderPath, "Test"),
                MakePath(testFolderPath, "Test","dir1"),
                MakePath(testFolderPath, "Test","dir1", "dir2"), 
                MakePath(testFolderPath, "Test","dir1", "dir2", "dir3")
            };

            foreach (string dir in testDirs)
            {
                Directory.CreateDirectory(dir);
            }

            string[] testFiles = {
                MakePath(testFolderPath, "Test", "dir1",
                    "file1.txt"),
                MakePath(testFolderPath, "Test", "dir1",
                    "file2.txt"),
                MakePath(testFolderPath, "Test", "dir1",
                    "dir2", "file3.txt"),
                MakePath(testFolderPath, "Test", "dir1",
                    "dir2", "file4.txt") };

            foreach (string file in testFiles)
            {
                FileStream str = File.Create(file);
                str.Close();
            }
        }

        [TearDown]
        public void TearDown()
        {
            Directory.Delete(testFolderPath, true);
        }

        [Test]
        public void FilesystemVisitorTest_RootPathNull_ThrowsNullArgumentException()
            => Assert.Throws<ArgumentNullException>(() => new FileSystemVisitor(null));

        [Test]
        public void FilesystemVisitorTest_RootDirectoryInfoNull_ThrowsNullArgumentException()
            => Assert.Throws<ArgumentNullException>(() => new FileSystemVisitor(new System.IO.DirectoryInfo(null)));

        [Test]
        public void FilesystemVisitorTest_RootDirectoryNotNullAndFilterNull_ThrowsNullArgumentException()
            => Assert.Throws<ArgumentNullException>(() => new FileSystemVisitor(new System.IO.DirectoryInfo("somePath"), null));

        [TestCase("dir1", 7)]
        [TestCase("dir2", 4)]
        [TestCase("Test", 8)]
        [TestCase("file4.txt", 1)]
        [TestCase("file1", 1)]
        [TestCase("file5", 0)]
        public void FilesystemVisitorTest_IterateAllElements_CheckAmountOfElementsWithoutFilter(string subString, int amount)
        {
            var actualResult = new List<string>();
            var fileSystemVisitor = new FileSystemVisitor(new DirectoryInfo(testFolderPath),
                s => s.Contains(subString));
            foreach (var element in fileSystemVisitor)
            {
                actualResult.Add(element);
            }

            Assert.AreEqual(amount, actualResult.Count);
        }

        [Test]
        public void FilesystemVisitorTest_IterateAllElements_CheckAmountOfElementsWithFilter()
        {
            var expectedResult = new List<string>();
            var actualResult = new List<string>();
            var fileSystemVisitor = new FileSystemVisitor(new DirectoryInfo(testFolderPath));
            foreach (var element in fileSystemVisitor)
            {
                actualResult.Add(element);
            }

            Assert.AreEqual(8, actualResult.Count);
        }

        [Test]
        public void FilesystemVisitorTest_IterateAllElements_StartEventCalled()
        {
            var isCalled = false;
            var fileSystemVisitor = new FileSystemVisitor(new DirectoryInfo(testFolderPath));

            fileSystemVisitor.Start += (sender, args) => { isCalled = true; };
            foreach (var element in fileSystemVisitor)
            {
            }

            Assert.IsTrue(isCalled);
        }

        [Test]
        public void FilesystemVisitorTest_BreakIterationBeforeEnd_FinishEventNotCalled()
        {
            var isCalled = false;
            var fileSystemVisitor = new FileSystemVisitor(new DirectoryInfo(testFolderPath));

            fileSystemVisitor.Finish += (sender, args) => { isCalled = true; };
            foreach (var element in fileSystemVisitor)
            {
                if (element == Path.Combine(testFolderPath, "Test"))
                {
                    break;
                }
            }

            Assert.IsFalse(isCalled);
        }

        [Test]
        public void FilesystemVisitorTest_IterateAllElements_FinishEventCalled()
        {
            var isCalled = false;
            var fileSystemVisitor = new FileSystemVisitor(new DirectoryInfo(testFolderPath));

            fileSystemVisitor.Finish += (sender, args) => { isCalled = true; };
            foreach (var element in fileSystemVisitor)
            {
            }

            Assert.IsTrue(isCalled);
        }

        [Test]
        public void FilesystemVisitorTest_IterateAllElements_FileFoundCalled()
        {
            var isCalled = false;
            var fileSystemVisitor = new FileSystemVisitor(new DirectoryInfo(testFolderPath));

            fileSystemVisitor.FileFound += (sender, args) => { isCalled = true; };
            foreach (var element in fileSystemVisitor)
            {
            }

            Assert.IsTrue(isCalled);
        }

        [Test]
        public void FilesystemVisitorTest_IterateAllElements_FileFoundCalledAndExclude()
        {
            var isCalled = false;
            var fileSystemVisitor = new FileSystemVisitor(new DirectoryInfo(testFolderPath),
                s => s.Contains("file4.txt"));
            var actualResult = new List<string>();

            fileSystemVisitor.FileFound += (sender, args) => { isCalled = true; args.Exclude = true; };
            foreach (var element in fileSystemVisitor)
            {
                actualResult.Add(element);
            }

            Assert.IsTrue(isCalled);
            Assert.AreEqual(0, actualResult.Count);
        }

        [Test]
        public void FilesystemVisitorTest_IterateAllElements_FileFoundCalledAndStop()
        {
            var isCalled = false;
            var fileSystemVisitor = new FileSystemVisitor(new DirectoryInfo(testFolderPath));
            var actualResult = new List<string>();

            fileSystemVisitor.FileFound += (sender, args) => { isCalled = true; args.Stop = true; };
            foreach (var element in fileSystemVisitor)
            {
                actualResult.Add(element);
            }

            Assert.IsTrue(isCalled);
            Assert.AreEqual(4, actualResult.Count);
        }

        [Test]
        public void FilesystemVisitorTest_IterateAllElements_DirectoryFoundCalled()
        {
            var isCalled = false;
            var fileSystemVisitor = new FileSystemVisitor(new DirectoryInfo(testFolderPath));

            fileSystemVisitor.DirectoryFound += (sender, args) => { isCalled = true; };
            foreach (var element in fileSystemVisitor)
            {
            }

            Assert.IsTrue(isCalled);
        }

        [Test]
        public void FilesystemVisitorTest_IterateAllElements_DirectoryFoundCalledAndExclude()
        {
            var isCalled = false;
            var fileSystemVisitor = new FileSystemVisitor(new DirectoryInfo(testFolderPath),
                s => s.Contains("Test"));
            var actualResult = new List<string>();

            fileSystemVisitor.DirectoryFound += (sender, args) => { isCalled = true; args.Exclude = true; };
            foreach (var element in fileSystemVisitor)
            {
                actualResult.Add(element);
            }

            Assert.IsTrue(isCalled);
            Assert.AreEqual(4, actualResult.Count);
        }


        [Test]
        public void FilesystemVisitorTest_IterateAllElements_DirectoryFoundCalledAndStop()
        {
            var isCalled = false;
            var fileSystemVisitor = new FileSystemVisitor(new DirectoryInfo(testFolderPath));
            var actualResult = new List<string>();

            fileSystemVisitor.FileFound += (sender, args) => { isCalled = true; args.Stop = true; };
            foreach (var element in fileSystemVisitor)
            {
                actualResult.Add(element);
            }

            Assert.IsTrue(isCalled);
            Assert.AreEqual(4, actualResult.Count);
        }

        [Test]
        public void FilesystemVisitorTest_IterateAllElements_DirectoryFilteredDirectoryFoundCalledAndStop()
        {
            var isCalled = false;
            var fileSystemVisitor = new FileSystemVisitor(new DirectoryInfo(testFolderPath),
            s => s.Contains("Test"));
            var actualResult = new List<string>();

            fileSystemVisitor.FilteredDirectoryFound += (sender, args) => { isCalled = true; args.Stop = true; };
            foreach (var element in fileSystemVisitor)
            {
                actualResult.Add(element);
            }

            Assert.IsTrue(isCalled);
            Assert.AreEqual(0, actualResult.Count);
        }

        [Test]
        public void FilesystemVisitorTest_IterateAllElements_FilteredFileFoundCalledAndStop()
        {
            var isCalled = false;
            var fileSystemVisitor = new FileSystemVisitor(new DirectoryInfo(testFolderPath),
                s => s.Contains("file4"));
            var actualResult = new List<string>();

            fileSystemVisitor.FilteredFileFound += (sender, args) => { isCalled = true; args.Stop = true; };
            foreach (var element in fileSystemVisitor)
            {
                actualResult.Add(element);
            }

            Assert.IsTrue(isCalled);
            Assert.AreEqual(0, actualResult.Count);
        }

        private string MakePath(params string[] tokens)
        {
            string fullpath = "";
            foreach (string token in tokens)
            {
                fullpath = Path.Combine(fullpath, token);
            }

            return fullpath;
        }
    }
}