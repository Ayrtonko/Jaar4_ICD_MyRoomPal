using myroompal_api.Entities.Types;

namespace myroompal_api.Entities.Entities;

public class Match
{
    public Guid Id { get; set; }
    public DateOnly MatchDate { get; set; }

    public Guid MatcherUserId { get; set; }
    public required User MatcherUser { get; set; }

    public Guid MatcheeUserId { get; set; }
    public required User MatcheeUser { get; set; }
}