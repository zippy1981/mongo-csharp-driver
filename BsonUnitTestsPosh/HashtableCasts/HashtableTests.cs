using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using MongoDB.Bson;
using NUnit.Framework;


namespace MonogoDB.BsonUnitTestsPosh.HashtableCasts
{
    [TestFixture]
    public sealed class HashtableTests : PoshTestsBase
    {
        [Test]
        public void TestHashTable()
        {
            const string script = @"
[MongoDB.Bson.BsonDocument] @{
	Name = 'Justin Dearing'
	EmailAddresses = 'zippy1981@gmail.com','justin@mongodb.org'
    PhoneNumber = '718-555-1212'

};
";
            var results = RunScript(script);
            Assert.AreEqual(3, results.Count, "Expected three result sets");
            Assert.IsTrue(results.Contains(new PSObject(new BsonElement("Name", "Justin Dearing"))));
            Assert.IsTrue(results.Contains(new PSObject(new BsonElement("PhoneNumber", "718-555-1212"))));
        }

        [Test]
        public void TestOrdered()
        {
            const string script = @"
[MongoDB.Bson.BsonDocument][ordered] @{
	Name = 'Justin Dearing'
	EmailAddresses = 'zippy1981@gmail.com','justin@mongodb.org'
    PhoneNumber = '718-555-1212'

};
";
            var results = RunScript(script);
            Assert.AreEqual(3, results.Count, "Expected three result sets");
            Assert.AreEqual(new PSObject(new BsonElement("Name", "Justin Dearing")), results[0]);
            Assert.AreEqual(new PSObject(new BsonElement("EmailAddresses", new BsonArray(new [] {"zippy1981@gmail.com","justin@mongodb.org"}))), results[1]);
            Assert.AreEqual(new BsonElement("PhoneNumber", "718-555-1212"), results[2].BaseObject);
        }
    }
}
