using System;

namespace NicoApi {

	/// <summary>
	/// Result of NicoNicoDouga video data query request.
	/// </summary>
	public class VideoDataResult {

		public VideoDataResult(string title, string thumbUrl, int? length, DateTimeOffset? created, int views, string authorId, string author, NicoTag[] tags) {
			Author = author;
            LengthSeconds = length;
			AuthorId = authorId;
			ThumbUrl = thumbUrl;
			Title = title;
			UploadDate = created;
			Views = views;
            Tags = tags;
		}

		/// <summary>
		/// Name of the user who uploaded the video.
		/// </summary>
		public string Author { get; }

		/// <summary>
		/// User Id of the user who uploaded the video.
		/// </summary>
		public string AuthorId { get; }

		/// <summary>
		/// Video length in seconds.
		/// </summary>
		public int? LengthSeconds { get; }

        public NicoTag[] Tags { get; }

		/// <summary>
		/// Thumbnail URL.
		/// Cannot be null. Can be empty.
		/// </summary>
		public string ThumbUrl { get; }

		/// <summary>
		/// Video title.
		/// Cannot be null or empty.
		/// </summary>
		public string Title { get; }

        /// <summary>
        /// Date when this video was uploaded.
        /// </summary>
        public DateTimeOffset? UploadDate { get; }

		/// <summary>
		/// Number of views.
		/// </summary>
		public int Views { get; }

	}

    public class NicoTag {

        public NicoTag(string name, bool locked) {
            Name = name;
            IsLocked = locked;
        }

        public bool IsLocked { get; }
        public string Name { get; }

    }

}
