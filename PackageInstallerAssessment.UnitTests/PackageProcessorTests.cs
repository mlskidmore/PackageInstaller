using Microsoft.VisualStudio.TestTools.UnitTesting;
using PackageInstallerAssessment.PackageProcessor;

namespace PackageInstallerAssessment.UnitTests
{
    [TestClass]
    public class PackageProcessorTests
    {
        private PkgProcessor packageProcessor;
        private Program program;

        [TestInitialize]
        public void Initialize()
        {            
            packageProcessor = new PkgProcessor();
            program = new Program(packageProcessor);
        }

        [TestMethod]
        [Description("No input package produces NoArguments ResultType")]
        public void NullInput_Produces_NoArgsResultType()
        {
            // Arrange
            string[] inputPackage = { };

            // Act
            var result = packageProcessor.Run(inputPackage);

            // Assert
            Assert.AreEqual(ErrorTypes.NoArguments, result);
        }        
    }
}
