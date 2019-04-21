using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ContourCore.database
{
    class JsonTreeDB
    {
        public string StorePath { get; private set; }
        public JObject Store { get; private set; }

        public JsonTreeDB(string storePath)
        {
            StorePath = storePath;
            Read();
        }

        public void Write()
        {
            try
            {
                File.WriteAllText(StorePath, JsonConvert.SerializeObject(Store));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to store - {ex}");
            }
        }

        public void Read()
        {
            if (File.Exists(StorePath))
            {
                try
                {
                    Store = (JObject)JsonConvert.DeserializeObject(File.ReadAllText(StorePath));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading from store - {ex}");
                }
            }
            else
            {
                Console.WriteLine($"Can not find store {StorePath} - creating new store");
                Store = new JObject();
            }
        }
    }
}
