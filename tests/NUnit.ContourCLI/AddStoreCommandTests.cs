// NUnit 3 tests
// See documentation : https://github.com/nunit/docs/wiki/NUnit-Documentation
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using ContourCLI.Actions;
using ContourCore.database;
using Newtonsoft.Json.Linq;
using System;
using ContourCLI;

namespace NUnit.ContourCLI
{
    [TestFixture]
    public class AddStoreCommandTests
    {
        protected AddStoreCommand Command;

        [SetUp]
        public void Setup()
        {
            MockJsonTreeDB.Clear();
            Command = new AddStoreCommand();
            Command.SetFactory(new MockJsonTreeDBFactory());
        }

        [TestCase("Property", "Value")]
        [TestCase("Property.Nested", "SomeValue.Cool")]
        [TestCase("Property.Nested.Inner", "Value")]
        [TestCase("Property.Nested.Deeper.Layer.Inner.Finer.More", "432.76135")]
        public void AddStoreSuccess(string path, string value)
        {
            Command.Path = path;
            Command.Value = value;
            Assert.That(Command.Execute(), Is.EqualTo(0), "Command did not return 0 code");
            AssertJsonTokenValueEqual(MockJsonTreeDB._Store[GlobalConfig.STORE], path, value);
        }


        [TestCase("Property.*", "Value")]
        [TestCase("Property.Nested.*", "SomeValue.Cool")]
        [TestCase("Property.Nested.", "SomeValue")]
        [TestCase("Property.*.Nested.Inner", "Value")]
        [TestCase("Property.[lol].Nested", "432.76135")]
        public void AddStoreInvalidJsonPathExpressionFails(string path, string value)
        {
            Command.Path = path;
            Command.Value = value;
            Assert.That(Command.Execute(), Is.EqualTo(-1));
        }

        [TestCase("Property.Nested", "New Value")]
        [TestCase("Property.Nested.Inner.Deeper", "SomeValue.Cool")]
        [TestCase("Property.Nested.Inner.Layer.NewForm", "SomeValue.Cool")]
        [TestCase("Property.NewPath.NewValue", "New Value")]
        public void AddStoreUpdateExistingStorePropertySuccess(string path, string value)
        {
            JObject j = new JObject
            {
                { "Nested", "Old Value" },
                { "Inner", new JObject
                        {
                            { "Deeper", "Older Value" },
                            { "Layer", "24.323" }
                        }
                }
            };
            MockJsonTreeDB._Store.Add("Property", j);

            Command.Path = path;
            Command.Value = value;
            Assert.That(Command.Execute(), Is.EqualTo(0), "Command did not return 0 code");
            AssertJsonTokenValueEqual(MockJsonTreeDB._Store[GlobalConfig.STORE], path, value);
        }

        [TestCase("Property.Inner", "Cool")]
        [TestCase("Property.Inner.Layer", "New Value")]
        public void AddStoreUpdateExistingStorePropertyOverwriteObjectToValue(string path, string value)
        {
            JObject j = new JObject
            {
                { "Nested", "Old Value" },
                { "Inner",
                  new JObject
                    {
                        { "Deeper", "Older Value" },
                        {
                            "Layer",
                            new JObject { {"More", "23" }, {"Value", "45"} }
                        }
                    }
                }
            };
            MockJsonTreeDB._Store.Add("Property", j);

            Command.Path = path;
            Command.Value = value;
            Assert.That(Command.Execute(), Is.EqualTo(0), "Command did not return 0 code");
            AssertJsonTokenValueEqual(MockJsonTreeDB._Store[GlobalConfig.STORE], path, value);
        }


        private void AssertJsonTokenValueEqual(JObject json, string path, string value)
        {
            if (json == null) Assert.Fail($"JObject is null");
            JToken jToken = json.SelectToken(path);
            if (jToken == null || jToken.Parent == null || !(jToken.Parent is JProperty)) Assert.Fail($"{path} JToken is null, JToken.Parent is null, or JToken is not a JProperty");
            string jTokenValue = (jToken.Parent as JProperty).Value.ToString();
            Assert.That(jTokenValue.Equals(value), Is.True, $"JSON property {path} has value {jTokenValue} that does not match expected {value}");
        }
    }
}
