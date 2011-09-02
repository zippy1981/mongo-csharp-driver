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
using System.Linq;
using System.Text;
using System.Xml;
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
]>
<contact>
</contact>");
        }

		[Test]
		public void TestXmlAttribute() {
			throw new NotImplementedException();
		}
		
		[Test]
		public void TestXmlDocument() {
			throw new NotImplementedException();
		}
		
		[Test]
		public void TestXmlDocumentFragment() {
			throw new NotImplementedException();
		}
		
		[Test]
		public void TestXmlDocumentType() {
			throw new NotImplementedException();
		}
		
		[Test]
		public void TestXmlElement() {
			throw new NotSupportedException();
		}
		
		[Test]
		public void TestXmlNotation() {
			var document = dtd.DocumentType.Notations.Item(0).ToBsonDocument();
			Assert.AreEqual("PublicNotation", document.Names.First());
			Assert.IsInstanceOf<BsonDocument>(document["PublicNotation"]);
			Assert.AreEqual("PUBLIC", ((BsonDocument)document)["PublicNotation"]);
			
			document = dtd.DocumentType.Notations.Item(1).ToBsonDocument();
		}
	}
}

