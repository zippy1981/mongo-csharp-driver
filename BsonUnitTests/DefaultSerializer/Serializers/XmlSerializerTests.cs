/* Copyright 2010-2011 10gen Inc.
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
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using MongoDB.Bson.Serialization.Options;
using NUnit.Framework;

using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace MongoDB.BsonUnitTests
{
	[TestFixture]
	public class XmlSerializerTests
	{
		
		private XmlDocument xml;
		private XmlDocument dtd;	
        
		public XmlSerializerTests ()
        {
            xml = new XmlDocument();

            xml.LoadXml(@"<rootElement>
	<outerElem outerElem='hi' innerElem='blarg'>
		<![CDATA[Some CData Stuff]]>
        <!-- A comment -->
		<![CDATA[Some Other CData Stuff]]>
		<innerElem>Inner</innerElem>
		someWords<![CDATA[A third line of CData Stuff]]>
		<innerElem2>Inner</innerElem2>
		<innerElem>Inner</innerElem>
		More Words
		<!-- Comment the sequel -->
        <tommy>inner text<![CDATA[cdata]]></tommy>
	</outerElem>
</rootElement>");
			dtd = new XmlDocument();
			dtd.LoadXml(@"<!DOCTYPE contact[
	<!NOTATION PublicNotation PUBLIC 'PublicId' >
	<!NOTATION SystemNotation SYSTEM 'SystemId' >
    <!ENTITY entityName 'Entity Value'>
]>
<contact>
</contact>");
        }

		[Test]
		public void TestXmlAttribute() {
			var xml = new XmlDocument();
		    var attr = xml.CreateAttribute("elemAttribute");
		    attr.Value = "attrVal";
            var document = attr.ToBsonDocument();
            Assert.AreEqual("attrVal", document["@elemAttribute"].AsString);

            Assert.Ignore("TODO: Get Deserialization working");
		}
		
		[Test]
		public void TestXmlDocument() {
            throw new NotImplementedException();

            Assert.Ignore("TODO: Get Deserialization working");
		}
		
		[Test]
		public void TestXmlDocumentFragment() {
            throw new NotImplementedException();

            Assert.Ignore("TODO: Get Deserialization working");
		}
		
		[Test]
		public void TestXmlDocumentType() {
            var document = dtd.DocumentType.ToBsonDocument();
            Assert.AreEqual(1, document.ElementCount);
            Assert.IsInstanceOf<BsonNull>(document["contact"]);

            // Ignore XmlSerializationOptions.SerializeDocType when directly calling serialize on DocType
            document = dtd.DocumentType.ToBsonDocument(new XmlSerializationOptions { SerializeDocType = false});
            Assert.AreEqual(1, document.ElementCount);
            Assert.IsInstanceOf<BsonNull>(document["contact"]);

            document = dtd.DocumentType.ToBsonDocument(new XmlSerializationOptions { SerializeDtdElements = true });
            Assert.AreEqual(1, document.ElementCount);
            Assert.IsInstanceOf<BsonDocument>(document["contact"]);
            Assert.AreEqual(3, ((BsonDocument)document["contact"]).ElementCount);

            Assert.Ignore("TODO: Get Deserialization working");
            
		}
		
		[Test]
		public void TestXmlElement() {
            throw new NotImplementedException();

            Assert.Ignore("TODO: Get Deserialization working");
		}
		
		[Test]
		public void TestXmlNotation() {
			// Public notation
            var documentPublic = dtd.DocumentType.Notations.GetNamedItem("PublicNotation").ToBsonDocument();
            Assert.AreEqual("PublicNotation", documentPublic.Names.First());
            Assert.IsInstanceOf<BsonDocument>(documentPublic["PublicNotation"]);
            Assert.AreEqual("PublicId", ((BsonDocument)documentPublic["PublicNotation"])["PUBLIC"].AsString);

            // System Notation
            var documentSystem = dtd.DocumentType.Notations.GetNamedItem("SystemNotation").ToBsonDocument();
            Assert.AreEqual("SystemNotation", documentSystem.Names.First());
            Assert.IsInstanceOf<BsonDocument>(documentSystem["SystemNotation"]);
            Assert.AreEqual("SystemId", ((BsonDocument)documentSystem["SystemNotation"])["SYSTEM"].AsString);

            Assert.Ignore("TODO: Get Deserialization working");

            var bsonPublic = documentPublic.ToBson();
            var rehydratedPublic = BsonSerializer.Deserialize<XmlDocumentType>(bsonPublic);
            Assert.IsTrue(bsonPublic.SequenceEqual(rehydratedPublic.ToBson()));

            var bsonSystem = documentPublic.ToBson();
            var rehydratedSystem = BsonSerializer.Deserialize<XmlDocumentType>(bsonSystem);
            Assert.IsTrue(bsonPublic.SequenceEqual(rehydratedSystem.ToBson()));
        }
	}
}

