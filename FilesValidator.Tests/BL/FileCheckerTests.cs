using Microsoft.VisualStudio.TestTools.UnitTesting;
using FilesValidator.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilesValidator.Model;
using Moq;
using System.IO;

namespace FilesValidator.BL.Tests
{
    [TestClass()]
    public class FileCheckerTests
    {
        public TestContext TestContext { get; set; }

        [TestMethod()]
        public void CheckTest()
        {
            var mockFileItem = new Mock<IFileItemResult>();
            var file = Path.Combine(TestContext.TestDeploymentDir, "1.txt");
            mockFileItem.Setup(m => m.FullFilename).Returns(file);
            var fileItems = new List<IFileItemResult>();
            fileItems.Add(mockFileItem.Object);
            var target = new FileChecker(fileItems);
            var result = target.Check(file);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void CheckTest2()
        {
            var file = Path.Combine(TestContext.TestDeploymentDir, "1.txt");
            var fileItems = new List<IFileItemResult>();
            var target = new FileChecker(fileItems);
            var result = target.Check(file);
            Assert.AreEqual("Not definded file was found", result.CheckResult);
            Assert.AreEqual(Path.GetDirectoryName(file), result.DestinationFolder);
            Assert.AreEqual(Path.GetFileName(file), result.ResultFile);
        }
    }
}