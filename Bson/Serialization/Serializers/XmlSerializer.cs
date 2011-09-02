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
using MongoDB.Bson.Serialization.Options;

namespace MongoDB.Bson.Serialization.Serializers {

    /// <summary>
    /// Represents a serializer for <see cref="XmlAttribute"/>s.
    /// </summary>
    public sealed class XmlAttributeSerializer : XmlNodeSerializer
    {

        #region private static fields
        private static readonly XmlAttributeSerializer instance = new XmlAttributeSerializer();
        #endregion

        #region public static properties
        /// <summary>
        /// Gets an instance of the XmlAttributeSerializer class.
        /// </summary>
        public static XmlAttributeSerializer Instance
        {
            get { return instance; }
        }
        #endregion

        #region public members
        public override void Serialize(BsonWriter bsonWriter, Type nominalType, object value, IBsonSerializationOptions options)
        {
            var attribute = (value as XmlAttribute);
            bsonWriter.WriteString(string.Format("@{0}", attribute.Name), attribute.Value);
        }
        #endregion

    }

    /// <summary>
    /// Represents a serializer for <see cref="XmlCharacterData"/>s.
    /// </summary>
    /// <remarks>
    /// We ignore <see cref="XmlWhitespace"/> and <see cref="XmlSignificantWhitespace"/>
    /// because there is no point in serializing them.
    /// </remarks>
    /// <seealso cref="XmlCDataSection"/>
    /// <seealso cref="XmlComment"/>
    /// <seealso cref="XmlText"/>
    public sealed class XmlCharacterDataSerializer : XmlNodeSerializer
    {

        #region private static fields
        private static readonly XmlCharacterDataSerializer instance = new XmlCharacterDataSerializer();
        #endregion

        #region public static properties
        /// <summary>
        /// Gets an instance of the XmlCDataSectionSerializer class.
        /// </summary>
        public static XmlCharacterDataSerializer Instance
        {
            get { return instance; }
        }
        #endregion

        #region public members
        public override void Serialize(BsonWriter bsonWriter, Type nominalType, object value, IBsonSerializationOptions options)
        {
            var characterData = (value as XmlCharacterData);
            if (value is XmlSignificantWhitespace || value is XmlSpace)
            {
                return;
            }
            var xmlSerializationOptions = options as XmlSerializationOptions;
            if (characterData.GetType()  is XmlComment && xmlSerializationOptions == null || !xmlSerializationOptions.SerializeComments)
            {
                return;
            }
            bsonWriter.WriteString(characterData.Name, characterData.Value);
        }
        #endregion

    }
	
	/// <summary>
    /// Represents a serializer for <see cref="XmlDocument"/>s.
    /// </summary>
    public sealed class XmlDocumentSerializer : XmlNodeSerializer {

        #region private static fields
        private static readonly XmlDocumentSerializer instance = new XmlDocumentSerializer();
        #endregion

        #region public static properties
        /// <summary>
        /// Gets an instance of the XmlDocumentSerializer class.
        /// </summary>
        public static XmlDocumentSerializer Instance
        {
            get { return instance; }
        }
        #endregion

        #region public members
        public override void Serialize(BsonWriter bsonWriter, Type nominalType, object value, IBsonSerializationOptions options)
        {
            var rootElem = (value as XmlDocument).DocumentElement;
            bsonWriter.WriteStartDocument();
            SerializeXmlElement(bsonWriter, rootElem, options as XmlSerializationOptions);
            bsonWriter.WriteEndDocument();
        }
        #endregion

    }

    /// <summary>
    /// Represents a serializer for <see cref="XmlDocumentFragment"/>s.
    /// </summary>
    public sealed class XmlDocumentFragmentSerializer : XmlNodeSerializer
    {

        #region private static fields
        private static readonly XmlDocumentFragmentSerializer instance = new XmlDocumentFragmentSerializer();
        #endregion

        #region public static properties
        /// <summary>
        /// Gets an instance of the XmlDocumentFragmentSerializer class.
        /// </summary>
        public static XmlDocumentFragmentSerializer Instance
        {
            get { return instance; }
        }
        #endregion

        #region public members
        public override void Serialize(BsonWriter bsonWriter, Type nominalType, object value, IBsonSerializationOptions options)
        {
            throw new NotImplementedException("");
        }
        #endregion

    }

