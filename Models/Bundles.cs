using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bundles.Models
{
    public class Bundle
    {
        public string? Name { get; set; }

        public string? Directory { get; set; }

        public string? Target { get; set; }
        public string? TargetFile { get; set; }
    }

}
