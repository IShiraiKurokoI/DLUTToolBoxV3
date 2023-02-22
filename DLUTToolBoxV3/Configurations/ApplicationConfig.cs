using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace DLUTToolBoxV3.Configurations
{
    static class ApplicationConfig
    {
        static ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        public static void SaveSettings(String Key, String Value)
        {
            localSettings.Values[Key] = Value;
        }

        public static string GetSettings(String Key)
        {
            return localSettings.Values[Key] as string;
        }
    }
}
