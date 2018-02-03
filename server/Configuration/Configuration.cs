using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;

namespace Leeward.Configuration
{
    [DataContract]  
    internal abstract class Configuration
    {
        private static readonly Utils.Logger _logger = Utils.Logger.Get(typeof(Configuration));
        
        protected string path;

        public abstract void Save();

        protected static void Save<T>(T conf) where T : Configuration
        {
            // TODO: Swap old file to .bak, to recover on error
            using (FileStream fs = new FileStream(conf.path, FileMode.Create))
            using (XmlDictionaryWriter jsonWriter = JsonReaderWriterFactory.CreateJsonWriter(fs, Encoding.UTF8, true, true, "  "))
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
                ser.WriteObject(jsonWriter, conf);
            }
            
            _logger.Debug($"Configuration file saved: {conf.path}");
        }
        
        protected static T LoadOrDefault<T>(string name) where T : Configuration, new()
        {
            string fileName = Path.Combine(Directory.GetCurrentDirectory(), name + ".json"); // TODO: Configurated by parameter

            T desConf = new T();
            
            if (File.Exists(fileName))
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Open))
                {
                    DataContractJsonSerializer ser = new DataContractJsonSerializer(desConf.GetType());
                    desConf = ser.ReadObject(fs) as T;
                }
                
                _logger.Debug($"Configuration file loaded: {fileName}");
            }
            else
            {
                _logger.Debug($"Configuration created: {name}");
            }

            desConf.path = fileName;
            return desConf;
        }
    }
}