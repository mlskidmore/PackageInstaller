using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageInstallerAssessment.PackageProcessor
{
    public interface IPackageProcessor
    {
        ErrorTypes Run(string[] args);
        ErrorTypes processArgs(string[] args);
        void processInvalidResult(ErrorTypes result);
        void writeLine(string[] s);
    }
}
