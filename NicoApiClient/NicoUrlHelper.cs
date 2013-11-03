using System.Linq;

namespace NicoApi {

	public class NicoUrlHelper {

		private static readonly RegexLinkMatcher[] matchers = new[] {
			new RegexLinkMatcher("www.nicovideo.jp/watch/{0}", @"nicovideo.jp/watch/([a-z]{2}\d{4,10})"),
			new RegexLinkMatcher("www.nicovideo.jp/watch/{0}", @"nicovideo.jp/watch/(\d{6,12})"),
			new RegexLinkMatcher("www.nicovideo.jp/watch/{0}", @"nico.ms/([a-z]{2}\d{4,10})"),
			new RegexLinkMatcher("www.nicovideo.jp/watch/{0}", @"nico.ms/(\d{6,12})")
		};

		public static string GetIdByUrl(string url) {

			var matcher = matchers.FirstOrDefault(m => m.IsMatch(url));

			if (matcher == null)
				return null;

			return matcher.GetId(url);

		}

		public string GetUrlById(string id) {

			var matcher = matchers.First();
			return string.Format("http://{0}", matcher.MakeLinkFromId(id));

		}

		public bool IsValidFor(string url) {

			return matchers.Any(m => m.IsMatch(url));

		}

		public VideoDataResult ParseByUrl(string url, bool getAuthorName) {

			var id = GetIdByUrl(url);

			if (id == null) {
				throw new NicoApiException("Not a valid NicoNicoDouga URL.");
			}

			return VideoApiClient.GetVideoData(id, getAuthorName);

		}

	}

}
