namespace EHealth.ManageItemLists.Presentation.Keycloack
{
    public class KeycloackConfig
    {
        public string Host { get; set; } = string.Empty;
        public string Realm { get; set; } = string.Empty;
        public string PublicKeyJWT { get; set; } = string.Empty;
        public string Client_Id { get; set; } = string.Empty;
        public string Client_Secret { get; set; } = string.Empty;
    }
}
