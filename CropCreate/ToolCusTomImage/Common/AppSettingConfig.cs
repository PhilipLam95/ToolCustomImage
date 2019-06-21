using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CropCreate.Common
{
    class AppSettingConfig
    {
        public static string GetAppSetting(string key)
        {

            string value = string.Empty;

            if (!(string.IsNullOrEmpty(key)))
            {

                ConfigurationManager.RefreshSection("appSettings");
                value = ConfigurationManager.AppSettings[key];
            }

            return value;

        }


        //public static bool SetAppSettingConfig̣(string value,string key)
        //{
        //    string tmp = string.Empty;
        //    ConfigurationManager.RefreshSection("appSettings");
        //    tmp = ConfigurationManager.AppSettings[key];
        //    bool flag = false;
        //    if (!(string.IsNullOrEmpty(tmp)))
        //    {
        //        ConfigurationManager.AppSettings[key] = value;
        //        flag = true;
        //    }
        //    else
        //    {
        //        flag = false;
        //    }

        //    string dsadsa = GetAppSetting(key);

        //    return flag;

        //}
    }
}
