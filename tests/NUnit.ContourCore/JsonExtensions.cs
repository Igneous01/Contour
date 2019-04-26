// NUnit 3 tests
// See documentation : https://github.com/nunit/docs/wiki/NUnit-Documentation
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using System.Dynamic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using ContourCore;
using System;
using System.Linq;

namespace NUnit.ContourCore
{
    [TestFixture]
    public class JsonExtensions
    {
        private static JObject jsonStore;

        [SetUp]
        public void Setup()
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
            store.WEBAPP.NEW = new ExpandoObject();
            store.WEBAPP.SECRET = new ExpandoObject();
            store.WEBAPP.DELETE = new ExpandoObject();
            store.WEBAPP.DELETE.A = "32";
            store.WEBAPP.DELETE.B = "123123";
            store.WEBAPP.DELETE.C = "135135";
            store.WEBAPP.DELETE.D = "346346346";
            store.WEBAPP.DELETE.NESTED = new ExpandoObject();
            store.WEBAPP.DELETE.NESTED.A = 22;
            store.WEBAPP.DELETE.NESTED.B = 44;
            store.SomeProperty = "42 is life";
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

            jsonStore = (JObject)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(store));
        }

        [TestCase("DATABASE.MySQL.Test._DATABASE", "", "_DATABASE")]
        [TestCase("DATABASE.Redis.Prod._DATABASE", "WEBAPP", "WEBAPP._DATABASE")]
        [TestCase("WEBAPP.SETTINGS.Test._VERSION", "DATABASE.MySQL.Test", "DATABASE.MySQL.Test._VERSION")]
        [TestCase("WEBAPP.SETTINGS.Test._VERSION", "WEBAPP.NEW", "WEBAPP.NEW._VERSION")]
        [TestCase("SomeProperty", "WEBAPP.SETTINGS.Prod", "WEBAPP.SETTINGS.Prod.SomeProperty")]
        [TestCase("DATABASE.Redis", "WEBAPP.NEW", "WEBAPP.NEW.Redis")]
        public void MoveSinglePropertySuccess(string source, string dest, string newPath)
        {
            jsonStore.MoveProperty(source, dest);
            Assert.That(jsonStore.SelectToken(source), Is.Null, $"property still exists at path {source} after move");
            Assert.That(jsonStore.SelectToken(newPath), Is.Not.Null, $"property does not exist at path {newPath} after move");
        }

        [TestCase("DATABASE.MySQL.*", "", new string[] { "Test._DATABASE", "Test._SSH", "Prod._DATABASE", "Prod._SSH" })]
        [TestCase("DATABASE.Redis.*", "WEBAPP.NEW", new string[] { "WEBAPP.NEW.Test._DATABASE", "WEBAPP.NEW.Prod._DATABASE" })]
        [TestCase("DATABASE.*", "WEBAPP.SETTINGS", new string[] { "WEBAPP.SETTINGS.MySQL.Test._DATABASE", "WEBAPP.SETTINGS.MySQL.Test._SSH", "WEBAPP.SETTINGS.MySQL.Prod._DATABASE", "WEBAPP.SETTINGS.MySQL.Prod._SSH", "WEBAPP.SETTINGS.Redis.Test._DATABASE", "WEBAPP.SETTINGS.Redis.Prod._DATABASE" })]
        public void MoveMultiplePropertiesSuccess(string source, string dest, string[] newPaths)
        {
            jsonStore.MoveProperty(source, dest);
            Assert.That(jsonStore.SelectTokens(source), Is.Null.Or.Empty, $"property still exists at path {source} after move");

            foreach (string p in newPaths)
                Assert.That(jsonStore.SelectToken(p), Is.Not.Null, $"property does not exist at path {p} after move");
        }

        [TestCase("DATABASE.MySQL.Test.*", "DATABASE.MySQL.Prod")]
        public void MoveSinglePropertyFail(string source, string dest)
        {
            Assert.Throws<System.ArgumentException>(() => jsonStore.MoveProperty(source, dest));
        }

        [TestCase("DATABASE.MySQL.Test._DATABASE")]
        [TestCase("DATABASE.Redis.Prod._DATABASE")]
        [TestCase("WEBAPP.SETTINGS.Test._VERSION")]
        [TestCase("WEBAPP.SETTINGS.Test._VERSION")]
        [TestCase("SomeProperty")]
        [TestCase("DATABASE.Redis")]
        [TestCase("WEBAPP.DELETE")]
        public void DeleteSinglePropertySuccess(string source)
        {
            jsonStore.DeleteProperty(source);
            Assert.That(jsonStore.SelectToken(source), Is.Null, $"property still exists at path {source} after delete");
        }

        [TestCase("DATABASE.*")]
        [TestCase("DATABASE.MySQL.Test.*")]
        [TestCase("WEBAPP.SETTINGS.Test.*")]
        public void DeleteMultiplePropertiesSuccess(string source)
        {
            jsonStore.DeleteProperty(source);
            Assert.That(jsonStore.SelectTokens(source), Is.Null.Or.Empty, $"property still exists at path {source} after delete");
        }

        [TestCase("COOL.NEW.DIRECTORY._VAL", "42")]
        [TestCase("DATABASE.MySQL.Test.Nested.SomeVal", "My new value")]
        [TestCase("BRAND.NEW.SETTINGS.Test.SomeConfigVal", "55")]
        [TestCase("WEBAPP.SETTINGS.Test.NewValue", "localhost:4200")]
        public void CreatePropertySuccess(string path, string value)
        {
            jsonStore.CreateProperty(path, value);
            JToken result = jsonStore.SelectToken(path);
            Assert.That(result, Is.Not.Null, $"property does not exist at path {path} after create");
            JToken val = (result.Parent as JProperty).Value;
            Assert.That(val, Is.EqualTo(JToken.FromObject(value)), $"property {path} value {val} does not equal {value}");
        }

        [TestCase("DATABASE.MySQL.Test", "42")]
        [TestCase("WEBAPP.SETTINGS.Test._VERSION.NEW.INNER", "My new value")]
        public void CreatePropertyUpdateExistingSuccess(string path, string value)
        {
            jsonStore.CreateProperty(path, value);
            JToken result = jsonStore.SelectToken(path);
            Assert.That(result, Is.Not.Null, $"property does not exist at path {path} after create");
            JToken val = (result.Parent as JProperty).Value;
            Assert.That(val, Is.EqualTo(JToken.FromObject(value)), $"property {path} value {val} does not equal {value}");
        }

        [TestCase("COOL.*.DIRECTORY._VAL")]
        [TestCase("DATABASE.[MySQL].Test.Nested.SomeVal")]
        [TestCase("\\.NEW.Test.SomeConfigVal")]
        [TestCase("WEBAPP./.Test.NewValue")]
        [TestCase("WEBAPP.New.")]
        public void CreatePropertyFailure(string path)
        {
            Assert.Throws<ArgumentException>(() => jsonStore.CreateProperty(path, 55));
        }

        [Test]
        public void FindAllChildProperties()
        {
            ICollection<JToken> results = jsonStore.FindAllProperties().ToList();
            Assert.That(results.Count, Is.EqualTo(21));
        }

        [TestCase("DATABASE", 6)]
        [TestCase("DATABASE.MySQL.Test.*", 2)]
        [TestCase("WEBAPP.SETTINGS.Test.*", 4)]
        [TestCase("WEBAPP.*", 14)]
        public void FindAllChildProperties(string path, int count)
        {
            ICollection<JToken> results = jsonStore.FindAllProperties(path).ToList();
            Assert.That(results.Count, Is.EqualTo(count));
        }
    }
}
