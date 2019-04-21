using LiteDB;
using System;
using Newtonsoft.Json;
using System.Dynamic;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace LiteDBPrototype
{
    class Program
    {
        // Basic example
        public class Tree
        {
            public int id { get; set; }
            public string Name { get; set; }
            public Tree[] Node { get; set; }
        }



        static void Main(string[] args)
        {

            // Open database (or create if not exits)
            //using (var db = new LiteDatabase(@"LiteDBTestData.db"))
            //{
            //    // Get customer collection
            //    var configTree = db.GetCollection<Tree>("Config");

            //    var testNode = new Tree { Name = "TEST", Node = null };
            //    var prodNode = new Tree { Name = "PROD", Node = null };

            //    var testNode2 = new Tree { Name = "TEST", Node = null };
            //    var prodNode2 = new Tree { Name = "PROD", Node = null };

            //    var testNode3 = new Tree { Name = "TEST", Node = null };
            //    var prodNode3 = new Tree { Name = "PROD", Node = null };

            //    var mySqlNode = new Tree
            //    {
            //        Name = "MYSQL",
            //        Node = new Tree[] { testNode, prodNode }
            //    };

            //    var redisNode = new Tree
            //    {
            //        Name = "REDIS",
            //        Node = new Tree[] { testNode2, prodNode2 }
            //    };

            //    var databaseNode = new Tree
            //    {
            //        Name = "DATABASE",
            //        Node = new Tree[] { mySqlNode, redisNode }
            //    };

            //    var settingsNode = new Tree
            //    {
            //        Name = "SETTINGS",
            //        Node = new Tree[] { testNode3, prodNode3 }
            //    };

            //    var webappNode = new Tree
            //    {
            //        Name = "WEBAPP",
            //        Node = new Tree[] { settingsNode }
            //    };

            //    var baseNode = new Tree
            //    {
            //        Name = "*",
            //        Node = new Tree[] { databaseNode, webappNode }
            //    };

            //    configTree.Insert(baseNode);

            //    // Use Linq to query documents
            //    var results = configTree.Find(x => x.Name.StartsWith("D"));
            //}

            dynamic store = new ExpandoObject();
            store.DATABASE = new ExpandoObject();
            store.DATABASE.MySQL = new ExpandoObject();
            store.DATABASE.MySQL.Test = new ExpandoObject();
            store.DATABASE.MySQL.Prod = new ExpandoObject();
            store.DATABASE.Redis = new ExpandoObject();
            store.DATABASE.Redis.Test = new ExpandoObject();
            store.DATABASE.Redis.Prod = new ExpandoObject();

            store.WEBAPP = new ExpandoObject();
            store.WEBAPP.SETTINGS = new ExpandoObject();
            store.WEBAPP.SETTINGS.Test = new ExpandoObject();
            store.WEBAPP.SETTINGS.Prod = new ExpandoObject();
            store.WEBAPP.NEW = new ExpandoObject();
            store.WEBAPP.SECRET = new ExpandoObject();
            store.WEBAPP.DELETE = new ExpandoObject();
            store.WEBAPP.DELETE.A = "32";
            store.WEBAPP.DELETE.B = "123123";
            store.WEBAPP.DELETE.C = "135135";
            store.WEBAPP.DELETE.D = "346346346";
            store.SomeProperty = "42 is life";

            store.DATABASE.MySQL.Test.__DATABASE = "localhost:8500;dzalewski;password;kidb";
            store.DATABASE.MySQL.Test.__SSH = "28h392h39ht98h9";
            store.DATABASE.MySQL.Prod.__DATABASE = "someserver:8348;prodacc;auhr;kidbprod";
            store.DATABASE.MySQL.Prod.__SSH = "837359283hg";
            store.DATABASE.Redis.Test.__DATABASE = "localhost:16379";
            store.DATABASE.Redis.Prod.__DATABASE = "localhost:22373";
            store.WEBAPP.SETTINGS.Test.__VERSION = "1.0";
            store.WEBAPP.SETTINGS.Test.__A = "1735";
            store.WEBAPP.SETTINGS.Test.__B = "1332";
            store.WEBAPP.SETTINGS.Test.__TITLE = "My Web Application";
            store.WEBAPP.SETTINGS.Prod.__VERSION = "2.0";
            store.WEBAPP.SETTINGS.Prod.__A = "32323";
            store.WEBAPP.SETTINGS.Prod.__B = "3434";
            store.WEBAPP.SETTINGS.Prod.__TITLE = "Production Applcation";

            File.WriteAllText(@"store.json", JsonConvert.SerializeObject(store));


            var store2 = (JObject)JsonConvert.DeserializeObject(File.ReadAllText(@"store.json"));

            store2.MoveProperty("DATABASE.Redis", "WEBAPP.NEW");
            store2.MoveProperty("WEBAPP.SETTINGS.Test.__VERSION", "DATABASE.MySQL.Prod");
            store2.MoveProperty("WEBAPP.SETTINGS.Prod.*", "WEBAPP.SECRET");
            store2.DeleteProperty("WEBAPP.DELETE.*");
            store2.DeleteProperty("WEBAPP.DELETE");

            //JToken t1 = store2.SelectToken("DATABASE.Redis").Parent;

            //JToken t2 = store2.SelectToken("WEBAPP.NEW");

            //if (t2.Last != null)
            //    t2.Last.AddAfterSelf(t1);
            //else
            //{
            //    // create new JObject that holds the JProperty from t1
            //    JObject newObject = new JObject
            //    {
            //        t1
            //    };

            //    JProperty p2 = t2.Parent as JProperty;
            //    p2.Value = newObject;
            //}

            //t1.Remove();

            File.WriteAllText(@"store.json", JsonConvert.SerializeObject(store2));
        }
    }
}
