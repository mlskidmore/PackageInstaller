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
            ReturnTypes result = ReturnTypes.Valid;

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

            return result;
        }

        public void processInvalidResult(ErrorTypes result)
        {
            throw new NotImplementedException();
        }       

        public void writeLine(string[] s)
        {
            throw new NotImplementedException();
        }
    }
}
