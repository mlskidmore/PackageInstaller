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
            Assert.AreEqual(ErrorType.NoArguments, result);
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
            Assert.AreEqual(ErrorType.TooManyArgs, result);
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
            Assert.AreEqual(ErrorType.NotCommaSeparated, result);
        }

        [TestMethod]
        [Description("Should produce ColonMissing ReturnType when colon is missing.")]
        public void MissingColonInput_Produces_ColonMissingResultTyoe()
        {
            // Arrange
            string inputPackage = "KittenService, Leetmeme:  Cyberportal,  Cyberportal  Ice, CamelCaser:  KittenService, Fraudstream: Leetmeme, Ice    ";

            // Act
            var actualOutput = packageProcessor.checkForColonFormatting(inputPackage);

            // Assert
            Assert.AreEqual(ErrorType.ColonMissing, actualOutput);
        }

        [TestMethod]
        [Description("Should produce InvalidPair ReturnType when package is missing.")]
        public void MissingPair_Produces_InvalidPairType()
        {
            // Arrange
            // Missing first pair
            string inputPackage = ", Leetmeme: Cyberportal, Cyberportal Ice, CamelCaser:  KittenService, Fraudstream: Leetmeme, Ice";
            // More than pair.length == 2
            //string inputPackage = "KittenService:, Leetmeme: Cyberportal:, Cyberportal Ice, CamelCaser:  KittenService, Fraudstream: Leetmeme, Ice";

            // Act
            var actualOutput = packageProcessor.checkForPairs(inputPackage);

            // Assert
            Assert.AreEqual(ErrorType.InvalidPair, actualOutput);
        }

        [TestMethod]
        [Description("Should produce NoDependency ReturnType when each package has no dependency.")]
        public void NoDependency_Produces_NoDependencyResultType()
        {
            // Arrange
            string inputPackage = "KittenService:, Cyberportal:, CamelCaser:, Fraudstream:, Ice:" ;

            // Act
            var actualOutpout = packageProcessor.checkForNoDependencies(inputPackage);

            // Assert
            Assert.AreEqual(ErrorType.NoDependency, actualOutpout);
        }

        [TestMethod]
        [Description("Should produce PackageDupe ReturnType when there are duplicate packages.")]
        public void DupePackage_Produces_PackageHasDupeResultType()
        {
            // Arrange
            string inputPackage = "KittenService:, Leetmeme: Cyberportal, Cyberportal: Ice, CamelCaser: KittenService, Cyberportal: Leetmeme, KittenService:";

            // Act
            var actualOutpout = packageProcessor.checkForPackageDupes(inputPackage);

            // Assert
            Assert.AreEqual(ErrorType.PackageHasDupe, actualOutpout);
        }

        [TestMethod]
        [Description("Should produce PackageHasCycle ReturnType when package contains a cycle.")]
        public void CylcePackage_Produces_PackageHasCyclesResultType()
        {
            // Arrange
            //string inputCyclePackage = "KittenService:, Leetmeme: Cyberportal, Cyberportal: Ice, CamelCaser: KittenService, Fraudstream:, Ice: Leetmeme";
            string inputCyclePackage = "KittenService:, Leetmeme: Cyberportal, Cyberportal: Ice, CamelCaser: KittenService, Fraudstream: Leetmeme, Ice:";
            // Act
            var actualOutpout = packageProcessor.OrderDependencies(inputCyclePackage);

            // Assert
            Assert.AreEqual(ErrorType.PackageHasCycle, actualOutpout);
        }
    }
}
