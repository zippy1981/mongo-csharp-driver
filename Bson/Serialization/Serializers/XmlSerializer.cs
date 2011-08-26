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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;

namespace MongoDB.Bson.Serialization.Serializers {
    
	/// <summary>
    /// Represents a serializer for <see cref="XmlAttribute"/>s.
    /// </summary>
    public class XmlAttributeSerializer : XmlNodeSerializer {
	}
	
	/// <summary>
    /// Represents a serializer for <see cref="XmlDocument"/>s.
    /// </summary>
    public class XmlDocumentSerializer : XmlNodeSerializer {
	}
	
	/// <summary>
    /// Represents a serializer for <see cref="XmlDocumentFragment"/>s.
    /// </summary>
    public class XmlDocumentFragmentSerializer : XmlNodeSerializer {
	}
	
	/// <summary>
    /// Represents a serializer for <see cref="XmlEntity"/>s.
    /// </summary>
    public class XmlEntitySerializer : XmlNodeSerializer {
	}
	
	/// <summary>
    /// Represents a serializer for <see cref="XmlLinkedNode"/>s.
    /// </summary>
    public class XmlLinkedNodeSerializer : XmlNodeSerializer {
	}
	
	/// <summary>
    /// Represents a serializer for <see cref="XmlNode"/>s.
    /// </summary>
    public abstract class XmlNodeSerializer : IBsonSerializer {
		#region IBsonSerializer implementation
		public object Deserialize (BsonReader bsonReader, Type nominalType, IBsonSerializationOptions options)
		{
			throw new NotImplementedException ();
		}

		public object Deserialize (BsonReader bsonReader, Type nominalType, Type actualType, IBsonSerializationOptions options)
		{
			throw new NotImplementedException ();
		}

		public bool GetDocumentId (object document, out object id, out Type idNominalType, out IIdGenerator idGenerator)
		{
			throw new NotImplementedException ();
		}

		public void Serialize (BsonWriter bsonWriter, Type nominalType, object value, IBsonSerializationOptions options)
		{
			throw new NotImplementedException ();
		}

		public void SetDocumentId (object document, object id)
		{
			// TODO: perhaps we can support this
			throw new NotSupportedException();
		}
		#endregion		
	}
	
	/// <summary>
    /// Represents a serializer for <see cref="XmlNotation"/>s.
    /// </summary>
    public class XmlNotationSerializer : XmlNodeSerializer {
	}
}

