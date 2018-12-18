using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageInstallerAssessment.PackageProcessor
{
    public interface IPackageProcessor
    {
        ErrorType Run(string[] args);
        ErrorType processArgs(string[] args);
        void processInvalidResult(ErrorType result);
        void writeLine(string[] s);
    }
}
