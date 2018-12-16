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
            throw new NotImplementedException();
        }
        public ErrorTypes processArgs(string[] args)
        {
            throw new NotImplementedException();
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
