namespace its.gamify.domains.Models;

public class AppSetting
{
    public Dictionary<string, string> ConnectionStrings { get; set; } = new();
    public FirebaseConfig FirebaseConfig { get; set; } = null;

}

public class FirebaseConfig
{
    public string ServiceAccountAdmin { get; set; } = string.Empty;
    public string ServiceAccountPass { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public string Bucket { get; set; } = string.Empty;

}
