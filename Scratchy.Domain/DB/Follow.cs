namespace Scratchy.Domain.DB
{
    public class Follow
    {
        public Follow()
        {
            
        }
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string FollowerId { get; set; } // User, der folgt
        public string FollowingId { get; set; } // User, dem gefolgt wird
        public DateTime CreatedAt { get; set; } // Zeitpunkt, wann das Follow erstellt wurde
    }
}
