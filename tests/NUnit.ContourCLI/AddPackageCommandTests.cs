using ContourCLI;
using ContourCLI.Actions;
using ContourCore.database;
using ContourCore.extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Dynamic;

namespace NUnit.ContourCLI
{
    [TestFixture]
    public class AddPackageCommandTests
    {
        protected AddPackageCommand Command;

        [SetUp]
        public void Setup()
        {
            MockJsonTreeDB.Clear();
            SetupDBStore();        
            Command = new AddPackageCommand();
            Command.SetFactory(new MockJsonTreeDBFactory());
        }

        [TestCase("Package", new string[] { "DATABASE.MySQL.Test" })]
        [TestCase("Package", new string[] { "DATABASE.MySQL.Test", "DATABASE.Redis.Test" })]
        [TestCase("Package", new string[] { "DATABASE.MySQL.Test", "DATABASE.Redis.Test", "WEBAPP.SETTINGS.Test.*" })]
        [TestCase("Packages.Nested.MyPackage", new string[] { "DATABASE.MySQL.Test", "DATABASE.Redis.Test", "WEBAPP.SETTINGS.Test.*" })]
        public void AddPackageSuccess(string path, IEnumerable<string> value)
        {
            Command.Path = path;
            Command.Value = value;
            Assert.That(Command.Execute(), Is.EqualTo(0), "Command did not return 0 code");
            AssertJsonTokenValueEqual(MockJsonTreeDB._Store[GlobalConfig.PACKAGE], path, value);
        }

        [TestCase("Package.*")]
        [TestCase("Package.")]
        [TestCase("Package.[awe]")]
        [TestCase("Packages.*.Nested.MyPackage")]
        public void AddPackageInvalidJsonPathExpressionFails(string path)
        {
            Command.Path = path;
            Command.Value = new string[] { "DATABASE.MySQL.Test" };
            Assert.That(Command.Execute(), Is.EqualTo(-1));
        }

        [TestCase("Package", new string[] { "DATABASE.MySQL.Test" })]
        public void AddPackageUpdateExistingPackageSuccess(string path, IEnumerable<string> value)
        {
            IEnumerable<string> mockValue = new string[] { "Base.*" };
            MockJsonTreeDB._Store[GlobalConfig.PACKAGE] = new JObject
            {
                {"Package", JToken.FromObject(mockValue) },
                {"Package2", JToken.FromObject(mockValue) }
            };
            Command.Path = path;
            Command.Value = value;
            Assert.That(Command.Execute(), Is.EqualTo(0), "Command did not return 0 code");
            AssertJsonTokenValueEqual(MockJsonTreeDB._Store[GlobalConfig.PACKAGE], path, value);
        }

        [TestCase("Package", new string[] { "DATABASE.MySQL.Test" })]
        public void AddPackageUpdateExistingPackageNoWeirdSideEffectsSuccess(string path, IEnumerable<string> value)
        {
            IEnumerable<string> mockValue = new string[] { "Base.*" };
            MockJsonTreeDB._Store[GlobalConfig.PACKAGE] = new JObject
            {
                {"Package", JToken.FromObject(mockValue) },
                {"Package2", JToken.FromObject(mockValue) }
            };
            Command.Path = path;
            Command.Value = value;
            Assert.That(Command.Execute(), Is.EqualTo(0), "Command did not return 0 code");
            AssertJsonTokenValueEqual(MockJsonTreeDB._Store[GlobalConfig.PACKAGE], "Package2", mockValue); // assert that Package2 has not been overwritten by strange side effects
        }

        private void AssertJsonTokenValueEqual(JObject json, string path, IEnumerable<string> values)
        {
            if (json == null) Assert.Fail($"JObject is null");
            JToken jToken = json.SelectToken(path);
            if (jToken == null || jToken.Parent == null || !(jToken.Parent is JProperty)) Assert.Fail($"{path} JToken is null, JToken.Parent is null, or JToken is not a JProperty");
            IEnumerable<string> jTokenValues = (jToken.Parent as JProperty).Value.ToObject<IEnumerable<string>>();
            Assert.That(jTokenValues, Is.EquivalentTo(values), $"JSON property {path} has value {jTokenValues.ToDelimitedString()} that does not match expected {values.ToDelimitedString()}");
        }

        private void SetupDBStore()
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
            MockJsonTreeDB._Store[GlobalConfig.STORE] = (JObject)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(store));
        }
    }
}