    /// <summary>
    /// Represents a serializer for <see cref="XmlDocumentType"/>s.
    /// </summary>
    public sealed class XmlDocumentTypeSerializer : XmlNodeSerializer
    {

        #region private static fields
        private static readonly XmlDocumentTypeSerializer instance = new XmlDocumentTypeSerializer();
        #endregion

        #region public static properties
        /// <summary>
        /// Gets an instance of the XmlDocumentTypeSerializer class.
        /// </summary>
        public static XmlDocumentTypeSerializer Instance
        {
            get { return instance; }
        }
        #endregion

        #region public members
        public override void Serialize(BsonWriter bsonWriter, Type nominalType, object value, IBsonSerializationOptions options)
        {
            throw new NotImplementedException("");
        }
        #endregion

    }

    /// <summary>
    /// Represents a serializer for <see cref="XmlEntity"/>s.
    /// </summary>
    public sealed class XmlElementSerializer : XmlNodeSerializer
    {

        #region private static fields
        private static readonly XmlElementSerializer instance = new XmlElementSerializer();
        #endregion

        #region public static properties
        /// <summary>
        /// Gets an instance of the XmlEntitySerializer class.
        /// </summary>
        public static XmlElementSerializer Instance
        {
            get { return instance; }
        }
        #endregion

        #region public members
        public override void Serialize(BsonWriter bsonWriter, Type nominalType, object value, IBsonSerializationOptions options)
        {
            var elem = value as XmlElement;
            SerializeXmlElement(bsonWriter, elem, options as XmlSerializationOptions);
        }
        #endregion

    }

    /// <summary>
    /// Represents a serializer for <see cref="XmlEntity"/>s.
    /// </summary>
    public sealed class XmlEntitySerializer : XmlNodeSerializer
    {

        #region private static fields
        private static readonly XmlEntitySerializer instance = new XmlEntitySerializer();
        #endregion

        #region public static properties
        /// <summary>
        /// Gets an instance of the XmlEntitySerializer class.
        /// </summary>
        public static XmlEntitySerializer Instance
        {
            get { return instance; }
        }
        #endregion

        #region public members
        public override void Serialize(BsonWriter bsonWriter, Type nominalType, object value, IBsonSerializationOptions options)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
	
	/// <summary>
    /// Represents a serializer for <see cref="XmlNode"/>s.
    /// </summary>
    public abstract class XmlNodeSerializer : IBsonSerializer {
        
        #region public members
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
			// TODO: perhaps we can support this
			throw new NotSupportedException();
		}

        public abstract void Serialize(BsonWriter bsonWriter, Type nominalType, object value, IBsonSerializationOptions options);

        protected static void SerializeXmlElement(BsonWriter bsonWriter, XmlElement elem, XmlSerializationOptions options)
        {
            // Special case
            if (!elem.HasAttributes && elem.HasChildNodes && elem.ChildNodes.Count == 1 && typeof(XmlText) == elem.ChildNodes[0].GetType())
            {
                bsonWriter.WriteString(elem.Name, elem.ChildNodes[0].Value);
                return;
            }

            bsonWriter.WriteName(elem.Name);
            SerializeXmlElementChildNodes(bsonWriter, elem, options);
            bsonWriter.WriteEndDocument();
        }

