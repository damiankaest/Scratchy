namespace Scratchy.Domain.DB
{
    public class UserBadge
    {
        public string Id { get; set; }

        // FK auf User
        public string UserId { get; set; }
        public User User { get; set; }

        // FK auf Badge
        public string BadgeId { get; set; }
        public Badge Badge { get; set; }

        // Level, den der User für diesen Badge aktuell hat
        public int Level { get; set; } = 1;

        // Wann erreicht / wann auf dieses Level aufgestiegen
        public DateTime EarnedOn { get; set; } = DateTime.Now;
    }
}
