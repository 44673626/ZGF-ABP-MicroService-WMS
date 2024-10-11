using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Business.CommonManagement.UploadBlobFiles.comm
{
    public static class ConfigDirHelper
    {
        private static IConfigurationRoot _appConfiguration = AppConfigurations.Get(Environment.CurrentDirectory);
        //用法1(有嵌套)：GetAppSetting("Authentication", "JwtBearer:SecurityKey")
        //用法2：GetAppSetting("App", "ServerRootAddress")
        public static string GetAppSetting(string section, string key)
        {
            return _appConfiguration.GetSection(section)[key];
        }
        public static string GetConnectionString(string key)
        {
            return _appConfiguration.GetConnectionString(key);
        }

        public static string GetDirPath(string reDir)
        {
            string dir = $"{Environment.CurrentDirectory}/{reDir}";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            return Path.GetFullPath(dir);
        }
    }
}
