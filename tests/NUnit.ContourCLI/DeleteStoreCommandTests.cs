using ContourCLI;
using ContourCLI.Actions;
using ContourCore.database;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUnit.ContourCLI
{
    [TestFixture]
    public class DeleteStoreCommandTests : AbstractDeleteCommandTests
    {
        protected DeleteStoreCommand Command;
        protected string DBPath = GlobalConfig.STORE;

        [SetUp]
        public void Setup()
        {
            MockJsonTreeDB.Clear();
            Command = new DeleteStoreCommand();
            Command.SetFactory(new MockJsonTreeDBFactory());
            SetupDB(DBPath);
        }

        [TestCase("DATABASE.MySQL.Test._DATABASE")]
        [TestCase("DATABASE.MySQL.Test")]
        [TestCase("WEBAPP.SETTINGS.*")]
        [TestCase("*")]
        public void DeleteStoreSuccess(string path)
        {
            Command.Path = path;
            Assert.That(Command.Execute(), Is.EqualTo(0), "Command did not return 0 code");
            Assert.That(MockJsonTreeDB._Store[DBPath].SelectTokens(path), Is.Empty.Or.Null, $"Path {path} not deleted successfully");
        }

        [TestCase("Property")]
        [TestCase("Property.Nested")]
        [TestCase("DATABASE.Nested")]
        [TestCase("DATABASE.*.MySQL")]
        [TestCase("WEBAPP.[lol]")]
        [TestCase("WEBAPP.[lol].*")]
        [TestCase("WEBAPP.\\SETTINGS\\")]
        [TestCase("WEBAPP./SETTINGS/")]
        [TestCase("WEBAPP.SETTINGS.Test._VERSION.")]
        public void DeleteStoreInvalidJsonPathExpressionFails(string path)
        {
            Command.Path = path;
            Assert.That(Command.Execute(), Is.EqualTo(-1), "Command did not return -1 code");
        }
    }
}
