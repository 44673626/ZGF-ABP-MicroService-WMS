using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileStorage
{

    public static class FileStorageDbProperties
    {
        public static string DbTablePrefix { get; set; } = "Set";

        public static string DbSchema { get; set; } = null;

        public const string ConnectionStringName = "FileStorage";

        public const string WMSConnectionStringName = "WMS";

    }
}
