namespace WBAPI.Configuration;

public class JwtSettings
{
    public string Key { get; set; }

    public string Issuer { get; set; }
    public string Audience { get; set; }
    public JwtSettings()
    {
        Key = String.Empty;
        Issuer = String.Empty;
        Audience = String.Empty;
    }

    public JwtSettings(string key, string issuer, string audience)
    {
        Key = key;
        Issuer = issuer;
        Audience = audience;
    }
}

