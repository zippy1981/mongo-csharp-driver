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
        /// This only applies to <see cref="XmlNode.ChildNode"/>s.
        /// If you attempt to serialize an XmlComment directly, it
        /// will always serialize.
        /// </remarks>
        public bool SerializeComments { get; set; }
        
		/// <summary>
		/// Get whether to serialize the <see cref="XmlDocumentType"/>.
		/// </summary>
		/// <remarks>
		/// This only applies to the <see cref="XmlDoocument.DocumentType" />.
		/// If you serialize an XmlDocType directly, it will always serialize.
		/// </remarks>
		public bool SerializeDocType { get; set; }
		
		/// <summary>
		/// Get whether to serialize <see cref="XmlNotation"/> and
		/// <see cref="XmlEntity"/> nodes.
		/// </summary>
        /// <remarks>
        /// This only applies to <see cref="XmlNode.ChildNode"/>s.
        /// If you attempt to serialize an XmlEntity or XmlNotation directly, it
        /// will always serialize.
        /// </remarks>
		public bool SerializeDtdElements { get; set; }
		#endregion
    }
}