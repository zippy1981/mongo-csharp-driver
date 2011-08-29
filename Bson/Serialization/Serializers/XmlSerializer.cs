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
    public sealed class XmlAttributeSerializer : XmlNodeSerializer
    {

        #region private static fields
        private static XmlAttributeSerializer instance = new XmlAttributeSerializer();
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
    /// Represents a serializer for <see cref="XmlCDataSection"/>s.
    /// </summary>
    public sealed class XmlCDataSectionSerializer : XmlNodeSerializer
    {

        #region private static fields
        private static XmlCDataSectionSerializer instance = new XmlCDataSectionSerializer();
        #endregion

        #region public static properties
        /// <summary>
        /// Gets an instance of the XmlCDataSectionSerializer class.
        /// </summary>
        public static XmlCDataSectionSerializer Instance
        {
            get { return instance; }
        }
        #endregion

        #region public members
        public override void Serialize(BsonWriter bsonWriter, Type nominalType, object value, IBsonSerializationOptions options)
        {
            var CDataSection = (value as XmlCDataSection);
            bsonWriter.WriteString(CDataSection.Name, CDataSection.Value);
        }
        #endregion

    }
	
	/// <summary>
    /// Represents a serializer for <see cref="XmlDocument"/>s.
    /// </summary>
    public sealed class XmlDocumentSerializer : XmlNodeSerializer {

        #region private static fields
        private static XmlDocumentSerializer instance = new XmlDocumentSerializer();
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
            SerializeXmlElement(bsonWriter, rootElem, options);
            bsonWriter.WriteEndDocument();
        }
        #endregion

    }
	
	/// <summary>
    /// Represents a serializer for <see cref="XmlDocumentFragment"/>s.
    /// </summary>
    public sealed class XmlDocumentFragmentSerializer : XmlNodeSerializer {
        
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
            SerializeXmlElement(bsonWriter, elem, options);
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

        protected static void SerializeXmlElement(BsonWriter bsonWriter, XmlElement elem, IBsonSerializationOptions options)
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

	    private static void SerializeXmlElementChildNodes(BsonWriter bsonWriter, XmlElement elem, IBsonSerializationOptions options)
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
	            bool serializedCData = false;
	            bool serializedText = false;

	            foreach (XmlNode node in elem.ChildNodes) {

                    if (node.GetType() == typeof(XmlElement)) {
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
                    else if (node.GetType() == typeof(XmlCDataSection))
                    {
                        if (serializedCData)
                        {
                            continue;
                        }
                        serializedCData = true;
                        var cDataNodes =
                            (from cData in elem.ChildNodes.OfType<XmlCDataSection>() select cData).ToArray();
                        if (cDataNodes.Length > 1) {
                            bsonWriter.WriteName(node.Name);
                            bsonWriter.WriteStartArray();
                            foreach (var cDataNode in cDataNodes)
                            {
                                bsonWriter.WriteString(cDataNode.Value);
                            }
                            bsonWriter.WriteEndArray();
                        }
                        else {
                            bsonWriter.WriteString(node.Name, node.Value);
                        }
                    }
                    else if (node.GetType() == typeof(XmlText)) {
                        if (serializedText) {
                            continue;
                        }
                        serializedText = true;
                        var textNodes =
                            (from text in elem.ChildNodes.OfType<XmlText>() select text).ToArray();
                        if (textNodes.Length > 1) {
                            bsonWriter.WriteName(node.Name);
                            bsonWriter.WriteStartArray();
                            foreach (var textNode in textNodes) {
                                bsonWriter.WriteString(textNode.Value);
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
            throw new NotImplementedException();
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

