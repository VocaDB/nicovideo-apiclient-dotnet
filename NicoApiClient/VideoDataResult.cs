using System;

namespace NicoApi {

	/// <summary>
	/// Result of NicoNicoDouga video data query request.
	/// </summary>
	public class VideoDataResult {

		public VideoDataResult(string title, string thumbUrl, int? length, DateTime created, int views, string authorId, string author) {
			Author = author;
			Length = length;
			AuthorId = authorId;
			ThumbUrl = thumbUrl;
			Title = title;
			Created = created;
			Views = views;
		}

		/// <summary>
		/// Name of the user who uploaded the video.
		/// </summary>
		public string Author { get; private set; }

		/// <summary>
		/// User Id of the user who uploaded the video.
		/// </summary>
		public string AuthorId { get; private set; }

		/// <summary>
		/// Date when this video was uploaded.
		/// </summary>
		public DateTime Created { get; private set; }

		/// <summary>
		/// Video length in seconds.
		/// </summary>
		public int? Length { get; private set; }

		/// <summary>
		/// Thumbnail URL.
		/// </summary>
		public string ThumbUrl { get; private set; }

		/// <summary>
		/// Video title.
		/// </summary>
		public string Title { get; private set; }

		/// <summary>
		/// Number of views.
		/// </summary>
		public int Views { get; private set; }

	}

}
