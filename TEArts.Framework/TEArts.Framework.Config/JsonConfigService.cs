using Newtonsoft.Json;
using System.Configuration;
using System.IO;
using System.Linq;

namespace TEArts.Etc.CollectionLibrary
{
    public class JsonConfigService
    {
        public static string ReadAppConfig(string node, string value)
        {
            //string s = ConfigurationManager.AppSettings.Get(node);
            string s = string.Empty;
            if (ConfigurationManager.AppSettings.AllKeys.Contains<string>(node))
            {
                s = ConfigurationManager.AppSettings[node];
            }
            else
            {
                //ConfigurationManager.AppSettings.Set(node, file);
                //s = file;
                //ConfigurationSection cfs = cfg.GetSection("Configure");
                s = value;
            }
            return s;
        }
        public static void SaveAppConfig(string node, string value)
        {
            if (ConfigurationManager.AppSettings.AllKeys.Contains<string>(node))
            {
                ConfigurationManager.AppSettings[node] = value;
            }
            else
            {
                Configuration cfl = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                cfl.AppSettings.Settings.Add(node, value);
                cfl.Save(ConfigurationSaveMode.Modified);
            }
            ConfigurationManager.RefreshSection("appSettings");
        }
        public static T ReadConfig<T>(string node, string file, T value)
        {
            try
            {
                string f = ReadAppConfig(node, file);
                using (TextReader tr = ((TextReader)(new StreamReader(f))))
                {
                    f = tr.ReadToEnd();
                    tr.Close();
                    return JsonConvert.DeserializeObject<T>(f);
                    //return null;
                }
            }
            catch
            {
                return value;
            }
        }
        public static bool SaveConfig<T>(T config, string node, string file)
        {
            try
            {
                using (TextWriter tr = new StreamWriter(ReadAppConfig(node, file)))
                {
                    tr.Write(JsonConvert.SerializeObject(config));
                    tr.Flush();
                    tr.Close();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
