using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FilesValidator.BL;
using Moq;
using FilesValidator.Model;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace FilesValidator.Tests.BL
{
    [TestClass]
    public class FileItemCheckerTests
    {
        public TestContext TestContext { get; set; }
        [TestMethod]
        public void ConstructorTest()
        {
            var target = new FileItemChecker(TestContext.TestDeploymentDir);
            Assert.IsNotNull(target);
        }

        [TestMethod]
        public void CheckTest()
        {
            var mockFileItem = new Mock<IFileItem>();
            mockFileItem.Setup(m => m.DestinationFolder).Returns("Hello");
            var target = new FileItemChecker(TestContext.TestDeploymentDir);
            var actual = target.Check(mockFileItem.Object);
            var exptectedMessage = String.Format(
                "Destination folder \"{0}\" not found",
                Path.Combine(TestContext.TestDeploymentDir, "Hello"));
            Assert.AreEqual(exptectedMessage, actual.CheckResult);
        }

        [TestMethod]
        public void CheckTest2()
        {
            var destinationDir = Path.Combine(
                TestContext.TestDeploymentDir, "Hello");
            Directory.CreateDirectory(destinationDir);
            var mockFileItem = new Mock<IFileItem>();
            mockFileItem.Setup(m => m.DestinationFolder).Returns("Hello");
            mockFileItem.Setup(m => m.ResultFile).Returns("Peace.txt");
            var target = new FileItemChecker(TestContext.TestDeploymentDir);
            var actual = target.Check(mockFileItem.Object);
            var exptectedMessage = String.Format(
                "Result file \"{0}\" not found",
                Path.Combine(TestContext.TestDeploymentDir, "Hello", "Peace.txt"));
            Assert.AreEqual(exptectedMessage, actual.CheckResult);
            Directory.Delete(destinationDir);
        }

        [TestMethod]
        public void CheckTest3()
        {
            var destinationDir = Path.Combine(
               TestContext.TestDeploymentDir, "Hello");
            Directory.CreateDirectory(destinationDir);
            var resultFile = Path.Combine(destinationDir, "Peace.txt");
            var fs = File.Create(resultFile);
            fs.Close();
            fs.Dispose();
            var mockFileItem = new Mock<IFileItem>();
            mockFileItem.Setup(m => m.DestinationFolder).Returns("Hello");
            mockFileItem.Setup(m => m.ResultFile).Returns("Peace.txt");
            var target = new FileItemChecker(TestContext.TestDeploymentDir);
            var actual = target.Check(mockFileItem.Object);
            Assert.IsTrue(String.IsNullOrEmpty(actual.CheckResult));
            File.Delete(resultFile);
            Directory.Delete(destinationDir);
        }

        [TestMethod]
        public void CheckTest4()
        {
            var destinationDir = Path.Combine(
               TestContext.TestDeploymentDir, "Hello");
            Directory.CreateDirectory(destinationDir);
            var resultFile = Path.Combine(destinationDir, "Peace.txt");
            var fs = File.Create(resultFile);
            var md5Bytes = MD5.Create().ComputeHash(fs);
            fs.Close();
            fs.Dispose();
            var md5 = new StringBuilder();
            for (int i = 0; i < md5Bytes.Length; i++)
                md5.Append(md5Bytes[i].ToString("x2"));
            File.AppendAllText(resultFile, "hello");            
            var mockFileItem = new Mock<IFileItem>();
            mockFileItem.Setup(m => m.DestinationFolder).Returns("Hello");
            mockFileItem.Setup(m => m.ResultFile).Returns("Peace.txt");
            mockFileItem.Setup(m => m.ResultFileMD5).Returns(md5.ToString());
            var target = new FileItemChecker(TestContext.TestDeploymentDir);
            var actual = target.Check(mockFileItem.Object);
            Assert.AreEqual("Hash is wrong", actual.CheckResult);
            File.Delete(resultFile);
            Directory.Delete(destinationDir);
        }

        [TestMethod]
        public void CheckTest5()
        {
            var destinationDir = Path.Combine(
               TestContext.TestDeploymentDir, "Hello");
            Directory.CreateDirectory(destinationDir);
            var resultFile = Path.Combine(destinationDir, "Peace.txt");
            var fs = File.Create(resultFile);
            var md5Bytes = MD5.Create().ComputeHash(fs);
            fs.Close();
            fs.Dispose();
            var md5 = new StringBuilder();
            for (int i = 0; i < md5Bytes.Length; ++i)
                md5.Append(md5Bytes[i].ToString("x2"));
            var mockFileItem = new Mock<IFileItem>();
            mockFileItem.Setup(m => m.DestinationFolder).Returns("Hello");
            mockFileItem.Setup(m => m.ResultFile).Returns("Peace.txt");
            mockFileItem.Setup(m => m.ResultFileMD5).Returns(md5.ToString());
            var target = new FileItemChecker(TestContext.TestDeploymentDir);
            var actual = target.Check(mockFileItem.Object);
            Assert.IsTrue(String.IsNullOrEmpty(actual.CheckResult));
            File.Delete(resultFile);
            Directory.Delete(destinationDir);
        }

        [TestMethod]
        public void CheckTest6()
        {
            var destinationDir = Path.Combine(
               TestContext.TestDeploymentDir, "Hello");
            Directory.CreateDirectory(destinationDir);
            var resultFile = Path.Combine(destinationDir, "Peace.txt");
            var fs = File.Create(resultFile);
            var md5Bytes = MD5.Create().ComputeHash(fs);
            var md5 = new StringBuilder();
            for (int i = 0; i < md5Bytes.Length; ++i)
                md5.Append(md5Bytes[i].ToString("x2"));
            var mockFileItem = new Mock<IFileItem>();
            mockFileItem.Setup(m => m.DestinationFolder).Returns("Hello");
            mockFileItem.Setup(m => m.ResultFile).Returns("Peace.txt");
            mockFileItem.Setup(m => m.ResultFileMD5).Returns(md5.ToString());
            var target = new FileItemChecker(TestContext.TestDeploymentDir);
            var actual = target.Check(mockFileItem.Object);
            var expected = String.Format(
                "The process cannot access the file '{0}' because it is being used by another process.",
                resultFile);
            Assert.AreEqual(expected, actual.CheckResult);
            fs.Close();
            fs.Dispose();
            File.Delete(resultFile);
            Directory.Delete(destinationDir);
        }
    }
}
