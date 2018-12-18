using PackageInstallerAssessment.PackageFolder;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageInstallerAssessment.PackageProcessor
{
    public class PkgProcessor : IPackageProcessor
    {
        // Use these for errors
        private string[] packageMissingColon;
        private List<string> duplicatePackages;
        public bool IsCycle = false;

        // Use this to find dependencies        
        public List<IPackage> ParsedPackageCollection { get; private set; }

        public PkgProcessor()
        {            
            ParsedPackageCollection = new List<IPackage>();
        }

        public ErrorTypes Run(string[] args)
        {
            ErrorTypes result = ErrorTypes.Valid;

            try
            {
                // Get/process the inbound package
                result = processArgs(args);

                if (result != ErrorTypes.Valid)
                {
                    processInvalidResult(result);

                    return result;
                }

                // TODO: get sequenced dependencies
            }
            catch (Exception ex)
            {
                Console.WriteLine("Message: " + ex.Message);
                //Console.ReadLine();
            }

            return result;
        }
        public ErrorTypes processArgs(string[] args)
        {
            // Check for no args
            if (args.Length == 0)
                return ErrorTypes.NoArguments;

            // Check for too many args
            if (args.Length > 1)
                return ErrorTypes.TooManyArgs;

            // Convert args to a string so we can work with it
            string packageCollection = args[0];

            // Check for comma separation
            if (checkForCommaSeparation(packageCollection) != ErrorTypes.Valid)
                return ErrorTypes.NotCommaSeparated;

            // Check for colon formatting
            if (checkForColonFormatting(packageCollection) != ErrorTypes.Valid)
                return ErrorTypes.ColonMissing;

            // Check for package/dependency pair
            if (checkForPairs(packageCollection) != ErrorTypes.Valid)
                return ErrorTypes.InvalidPair;

            // Check for at least one dependency
            if (checkForNoDependencies(packageCollection) != ErrorTypes.NoDependency)
                return ErrorTypes.NoDependency;

            // Check for no Package duplicates
            if (checkForPackageDupes(packageCollection) != ErrorTypes.PackageHasDupe)
                return ErrorTypes.PackageHasDupe;

            // Check for package cycles
            if (OrderDependencies(packageCollection) != ErrorTypes.PackageHasCycle)
                return ErrorTypes.PackageHasCycle;

            return ErrorTypes.Valid;
        }

        public ErrorTypes checkForCommaSeparation(string packageString)
        {
            var parsedPackageArray = packageString.Split(',');
            int commaCount = packageString.Split(',').Length - 1;

            if (commaCount == 0)
                return ErrorTypes.NotCommaSeparated;

            if ((parsedPackageArray.Length - commaCount) > 1)
                return ErrorTypes.NotCommaSeparated;

            // TODO: find missing comma location

            return ErrorTypes.Valid;
        }

        public ErrorTypes checkForColonFormatting(string packageString)
        {
            int packagePosition = 0;

            // Check if entire package list has at least one colon
            if (!packageString.Contains(':'))
                return ErrorTypes.ColonMissing;

            var parsedPackageArray = packageString.Split(',');

            // Strip off any white space
            for (int i = 0; i < parsedPackageArray.Length; i++)
            {
                parsedPackageArray[i] = parsedPackageArray[i].Trim();
            }

            // Store packages missing a colon
            packageMissingColon = new string[parsedPackageArray.Length];

            // Find/keep any/all packages missing a colon
            foreach (string element in parsedPackageArray)
            {
                if (!element.Contains(':') && !string.IsNullOrEmpty(element))
                {
                    if (element.Contains(' '))
                        packageMissingColon[packagePosition] = element.Substring(0, element.IndexOf(' '));
                    else
                        packageMissingColon[packagePosition] = element;
                }

                packagePosition++;
            }

            // Remove null entries
            packageMissingColon = packageMissingColon.Where(el => el != null).ToArray();

            if (packageMissingColon.Length > 0)
            {
                return ErrorTypes.ColonMissing;
            }

            return ErrorTypes.Valid;
        }

        public ErrorTypes checkForPairs(string packageString)
        {
            var packageArray = packageString.Split(',');

            foreach (string package in packageArray)
            {
                string[] pair = package.Split(':');

                if (pair.Length != 2)
                    return ErrorTypes.InvalidPair;
            }
            return ErrorTypes.Valid;
        }

        public ErrorTypes checkForNoDependencies(string packageString)
        {
            // Remove extra white spaces between comma and next package name
            //packageString = new Regex(" {2,}").Replace(packageString, " ");
            while (packageString.Contains("  "))
                packageString = packageString.Replace("  ", " ");

            // Remove extra white space between colon and next comma
            while (packageString.Contains(": ,"))
                packageString = packageString.Replace(": ,", ":,");

            packageString = packageString.Trim();

            var parsedPackageArray = packageString.Split(',');

            //var packages = new List<string>();
            var dependencies = new List<string>();

            foreach (string package in parsedPackageArray)
            {
                string[] pair = package.Split(':');

                if (!string.IsNullOrEmpty(pair[1].Trim()))
                    dependencies.Add(pair[1]);
            }

            if (dependencies.Count() == 0)
                // After stripping spaces, following strings determine no dependency
                return ErrorTypes.NoDependency;

            return ErrorTypes.Valid;
        }

        public ErrorTypes checkForPackageDupes(string packageString)
        {
            var parsedPackage = packageString.Split(',');
            var packageDupes = new List<string>();
            var dependencyDupes = new List<string>();

            foreach (string packageDependencyPair in parsedPackage)
            {
                string[] pair = packageDependencyPair.Split(':');

                string packageName = pair[0].Trim();
                string dependencyName = pair[1].Trim();

                packageDupes.Add(packageName);
                dependencyDupes.Add(dependencyName); // if dep dupes are needed
            }

            // Count package dupes
            bool areDupes = packageDupes.GroupBy(n => n).Any(c => c.Count() > 1);

            // Get the duplicate package names
            duplicatePackages = packageDupes.GroupBy(p => p)
                                            .Where(group => group.Count() > 1)
                                            .Select(group => group.Key)
                                            .ToList();

            if (areDupes)
                return ErrorTypes.PackageHasDupe;

            return ErrorTypes.Valid;
        }

        public ErrorTypes OrderDependencies(string PackageCollection)
        {
            var parsedPackage = PackageCollection.Split(',');

            if (CheckForCycles(ref parsedPackage))
                return ErrorTypes.PackageHasCycle;

            return ErrorTypes.Valid;
        }

        private bool CheckForCycles(ref string[] parsedPackage)
        {
            foreach (string package in parsedPackage)
            {
                string[] pair = package.Split(':');

                string packageName = pair[0].Trim();
                string dependencyName = pair[1].Trim();

                CheckPackageAndDependency(packageName, dependencyName);
            }

            if (!IsCycle)
                return true;

            return false;
        }
        private bool CheckPackageAndDependency(string packageName, string dependencyName = null)
        {
            // Check if package already exits...
            var existingPackage = CheckForExistingPackage(packageName);

            // If pkg exists and has dependency, cycle exists
            if (existingPackage != null && existingPackage.Dependency != null)
            {
                IsCycle = true;
                return IsCycle;
            }

            var newPackages = new List<IPackage>();

            IPackage dependency = default(IPackage);

            // When dependency name is available check for it
            if (!string.IsNullOrWhiteSpace(dependencyName))
                dependency = FindOrCreatePackage(dependencyName, newPackages);

            // Same for package: get it from the list or create it
            var package = FindOrCreatePackage(packageName, newPackages);
            package.Dependency = dependency;

            // Now check for cycles
            if (packageHasCycle(package, package.packageName))
            {
                IsCycle = true;
                return IsCycle;
            }

            ParsedPackageCollection.AddRange(newPackages);
            var last = ParsedPackageCollection.Last();
            ParsedPackageCollection.Add(last);

            return false;
        }

        private IPackage CheckForExistingPackage(string packageName)
        {
            return ParsedPackageCollection.Find(p => p.packageName.Equals(p.packageName, StringComparison.CurrentCultureIgnoreCase));
        }

        private IPackage FindOrCreatePackage(string packageName, IList packages = null)
        {
            var package = CheckForExistingPackage(packageName);

            // If no existing package, create it
            if (package == null)
            {
                package = new Package()
                {
                    packageName = packageName
                };

                if (packages == null)
                {
                    packages = ParsedPackageCollection;
                }

                packages.Add(package);
            }

            return package;
        }

        private bool packageHasCycle(IPackage package, string originalPackageName)
        {
            // If the dependency is null, can't be a cycle
            if (package.Dependency == null)
                return false;

            // If package name = dependency name, cycle exists
            if (package.Equals(package.Dependency))
                return true;

            // If originaPackageName is same as dependency Name, cycle exists
            if (package.Dependency.packageName == originalPackageName)
                return true;

            // Recurse through dependencies
            return packageHasCycle(package.Dependency, originalPackageName);
        }

        public void processInvalidResult(ErrorTypes result)
        {
            switch (result)
            {
                case ErrorTypes.NoArguments:
                    writeLine("You must provide a list of single dependency packages.");
                    writeLine("Example: \"KittenService:, Leetmeme: Cyberportal, Cyberportal: Ice\"");
                    break;
            }
        }

        public void writeLine(string s)
        {
            Console.WriteLine(s);

            //Console.ReadLine();
        }

        public void writeLine(string[] s)
        {
            throw new NotImplementedException();
        }
    }
}
