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

            return result;
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
