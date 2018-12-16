﻿using PackageInstallerAssessment.PackageFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageInstallerAssessment.PackageProcessor
{
    public class PkgProcessor : IPackageProcessor
    {   
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

            return result;
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
