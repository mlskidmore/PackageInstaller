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

        [TestMethod]
        [Description("Should produce TooManyArgs ResultType.")]
        public void TwoPackageInput_Produces_TooManyArgsResultType()
        {
            // Arrange
            string[] mockPackage = { "KittenService:, Leetmeme: Cyberportal, Cyberportal: Ice, CamelCaser: KittenService, Fraudstream: Leetmeme, Ice:",
                                     "KittenService:, Leetmeme: Cyberportal, Cyberportal: Ice, CamelCaser: KittenService, Fraudstream: Leetmeme, Ice:"};

            // Act
            var result = packageProcessor.Run(mockPackage);

            // Assert
            Assert.AreEqual(ErrorTypes.TooManyArgs, result);
        }

        [TestMethod]
        [Description("Should produce NotCommaSeparated ReturnType when packages are not comma separated.")]
        public void NotCommaSeparated_Produces_NotCommaSeparatedResultTyoe()
        {
            // Arrange
            string mockPackage = "KittenService: Leetmeme: Cyberportal Cyberportal: Ice CamelCaser: KittenService Fraudstream: Leetmeme Ice:";

            // Act
            var result = packageProcessor.checkForCommaSeparation(mockPackage);

            // Assert
            Assert.AreEqual(ErrorTypes.NotCommaSeparated, result);
        }
    }
}
