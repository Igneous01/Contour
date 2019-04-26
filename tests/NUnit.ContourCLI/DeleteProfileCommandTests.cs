using ContourCLI;
using ContourCLI.Actions;
using ContourCore.database;
using NUnit.Framework;

namespace NUnit.ContourCLI
{
    [TestFixture]
    public class DeleteProfileCommandTests : AbstractDeleteCommandTests
    {
        protected DeleteProfileCommand Command;
        protected string DBPath = GlobalConfig.PROFILE;

        [SetUp]
        public void Setup()
        {
            MockJsonTreeDB.Clear();
            Command = new DeleteProfileCommand();
            Command.SetFactory(new MockJsonTreeDBFactory());
            SetupDB(DBPath);
        }

        [TestCase("DATABASE.MySQL.Test._DATABASE")]
        [TestCase("DATABASE.MySQL.Test")]
        [TestCase("WEBAPP.SETTINGS.*")]
        [TestCase("*")]
        public void DeleteProfileSuccess(string path)
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
        public void DeleteProfileInvalidJsonPathExpressionFails(string path)
        {
            Command.Path = path;
            Assert.That(Command.Execute(), Is.EqualTo(-1), "Command did not return -1 code");
        }
    }
}
