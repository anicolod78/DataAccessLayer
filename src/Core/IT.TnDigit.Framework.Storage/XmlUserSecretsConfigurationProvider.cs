using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Microsoft.Extensions.Configuration;

namespace IT.TnDigit.ORM.DataStorage
{
    public class XmlUserSecretsConfigurationProvider : ConfigurationProvider
    {
        private readonly string _filePath;

        public XmlUserSecretsConfigurationProvider(string filePath)
        {
            _filePath = filePath;
        }

        public override void Load()
        {
            if (!File.Exists(_filePath))
                return;

            var data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            try
            {
                var document = new XmlDocument();
                document.Load(_filePath);

                // Carica connectionStrings
                var connectionStrings = document.SelectNodes("//connectionStrings/add");
                if (connectionStrings != null)
                {
                    foreach (XmlNode node in connectionStrings)
                    {
                        var name = node.Attributes?["name"]?.Value;
                        var connectionString = node.Attributes?["connectionString"]?.Value;

                        if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(connectionString))
                        {
                            data[$"ConnectionStrings:{name}"] = connectionString;
                        }
                    }
                }

                // Carica appSettings
                var appSettings = document.SelectNodes("//appSettings/add");
                if (appSettings != null)
                {
                    foreach (XmlNode node in appSettings)
                    {
                        var key = node.Attributes?["key"]?.Value;
                        var value = node.Attributes?["value"]?.Value;

                        if (!string.IsNullOrEmpty(key) && value != null)
                        {
                            data[key] = value;
                        }
                    }
                }

                Data = data;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading XML secrets: {ex.Message}");
            }
        }
    }

    public class XmlUserSecretsConfigurationSource : IConfigurationSource
    {
        private readonly string _filePath;

        public XmlUserSecretsConfigurationSource(string filePath)
        {
            _filePath = filePath;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new XmlUserSecretsConfigurationProvider(_filePath);
        }
    }
}
