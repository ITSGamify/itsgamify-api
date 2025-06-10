namespace its.gamify.core.Services.Interfaces;

public interface ICurrentTime
{
    DateTime GetCurrentTime();
}


public class CurrentTime : ICurrentTime
{
    public DateTime GetCurrentTime() => DateTime.Now;
}