	    private static void SerializeXmlElementChildNodes(BsonWriter bsonWriter, XmlElement elem, XmlSerializationOptions options)
	    {
            // Special case
            if (!elem.HasAttributes && elem.HasChildNodes && elem.ChildNodes.Count == 1 && typeof(XmlText) == elem.ChildNodes[0].GetType()) {
                bsonWriter.WriteString(elem.ChildNodes[0].Value);
            }

            else if (elem.HasAttributes || elem.HasChildNodes)
	        {
	            bsonWriter.WriteStartDocument();

	            foreach (XmlAttribute attrbute in elem.Attributes)
	            {
	                //TODO: Consider strongly typing if the schema allows it.
	                bsonWriter.WriteString(string.Format("@{0}", attrbute.Name), attrbute.Value);
	            }

	            var serializedElements = new List<string>();
                var serializedCharacterData = new List<string>();

	            foreach (XmlNode node in elem.ChildNodes) {

                    if (node is XmlElement) {
                        if (serializedElements.Contains(node.Name))
                        {
                            continue;
                        }
                        serializedElements.Add(node.Name);
                        var nodes = (from childElements in elem.ChildNodes.OfType<XmlElement>()
                                     where childElements.Name == node.Name
                                     select childElements).ToArray();
                        if (nodes.Length > 1)
                        {
                            bsonWriter.WriteName(node.Name);
                            bsonWriter.WriteStartArray();
                            foreach (var sameNamedNode in nodes)
                            {
                                SerializeXmlElementChildNodes(bsonWriter, sameNamedNode, options);
                            }
                            bsonWriter.WriteEndArray();
                        }
                        else
                        {
                            XmlElementSerializer.Instance.Serialize(bsonWriter, typeof(XmlElement), node, options);
                        }
                    }
                    else if (node is XmlCharacterData)
                    {
                        if (node is XmlComment && (options == null || !options.SerializeComments))
                        {
                            continue;
                        }
                        if (node is XmlWhitespace || node is XmlSignificantWhitespace)
                        {
                            continue;
                        }
                        if (serializedCharacterData.Contains(node.Name))
                        {
                            continue;
                        }
                        serializedCharacterData.Add(node.Name);
                        var nodes =
                            (from characterData in elem.ChildNodes.OfType<XmlCharacterData>() where characterData.Name == node.Name select characterData).ToArray();
                        if (nodes.Length > 1) {
                            bsonWriter.WriteName(node.Name);
                            bsonWriter.WriteStartArray();
                            foreach (var characterData in nodes)
                            {
                                bsonWriter.WriteString(characterData.Value);
                            }
                            bsonWriter.WriteEndArray();
                        }
                        else {
                            bsonWriter.WriteString(node.Name, node.Value);
                        }
                    }
                    else {
                        // When I see this I fix it
                        throw new NotImplementedException(string.Format("XmlNodes of type {0} cannot be serialized. Element: {1}", node.GetType(), node.OuterXml));
                    }
	            }
	        }
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
    public sealed class XmlNotationSerializer : XmlNodeSerializer
    {

        #region private static fields
        private static readonly XmlNotationSerializer instance = new XmlNotationSerializer();
        #endregion

        #region public static properties
        /// <summary>
        /// Gets an instance of the XmlNotationSerializer class.
        /// </summary>
        public static XmlNotationSerializer Instance
        {
            get { return instance; }
        }
        #endregion

        #region public members
        public override void Serialize(BsonWriter bsonWriter, Type nominalType, object value, IBsonSerializationOptions options)
        {
            bool mustClose = false;
            if (bsonWriter.State == BsonWriterState.Initial) {
                bsonWriter.WriteStartDocument();
                mustClose = true;
            }
            var xmlNotation = value as XmlNotation;
            bsonWriter.WriteName(xmlNotation.Name);
            if (xmlNotation.SystemId != null) {
                bsonWriter.WriteStartDocument();
                bsonWriter.WriteString("SYSTEM", xmlNotation.SystemId);
                bsonWriter.WriteEndDocument();
            }
            else if (xmlNotation.PublicId != null) {
                bsonWriter.WriteStartDocument();
                bsonWriter.WriteString("PUBLIC", xmlNotation.PublicId);
                bsonWriter.WriteEndDocument();
            }
            else {
                bsonWriter.WriteNull();
            }
            if (mustClose) {
                bsonWriter.WriteEndDocument();
            }
        }
        #endregion

    }

    /// <summary>
    /// Represents a serializer for <see cref="XmlText"/>s.
    /// </summary>
    public sealed class XmlTextSerializer : XmlNodeSerializer
    {

        #region private static fields
        private static readonly XmlTextSerializer instance = new XmlTextSerializer();
        #endregion

        #region public static properties
        /// <summary>
        /// Gets an instance of the XmlTextSerializer class.
        /// </summary>
        public static XmlTextSerializer Instance
        {
            get { return instance; }
        }
        #endregion

        #region public members
        public override void Serialize(BsonWriter bsonWriter, Type nominalType, object value, IBsonSerializationOptions options)
        {
            var xmlText = value as XmlText;
            bsonWriter.WriteString(xmlText.Name, xmlText.Value);
        }
        #endregion

    }
}

