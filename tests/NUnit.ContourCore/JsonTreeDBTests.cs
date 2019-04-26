using ContourCore.database;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUnit.ContourCore
{
    [TestFixture]
    public class JsonTreeDBTests
    {
        [TestCase(typeof(MockJsonTreeDB), typeof(MockJsonTreeDBFactory))]
        [TestCase(typeof(JsonTreeDB), typeof(JsonTreeDBFactory))]
        public void FactoryReturnsCorrectType(Type ExpectedType, Type FactoryType)
        { 
            IJsonTreeDBFactory factory =Activator.CreateInstance(FactoryType) as IJsonTreeDBFactory;
            Assert.That(factory.Create("PATH"), Is.TypeOf(ExpectedType));
        }
    }
}
