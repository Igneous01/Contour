using ContourCore.database;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace NUnit.ContourCLI
{
    public abstract class AbstractDeleteCommandTests
    {
        protected void SetupDB(string DBPath)
        {
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
            store.DATABASE.MySQL.Test._DATABASE = "localhost:8500;dzalewski;password;kidb";
            store.DATABASE.MySQL.Test._SSH = "28h392h39ht98h9";
            store.DATABASE.MySQL.Prod._DATABASE = "someserver:8348;prodacc;auhr;kidbprod";
            store.DATABASE.MySQL.Prod._SSH = "837359283hg";
            store.DATABASE.Redis.Test._DATABASE = "localhost:16379";
            store.DATABASE.Redis.Prod._DATABASE = "localhost:22373";
            store.WEBAPP.SETTINGS.Test._VERSION = "1.0";
            store.WEBAPP.SETTINGS.Test._A = "1735";
            store.WEBAPP.SETTINGS.Test._B = "1332";
            store.WEBAPP.SETTINGS.Test._TITLE = "My Web Application";
            store.WEBAPP.SETTINGS.Prod._VERSION = "2.0";
            store.WEBAPP.SETTINGS.Prod._A = "32323";
            store.WEBAPP.SETTINGS.Prod._B = "3434";
            store.WEBAPP.SETTINGS.Prod._TITLE = "Production Applcation";
            MockJsonTreeDB._Store[DBPath] = (JObject)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(store));
        }
    }
}
