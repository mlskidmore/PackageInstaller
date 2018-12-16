using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageInstallerAssessment.PackageFolder
{
    public interface IPackage
    {
        string packageName { get; set; }
        IPackage Dependency { get; set; }
    }
}
