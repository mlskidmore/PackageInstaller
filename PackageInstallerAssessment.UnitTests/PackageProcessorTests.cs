using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PackageInstallerAssessment.UnitTests
{
    [TestClass]
    public class PackageProcessorTests
    {
        [TestMethod]
        [Description("No input package produces NoArguments ResultType")]
        public void NullInput_Produces_NoArgsResultType()
        {
            // Arrange
            string[] inputPackage = { };

            // Act
            var result = packageProcessor.Run(inputPackage);

            // Assert
            Assert.AreEqual(ReturnTypes.NoArguments, result);
        }

        [TestMethod]
    }
}
