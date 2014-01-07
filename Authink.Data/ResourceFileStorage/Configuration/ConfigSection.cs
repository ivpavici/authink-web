using System;
using System.Configuration;

namespace Authink.Data.ResourceFileStorage.Configuration
{
    public class ResourceFileStorageAdapterConfigurationSection : ConfigurationSection
    {
        public static ResourceFileStorageAdapter.Settings GetSettings()
        {
            var configurationSection = (ResourceFileStorageAdapterConfigurationSection)ConfigurationManager.GetSection(sectionName: "resourceFileStorageAdapter");

            return new ResourceFileStorageAdapter.Settings(
                ftpHostname:                      new Uri(configurationSection.FtpHostname),
                username:                         configurationSection.Username,
                password:                         configurationSection.Password,
                uploadedFileNameRandomPartLength: configurationSection.UploadedFileNameRandomPartLength,
                httpHostname:                     new Uri(configurationSection.HttpHostname)
            );
        }

        [ConfigurationProperty("ftpHostname",  IsRequired = true)]
        public string FtpHostname
        {
            get { return (string)this["ftpHostname"]; }
            set { this["ftpHostname"] = value.ToString();      }
        }

        [ConfigurationProperty("httpHostname", IsRequired = true)]
        public string HttpHostname
        {
            get { return (string)this["httpHostname"]; }
            set { this["httpHostname"] = value.ToString();      }
        }

        [ConfigurationProperty("username",     IsRequired = true)]
        public string Username
        {
            get { return (string)this["username"]; }
            set { this["username"] = value;        }
        }

        [ConfigurationProperty("password",     IsRequired = true)]
        public string Password
        {
            get { return (string)this["password"]; }
            set { this["password"] = value;        }
        }

        [ConfigurationProperty("uploadedFileNameRandomPartLength", IsRequired = true)]
        public int UploadedFileNameRandomPartLength
        {
            get { return (int)this["uploadedFileNameRandomPartLength"]; }
            set { this["uploadedFileNameRandomPartLength"] = value;     }
        }
    }
}
