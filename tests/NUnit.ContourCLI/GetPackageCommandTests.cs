using ContourCLI;
using ContourCLI.Actions;
using ContourCore.database;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUnit.ContourCLI
{
    [TestFixture]
    public class GetPackageCommandTests
    {
        protected GetPackageCommand Command;
        protected string DBPath = GlobalConfig.PACKAGE;

        [SetUp]
        public void Setup()
        {
            MockJsonTreeDB.Clear();
            Command = new GetPackageCommand();
            Command.SetFactory(new MockJsonTreeDBFactory());

            dynamic store = new ExpandoObject();
            MockJsonTreeDB._Store[DBPath] = (JObject)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(store));
        }

        [Test]
        public void GetPackageNoResults()
        {
            // TODO: Add your test code here
            Command.Path = "Unknown";
            Assert.That(Command.Execute(), Is.EqualTo(0), "Command did not return 0 code");
        }
    }
}
