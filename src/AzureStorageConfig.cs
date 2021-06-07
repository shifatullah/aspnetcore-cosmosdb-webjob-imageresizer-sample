using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Images
{
    public class AzureStorageConfig
    {
        public string Account { get; set; }
        public string Key { get; set; }

        public string ImageContainerName { get; set; }
    }
}
