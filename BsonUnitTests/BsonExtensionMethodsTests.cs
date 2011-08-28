﻿/* Copyright 2010-2011 10gen Inc.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using NUnit.Framework;

using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Options;

namespace MongoDB.BsonUnitTests {
    [TestFixture]
    public class BsonExtensionMethodsTests {
        private class C {
            public int N;
            public ObjectId Id; // deliberately not the first element
        }

        private XmlDocument xml;

        public BsonExtensionMethodsTests ()
        {
            xml = new XmlDocument();

            xml.LoadXml(@"<rootElement>
	<outerElem outerElem='hi' innerElem='blarg'>
		<![CDATA[Some CData Stuff]]>
		<![CDATA[Some Other CData Stuff]]>
		<innerElem>Inner</innerElem>
		someWords<![CDATA[A third line of CData Stuff]]>
		<innerElem2>Inner</innerElem2>
		<innerElem>Inner</innerElem>
		More Words
		<tommy>inner text<![CDATA[cdata]]></tommy>
	</outerElem>
</rootElement>");
        }

        [Test]
        public void TestToBsonEmptyDocument() {
            var document = new BsonDocument();
            var bson = document.ToBson();
            var expected = new byte[] { 5, 0, 0, 0, 0 };
            Assert.IsTrue(expected.SequenceEqual(bson));
        }

        [Test]
        public void TestToBson() {
            var c = new C { N = 1, Id = ObjectId.Empty };
            var bson = c.ToBson();
            var expected = new byte[] { 29, 0, 0, 0, 16, 78, 0, 1, 0, 0, 0, 7, 95, 105, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            Assert.IsTrue(expected.SequenceEqual(bson));
        }

        [Test]
        public void TestToBsonXml()
        {
            throw new NotImplementedException("Must write this.");
            var bson = xml.ToBson();
            var expected = new byte[] { 29, 0, 0, 0, 16, 78, 0, 1, 0, 0, 0, 7, 95, 105, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            Assert.IsTrue(expected.SequenceEqual(bson));
        }

        [Test]
        public void TestToBsonIdFirst() {
            var c = new C { N = 1, Id = ObjectId.Empty };
            var bson = c.ToBson(DocumentSerializationOptions.SerializeIdFirstInstance);
            var expected = new byte[] { 29, 0, 0, 0, 7, 95, 105, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 16, 78, 0, 1, 0, 0, 0, 0 };
            Assert.IsTrue(expected.SequenceEqual(bson));
        }

        [Test]
        public void TestToBsonDocumentEmptyDocument() {
            var empty = new BsonDocument();
            var document = empty.ToBsonDocument();
            Assert.AreEqual(0, document.ElementCount);
        }

        [Test]
        public void TestToBsonDocument() {
            var c = new C { N = 1, Id = ObjectId.Empty };
            var document = c.ToBsonDocument();
            Assert.AreEqual(2, document.ElementCount);
            Assert.AreEqual("N", document.GetElement(0).Name);
            Assert.AreEqual("_id", document.GetElement(1).Name);
            Assert.IsInstanceOf<BsonInt32>(document[0]);
            Assert.IsInstanceOf<BsonObjectId>(document[1]);
            Assert.AreEqual(1, document[0].AsInt32);
            Assert.AreEqual(ObjectId.Empty, document[1].AsObjectId);
        }

        [Test]
        public void TestToBsonDocumentIdFirst() {
            var c = new C { N = 1, Id = ObjectId.Empty };
            var document = c.ToBsonDocument(DocumentSerializationOptions.SerializeIdFirstInstance);
            Assert.AreEqual(2, document.ElementCount);
            Assert.AreEqual("_id", document.GetElement(0).Name);
            Assert.AreEqual("N", document.GetElement(1).Name);
            Assert.IsInstanceOf<BsonObjectId>(document[0]);
            Assert.IsInstanceOf<BsonInt32>(document[1]);
            Assert.AreEqual(ObjectId.Empty, document[0].AsObjectId);
            Assert.AreEqual(1, document[1].AsInt32);
        }

        [Test]
        public void TestToBsonDocumentXml()
        {
            var document = xml.ToBsonDocument();
            Assert.AreEqual(1, document.ElementCount);
            Assert.IsTrue(document.Names.Contains("rootElement"));
            Assert.AreEqual(1, document["rootElement"].AsBsonDocument.ElementCount);
            Assert.IsTrue(document["rootElement"].AsBsonDocument.Contains("outerElem"));
            Assert.AreEqual(7, document["rootElement"].AsBsonDocument["outerElem"].AsBsonDocument.ElementCount);
            Assert.IsTrue(document["rootElement"].AsBsonDocument["outerElem"].AsBsonDocument.Names.Contains("@outerElem"));
            Assert.IsTrue(document["rootElement"].AsBsonDocument["outerElem"].AsBsonDocument.Names.Contains("@innerElem"));
            Assert.IsTrue(document["rootElement"].AsBsonDocument["outerElem"].AsBsonDocument.Names.Contains("#cdata-section"));
            Assert.IsTrue(document["rootElement"].AsBsonDocument["outerElem"].AsBsonDocument.Names.Contains("#cdata-section"));
            Assert.IsTrue(document["rootElement"].AsBsonDocument["outerElem"].AsBsonDocument.Names.Contains("innerElem"));
            Assert.IsTrue(document["rootElement"].AsBsonDocument["outerElem"].AsBsonDocument.Names.Contains("#text"));
            Assert.IsTrue(document["rootElement"].AsBsonDocument["outerElem"].AsBsonDocument.Names.Contains("innerElem2"));
            Assert.IsTrue(document["rootElement"].AsBsonDocument["outerElem"].AsBsonDocument.Names.Contains("tommy"));
        }

        [Test]
        public void TestToJsonEmptyDocument() {
            var document = new BsonDocument();
            var json = document.ToJson();
            var expected = "{ }";
            Assert.AreEqual(expected, json);
        }

        [Test]
        public void TestToJson() {
            var c = new C { N = 1, Id = ObjectId.Empty };
            var json = c.ToJson();
            var expected = "{ 'N' : 1, '_id' : ObjectId('000000000000000000000000') }".Replace("'", "\"");
            Assert.AreEqual(expected, json);
        }

        [Test]
        public void TestToJsonIdFirst() {
            var c = new C { N = 1, Id = ObjectId.Empty };
            var json = c.ToJson(DocumentSerializationOptions.SerializeIdFirstInstance);
            var expected = "{ '_id' : ObjectId('000000000000000000000000'), 'N' : 1 }".Replace("'", "\"");
            Assert.AreEqual(expected, json);
        }

        [Test]
        public void TestToJsonXml()
        {
            var json = xml.ToJson();
            var expected = "{ \"rootElement\" : { \"outerElem\" : { \"@outerElem\" : \"hi\", \"@innerElem\" : \"blarg\", \"#cdata-section\" : [\"Some CData Stuff\", \"Some Other CData Stuff\", \"A third line of CData Stuff\"], \"innerElem\" : [\"Inner\", \"Inner\"], \"#text\" : [\"\\r\\n\\t\\tsomeWords\", \"\\r\\n\\t\\tMore Words\\r\\n\\t\\t\"], \"innerElem2\" : \"Inner\", \"tommy\" : { \"#text\" : \"inner text\", \"#cdata-section\" : \"cdata\" } } } }";
            Assert.AreEqual(expected, json);
        }
    }
}
