namespace AuthFlowMaui.Constants;

public readonly record struct RealmConstants()
{
    public const string HttpClientName = "maui-to-https-keycloak";
    public const string RealmUrl = "/realms/dev/protocol/openid-connect";
#if ANDROID
    public const string BaseUrl = "https://10.0.2.2:8843";
#else
    public const string BaseUrl = "https://localhost:8843";
#endif
#if ANDROID
        public const string ValidIssuer = "https://10.0.2.2:8843/realms/dev";
        public const string ValidAudience = "https://10.0.2.2:8843/realms/dev";
#else
    public const string ValidIssuer = "https://localhost:8843/realms/dev";
    public const string ValidAudience = "https://localhost:8843/realms/dev";

#endif
    public const string ValidRealmAudience = "maui-client";
}
