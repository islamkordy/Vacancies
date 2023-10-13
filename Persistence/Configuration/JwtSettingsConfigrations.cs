namespace Persistance.Configurations;

public class JwtSettingsConfigrations
{
    public string AccessTokenSecret { get; set; }
    public double AccessTokenExpirationSecond { get; set; }
    public double RefreshTokenExpirationSecond { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
}
