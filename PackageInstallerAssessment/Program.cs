using PackageInstallerAssessment.PackageProcessor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageInstallerAssessment
{
    public class Program
    {
        private IPackageProcessor _packageProcessor;

        public Program(IPackageProcessor packageProcessor)
        {
            _packageProcessor = packageProcessor;
        }
        static void Main(string[] args)
        {
            var program = new Program(new PkgProcessor());

            program._packageProcessor.Run(args);
        }
    }
}
