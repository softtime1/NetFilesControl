using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FilesValidator.BL;
using System.IO;

namespace FilesValidator.Tests.BL
{
    [TestClass]
    public class ExcelReaderTests
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException), "NotExitstFile.xlsx")]
        public void ReadFileItemTest()
        {
            using (var target = new ExcelReader("NotExitstFile.xlsx"))
            {
                Assert.IsNotNull(target);
                var fileItem = target.ReadFileItem();
                Assert.IsNull(fileItem);
            }
        }

        [TestMethod]
        public void ReadFileItemTest2()
        {
            using (var target = new ExcelReader(Path.Combine(
                TestContext.TestDeploymentDir, "BookEmpty.xlsx")))
            {
                Assert.IsNotNull(target);
                var fileItem = target.ReadFileItem();
                Assert.IsNull(fileItem);
            }
        }

        [TestMethod]
        public void ReadFileItemTest3()
        {
            using (var target = new ExcelReader(Path.Combine(
                TestContext.TestDeploymentDir, "Book1.xlsx")))
            {
                Assert.IsNotNull(target);
                var fileItem = target.ReadFileItem();
                Assert.IsNotNull(fileItem);
                Assert.AreEqual("Hello", fileItem.SourceFile);
                Assert.AreEqual("World", fileItem.DestinationFolder);
                Assert.AreEqual("Peace", fileItem.ResultFile);
                Assert.AreEqual(
                    "9e107d9d372bb6826bd81d3542a419d6", fileItem.ResultFileMD5);
                fileItem = target.ReadFileItem();
                Assert.IsNull(fileItem);
            }
        }
    }
}
