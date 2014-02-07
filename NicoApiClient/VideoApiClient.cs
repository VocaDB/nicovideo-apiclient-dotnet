using System;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using HtmlAgilityPack;

namespace NicoApi {

	/// <summary>
	/// Various methods for the NicoNicoDouga video API.
	/// </summary>
	public static class VideoApiClient {

		private static string GetUserName(Stream htmlStream, Encoding encoding) {

			var doc = new HtmlDocument();
			try {
				doc.Load(htmlStream, encoding);
			} catch (IOException) {
				return null;
			}

			var titleElem = doc.DocumentNode.SelectSingleNode("//html/body/div/p[2]/a/strong");

			var titleText = (titleElem != null ? titleElem.InnerText : null);

			return (titleText != null ? HtmlEntity.DeEntitize(titleText) : null);

		}

		/// <summary>
		/// Gets NND user name from user Id.
		/// </summary>
		/// <param name="userId">User Id string. Can be null or empty, in which case null is returned.</param>
		/// <returns>
		/// User account name of the user matching the Id. Can be null or empty, if the user Id supplied was null or empty, or if the user could not be loaded.
		/// </returns>
		public static string GetUserName(string userId) {

			if (string.IsNullOrEmpty(userId))
				return null;

			var url = string.Format("http://ext.nicovideo.jp/thumb_user/{0}", userId);

			var request = WebRequest.Create(url);
			request.Timeout = 10000;

			try {
				using (var response = request.GetResponse())
				using (var stream = response.GetResponseStream()) {
					var enc = GetEncoding(response.Headers[HttpResponseHeader.ContentEncoding]);
					return GetUserName(stream, enc);
				}
			} catch (WebException) {
				return null;
			}

		}

		/// <summary>
		/// Gets video data by NND video Id.
		/// </summary>
		/// <param name="id">Video Id (such as sm1234567)</param>
		/// <param name="getAuthorName">
		/// Whether author name should be loaded. 
		/// This will cause an entra query, so specify false if you don't need it.
		/// </param>
		/// <returns>Result of the video data parsing. Can be null, if supplied Id was null.</returns>
		/// <exception cref="NicoApiException">If video data could not be loaded.</exception>
		public static VideoDataResult GetVideoData(string id, bool getAuthorName) {

			if (string.IsNullOrEmpty(id))
				return null;

			var url = string.Format("http://ext.nicovideo.jp/api/getthumbinfo/{0}", id);

			var request = WebRequest.Create(url);
			request.Timeout = 10000;

			XDocument doc;

			try {
				using (var response = request.GetResponse())
				using (var stream = response.GetResponseStream()) {
					doc = XDocument.Load(stream);
				}
			} catch (WebException x) {
				throw new NicoApiException("Unable to query data.", x);
			} catch (XmlException x) {
				throw new NicoApiException("Unable to query data.", x);
			}

			var res = doc.Element("nicovideo_thumb_response");

			if (res == null || res.Attribute("status") == null || res.Attribute("status").Value == "fail") {
				var err = (res != null ? res.XPathSelectElement("//nicovideo_thumb_response/error/description").Value : "empty response");
				throw new NicoApiException(string.Format("API response contained an error: {0}.", err));
			}

			var thumbElem = doc.XPathSelectElement("//nicovideo_thumb_response/thumb");
			var titleElem = (thumbElem != null ? thumbElem.Element("title") : null);

			if (titleElem == null) {
				throw new NicoApiException("API response didn't contain title element.");
			}

			var title = HtmlEntity.DeEntitize(titleElem.Value);

			var thumbUrl = XmlHelper.GetNodeTextOrEmpty(thumbElem.Element("thumbnail_url"));
			var userId = XmlHelper.GetNodeTextOrEmpty(thumbElem.Element("user_id"));
			var author = XmlHelper.GetNodeTextOrEmpty(thumbElem.Element("user_nickname"));
			var length = ParseLength(XmlHelper.GetNodeTextOrEmpty(thumbElem.Element("length")));
			var dateElem = thumbElem.Element("first_retrieve");
			var viewsElem = thumbElem.Element("view_counter");

			var date = DateTime.Parse(XmlHelper.GetNodeTextOrEmpty(dateElem));
			var views = int.Parse(XmlHelper.GetNodeTextOrEmpty(viewsElem));

			if (getAuthorName && string.IsNullOrEmpty(author))
				author = GetUserName(userId);

			var result = new VideoDataResult(title, thumbUrl, length, date, views, userId, author);
			return result;

		}

		private static Encoding GetEncoding(string encodingStr) {

			if (string.IsNullOrEmpty(encodingStr))
				return Encoding.UTF8;

			try {
				return Encoding.GetEncoding(encodingStr);
			} catch (ArgumentException) {
				return Encoding.UTF8;
			}

		}

		/// <summary>
		/// Parses video length seconds from the length string NND uses.
		/// For example 2:34 -> 154.
		/// </summary>
		/// <param name="lengthStr">Length string with minutes and seconds, for example 2:34. Can be null or empty.</param>
		/// <returns>Parsed length in seconds if successful, otherwise null.</returns>
		public static int? ParseLength(string lengthStr) {

			if (string.IsNullOrEmpty(lengthStr))
				return null;

			var parts = lengthStr.Split(':');

			if (parts.Length != 2)
				return null;

			int min, sec;
			if (!int.TryParse(parts[0], out min) || !int.TryParse(parts[1], out sec))
				return null;

			var totalSec = min * 60 + sec;

			return totalSec;

		}

	}

}
