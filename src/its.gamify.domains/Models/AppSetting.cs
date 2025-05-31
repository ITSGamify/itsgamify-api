namespace its.gamify.domains.Models;

public class AppSetting
{
    public Dictionary<string, string> ConnectionStrings { get; set; } = new();
}