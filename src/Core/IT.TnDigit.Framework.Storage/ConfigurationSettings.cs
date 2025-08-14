using System.Collections.Specialized;

namespace System.Configuration
{
    public static class StorageConfigurationSettings
    {
        private static readonly NameValueCollection appSettings = new NameValueCollection();

        public static bool ConfigExists()
        {
            string configPath = "Storage.config";

            if (System.IO.File.Exists(configPath) == false)
            {
                try
                {
                    return false;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("EXCEPTION - ERROR : " + ex.ToString());
                    return false;
                }
            }
            return true;
        }

        static StorageConfigurationSettings()
        {
            string configPath = "Storage.config";

            if (System.IO.File.Exists(configPath) == false)
            {
                throw new System.IO.FileNotFoundException(string.Format("File Storage.config non trovato nel percorso {0} .", configPath));
            }

            System.Xml.XmlReader reader = System.Xml.XmlReader.Create(configPath);

            int levels = 0;
            bool exit = false;

            while (reader.Read())
            {
                if (reader.NodeType == System.Xml.XmlNodeType.Element)
                {
                    if (levels == 0 && reader.Name == "configuration")
                        levels++;
                    if (levels == 1 && reader.Name == "appSettings")
                        levels++;
                    if (levels == 2)
                    {
                        while (reader.Read())
                        {
                            if (reader.NodeType == System.Xml.XmlNodeType.Element && reader.Name == "add")
                            {
                                appSettings.Add(reader.GetAttribute("key"), reader.GetAttribute("value"));
                            }
                            else if (reader.NodeType == System.Xml.XmlNodeType.EndElement && reader.Name == "appSettings")
                            {
                                exit = true;
                                break;
                            }
                        }

                        if (exit) break;
                    }
                }
            }

            reader.Close();
        }

        public static NameValueCollection AppSettings
        {
            get { return appSettings; }
        }

        public static string GetApplicationDirectory()
        {
            string path = System.Reflection.Assembly.GetCallingAssembly().GetModules()[0].FullyQualifiedName;

            return System.IO.Path.GetDirectoryName(path);
        }

    }
}
