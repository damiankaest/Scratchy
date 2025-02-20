namespace Scratchy.Domain.Exceptions
{
    public class AlbumServiceException : BaseException
    {
        public AlbumServiceException(string message, string errorSource = "AlbumService")
            : base(461, errorSource, message) { }
    }

    public class ArtistServiceException : BaseException
    {
        public ArtistServiceException(string message, string errorSource = "ArtistService")
            : base(462, errorSource, message) { }
    }

    public class BadgeServiceException : BaseException
    {
        public BadgeServiceException(string message, string errorSource = "BadgeService")
            : base(463, errorSource, message) { }
    }

    public class BlobServiceException : BaseException
    {
        public BlobServiceException(string message, string errorSource = "BlobService")
            : base(464, errorSource, message) { }
    }

    public class CollectionServiceException : BaseException
    {
        public CollectionServiceException(string message, string errorSource = "CollectionService")
            : base(465, errorSource, message) { }
    }

    public class ExploreServiceException : BaseException
    {
        public ExploreServiceException(string message, string errorSource = "ExploreService")
            : base(466, errorSource, message) { }
    }

    public class FollowerServiceException : BaseException
    {
        public FollowerServiceException(string message, string errorSource = "FollowerService")
            : base(467, errorSource, message) { }
    }

    public class FriendshipServiceException : BaseException
    {
        public FriendshipServiceException(string message, string errorSource = "FriendshipService")
            : base(468, errorSource, message) { }
    }

    public class LibraryServiceException : BaseException
    {
        public LibraryServiceException(string message, string errorSource = "LibraryService")
            : base(469, errorSource, message) { }
    }

    public class LoginServiceException : BaseException
    {
        public LoginServiceException(string message, string errorSource = "LoginService")
            : base(470, errorSource, message) { }
    }

    public class NotificationServiceException : BaseException
    {
        public NotificationServiceException(string message, string errorSource = "NotificationService")
            : base(471, errorSource, message) { }
    }

    public class PostServiceException : BaseException
    {
        public PostServiceException(string message, string errorSource = "PostService")
            : base(472, errorSource, message) { }
    }

    public class ScratchServiceException : BaseException
    {
        public ScratchServiceException(string message, string errorSource = "ScratchService")
            : base(473, errorSource, message) { }
    }

    public class ShowCaseServiceException : BaseException
    {
        public ShowCaseServiceException(string message, string errorSource = "ShowCaseService")
            : base(474, errorSource, message) { }
    }

    public class SpotifyServiceException : BaseException
    {
        public SpotifyServiceException(string message, string errorSource = "SpotifyService")
            : base(475, errorSource, message) { }
    }

    public class StatServiceException : BaseException
    {
        public StatServiceException(string message, string errorSource = "StatService")
            : base(476, errorSource, message) { }
    }

    public class TokenServiceException : BaseException
    {
        public TokenServiceException(string message, string errorSource = "TokenService")
            : base(477, errorSource, message) { }
    }

    public class UserServiceException : BaseException
    {
        public UserServiceException(string message, string errorSource = "UserService")
            : base(478, errorSource, message) { }
    }

}
