using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageInstallerAssessment
{
    public enum ErrorTypes
    {
        Valid = 0,
        NoArguments = 1,
        TooManyArgs = 2,
        ColonMissing = 3,
        NotCommaSeparated = 4,
        InvalidPair = 5,
        NoDependency = 6,
        PackageHasDupe = 7,
        PackageHasCycle = 8,
        GeneralError = 9
    }
}
