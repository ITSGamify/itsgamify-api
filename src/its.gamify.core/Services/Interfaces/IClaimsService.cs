namespace its.gamify.core.Services.Interfaces;

public interface IClaimsService
{
    public Guid CurrentUser { get; }
    public string CurrentRole { get; }
}
