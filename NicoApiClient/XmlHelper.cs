using System.Xml.Linq;
using System.Xml.XPath;

namespace NicoApi {

	/// <summary>
	/// Various XML helper methods.
	/// </summary>
	internal static class XmlHelper {

		public static string GetNodeTextOrEmpty(XElement node) {

			if (node == null)
				return string.Empty;

			return node.Value;

		}

		public static string GetNodeTextOrEmpty(XDocument doc, string xpath) {

			return GetNodeTextOrEmpty(doc.XPathSelectElement(xpath));

		}

	}

}
