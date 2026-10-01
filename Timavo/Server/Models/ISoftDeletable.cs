namespace Timavo.Server.Models
{
    public interface ISoftDeletable
    {
        DateTimeOffset? DeletedAt { get; set; }
    }
}
