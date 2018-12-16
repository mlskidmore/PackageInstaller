using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageInstallerAssessment.PackageFolder
{
    public class Package : IPackage
    {
        public string packageName { get; set; }
        public IPackage Dependency { get; set; }
    }
}
