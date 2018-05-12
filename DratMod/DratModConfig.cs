using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DratMod
{
    public class DratModConfig
    {
        public bool CoffeeBeanStoreEnabled { get; set; } = true;
        public int CoffeeBeanPrice { get; set; } = 7000;

        public bool ArtifactSubmodEnabled { get; set; } = true;
        public int ArtifactThreshold { get; set; } = 10;
        public int ArtifactPrice { get; set; } = 7500;
        
    }
}
