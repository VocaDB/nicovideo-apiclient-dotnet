using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NicoApi.Tests {

	/// <summary>
	/// Unit tests for <see cref="VideoApiClient"/>.
	/// </summary>
	[TestClass]
	public class VideoApiClientTests {

		[TestMethod]
		public void ParseLength_LessThan10Mins() {

			var result = NicoApiClient.ParseLength("3:09");

			Assert.AreEqual(189, result, "result");

		}

		[TestMethod]
		public void ParseLength_MoreThan10Mins() {

			var result = NicoApiClient.ParseLength("39:39");

			Assert.AreEqual(2379, result, "result");

		}

		[TestMethod]
		public void ParseLength_MoreThan60Mins() {

			var result = NicoApiClient.ParseLength("339:39");

			Assert.AreEqual(20379, result, "result");

		}

	}

}
