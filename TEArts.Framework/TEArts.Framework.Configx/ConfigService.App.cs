using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace TEArts.Framework.Config
{
    public class AppConfigService
    {
        private static Dictionary<string, Configuration> AppConfiguration = new Dictionary<string, Configuration>();

        public static T GetConfig<T>(string key, Converter<string, T> converterFrom, Converter<T, string> converterTo = null, string appPath = "")
        {
            return GetConfig(key, default(T), converterFrom, converterTo, appPath);
        }
        public static T GetConfig<T>(string key, T value = default(T), Converter<string, T> converterFrom = null, Converter<T, string> converterTo = null, string appPath = "")
        {
            if (string.IsNullOrWhiteSpace(appPath))
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains(key))
                {
                    return converterFrom(ConfigurationManager.AppSettings[key]);
                }
                else
                {
                    SetConfig(key, value, appPath, converterTo);
                    return value;
                }
            }
            else
            {
                Configuration configuration = OpenConfiguration(appPath);
                if (configuration == null)
                {
                    return value;
                }
                else
                {
                    SaveIfNotExists(key, value, configuration, converterTo);
                    return converterFrom(configuration.AppSettings.Settings[key].Value);
                }
            }
        }

        private static void SaveIfNotExists<T>(string key, T value, Configuration configuration, Converter<T, string> converterTo = null)
        {
            if (!(configuration.AppSettings.Settings.AllKeys.Contains(key)))
            {
                if (converterTo == null)
                {
                    converterTo = x => x.ToString();
                }
                configuration.AppSettings.Settings.Add(key, converterTo(value));
                configuration.Save(ConfigurationSaveMode.Minimal);
                ConfigurationManager.RefreshSection("appSettings");
            }
        }

        private static Configuration OpenConfiguration(string appPath)
        {
            Configuration configuration = null;
            if (AppConfiguration.ContainsKey(appPath.ToLower()))
            {
                configuration = AppConfiguration[appPath.ToLower()];
            }
            else
            {
                try
                {
                    configuration = ConfigurationManager.OpenExeConfiguration(new FileInfo(appPath).FullName);
                    AppConfiguration[appPath.ToLower()] = configuration;
                }
                catch { }
            }

            return configuration;
        }

        public static void SetConfig<T>(string key, T value, string appPath = "", Converter<T, string> converterTo = null)
        {
            Configuration configuration = null;
            if (string.IsNullOrWhiteSpace(appPath))
            {
                configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            }
            else
            {
                configuration = OpenConfiguration(appPath);
            }
            try
            {
                if (configuration.AppSettings.Settings.AllKeys.Contains(key))
                {
                    configuration.AppSettings.Settings.Remove(key);
                }
                SaveIfNotExists(key, value, configuration, converterTo);
            }
            catch { }
        }
    }
}
