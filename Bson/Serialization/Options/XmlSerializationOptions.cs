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

using System.Xml;

using MongoDB.Bson.Serialization.Serializers;

namespace MongoDB.Bson.Serialization.Options
{
    /// <summary>
    /// Serialization options for <see cref="XmlNode"/>s.
    /// </summary>
    /// <seealso cref="XmlNodeSerializer"/>
    public sealed class XmlSerializationOptions : IBsonSerializationOptions
    {
        #region private static fields
        private static XmlSerializationOptions defaults = new XmlSerializationOptions();
        #endregion

        #region private fields
       
        #endregion

        #region public static properties
        /// <summary>
        /// Gets or sets the default document serialization options.
        /// </summary>
        public static XmlSerializationOptions Defaults {
            get { return defaults; }
            set { defaults = value; }
        }
        #endregion

        #region public properties
        /// <summary>
        /// Gets whether to serialize <see cref="XmlComment"/>s.
        /// </summary>
        /// <remarks>
        /// This only applies to <see cref="XmlNode.ChildNodes"/>.
        /// If you attempt to serialize an XmlComment directly, it
        /// will always serialize.
        /// </remarks>
        public bool SerializeComments { get; set; }
        
		/// <summary>
		/// Get whether to serialize the <see cref="XmlDocumentType"/>.
		/// </summary>
		/// <remarks>
		/// This only applies to the <see cref="XmlDocument.DocumentType" />.
		/// If you serialize an XmlDocType directly, it will always serialize.
		/// </remarks>
		public bool SerializeDocType { get; set; }
		
		/// <summary>
		/// Get whether to serialize <see cref="XmlNotation"/> and
		/// <see cref="XmlEntity"/> nodes.
		/// </summary>
        /// <remarks>
        /// This only applies to <see cref="XmlNode.ChildNodes"/>.
        /// If you attempt to serialize an XmlEntity or XmlNotation directly, it
        /// will always serialize.
        /// </remarks>
		public bool SerializeDtdElements { get; set; }
		#endregion
    }
}