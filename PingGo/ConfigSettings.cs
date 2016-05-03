using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingGo
{
    public class ConfigSettings
    {
        private ServiceConfigurationSection ServiceConfigurationSection
        {
            get
            {
                return (ServiceConfigurationSection)ConfigurationManager.GetSection("ServicesSection"); //watchesSection
            }
        }

        private ServiceCollection ServiceCollection
        {
            get
            {
                return this.ServiceConfigurationSection.Services;
            }
        }

        public IEnumerable<ServiceConfig> Services
        {
            get
            {
                foreach (ServiceConfig element in this.ServiceCollection)
                {
                    if (element != null)
                        yield return element;
                }
            }
        }
    }

    public class ServiceConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("Services", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(ServiceCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public ServiceCollection Services
        {
            get
            {
                return (ServiceCollection)base["Services"];
            }
        }
    }

    public class ServiceCollection : ConfigurationElementCollection
    {
        public ServiceCollection()
        {
        }

        public ServiceConfig this[int index]
        {
            get { return (ServiceConfig)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public void Add(ServiceConfig serviceConfig)
        {
            BaseAdd(serviceConfig);
        }

        public void Clear()
        {
            BaseClear();
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ServiceConfig();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ServiceConfig)element).Name;
        }

        public void Remove(ServiceConfig serviceConfig)
        {
            BaseRemove(serviceConfig.Name);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }
    }

    public class ServiceConfig : ConfigurationElement
    {
        public ServiceConfig() { }

        public ServiceConfig(string name, string url)
        {
            Name = name;
            Url = url;
            IsActive = true;
        }

        //[ConfigurationProperty("Port", DefaultValue = 0, IsRequired = true, IsKey = true)]
        //public int Port
        //{
        //    get { return (int)this["Port"]; }
        //    set { this["Port"] = value; }
        //}

        [ConfigurationProperty("Name", DefaultValue = "name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)this["Name"]; }
            set { this["Name"] = value; }
        }

        [ConfigurationProperty("Url", DefaultValue = "localhost", IsRequired = true, IsKey = false)]
        public string Url
        {
            get { return (string)this["Url"]; }
            set { this["Url"] = value; }
        }

        [ConfigurationProperty("IsActive", DefaultValue = "true", IsRequired = false, IsKey = false)]
        public bool IsActive
        {
            get { return (bool)this["IsActive"]; }
            set { this["IsActive"] = value; }
        }
    }



    /*
    public class ConfigSettings
    {
        public ConnectionSection ServerAppearanceConfiguration
        {
            get
            {
                return (ConnectionSection)ConfigurationManager.GetSection("serverSection");
            }
        }

        public ServerAppearanceCollection ServerApperances
        {
            get
            {
                return this.ServerAppearanceConfiguration.ServerElement;
            }
        }

        public IEnumerable<Element> ServerElements
        {
            get
            {
                foreach (Element selement in this.ServerApperances)
                {
                    if (selement != null)
                        yield return selement;
                }
            }
        }
    }

    public class ConnectionSection : ConfigurationSection
    {
        [ConfigurationProperty("Servers")]
        public ServerAppearanceCollection ServerElement
        {
            get { return ((ServerAppearanceCollection)(base["Servers"])); }
            set { base["Servers"] = value; }
        }
    }

    [ConfigurationCollection(typeof(Element))]
    public class ServerAppearanceCollection : ConfigurationElementCollection
    {
        internal const string PropertyName = "Element";

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMapAlternate;
            }
        }
        protected override string ElementName
        {
            get
            {
                return PropertyName;
            }
        }

        protected override bool IsElementName(string elementName)
        {
            return elementName.Equals(PropertyName, StringComparison.InvariantCultureIgnoreCase);
        }


        public override bool IsReadOnly()
        {
            return false;
        }


        protected override ConfigurationElement CreateNewElement()
        {
            return new Element();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((Element)(element)).name;
        }

        public Element this[int idx]
        {
            get
            {
                return (Element)BaseGet(idx);
            }
        }
    }

    public class Element : ConfigurationElement
    {
        [ConfigurationProperty("name", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string name
        {
            get { return (string)base["name"]; }
            set { base["name"] = value; }
        }
        [ConfigurationProperty("servername", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string servername
        {
            get { return (string)base["servername"]; }
            set { base["servername"] = value; }
        }

        [ConfigurationProperty("isactive", DefaultValue = "true", IsKey = false, IsRequired = false)]
        public bool isactive
        {
            get { return (bool)base["isactive"]; }
            set { base["isactive"] = value; }
        }

        [ConfigurationProperty("userid", DefaultValue = "abhi", IsKey = false, IsRequired = false)]
        public string userid
        {
            get { return (string)base["userid"]; }
            set { base["userid"] = value; }
        }

        [ConfigurationProperty("password", DefaultValue = "password", IsKey = false, IsRequired = false)]
        public string password
        {
            get { return (string)base["password"]; }
            set { base["password"] = value; }
        }
    }*/



}
