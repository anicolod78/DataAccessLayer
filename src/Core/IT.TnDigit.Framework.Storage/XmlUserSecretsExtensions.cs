using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace IT.TnDigit.ORM.DataStorage
{
    public static class XmlUserSecretsExtensions
    {
        public static IConfigurationBuilder AddXmlUserSecrets(this IConfigurationBuilder builder, Assembly assembly)
        {
            var userSecretsId = assembly.GetCustomAttribute<UserSecretsIdAttribute>()?.UserSecretsId;
            if (string.IsNullOrEmpty(userSecretsId))
                return builder;

            return builder.AddXmlUserSecrets(userSecretsId);
        }

        public static IConfigurationBuilder AddXmlUserSecrets(this IConfigurationBuilder builder, string userSecretsId)
        {
            var secretsPath = GetSecretsPathFromSecretsId(userSecretsId);
            return builder.AddXmlUserSecrets(secretsPath, true);
        }

        public static IConfigurationBuilder AddXmlUserSecrets(this IConfigurationBuilder builder, string filePath, bool optional)
        {
            if (!optional && !File.Exists(filePath))
                throw new FileNotFoundException($"XML secrets file not found at: {filePath}");

            if (File.Exists(filePath))
            {
                builder.Add(new XmlUserSecretsConfigurationSource(filePath));
            }

            return builder;
        }

        private static string GetSecretsPathFromSecretsId(string userSecretsId)
        {   
            try
            {
                var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var secretsDirectory = Path.Combine(appData, "Microsoft", "UserSecrets", userSecretsId);
                return Path.Combine(secretsDirectory, "secrets.xml");
            }
            catch
            {
                return null;
            }
        }
    }
}
