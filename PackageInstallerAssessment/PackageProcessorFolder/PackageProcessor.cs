using PackageInstallerAssessment.PackageFolder;
using System;
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
            ErrorTypes result = ErrorTypes.Valid;

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
