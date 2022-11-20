namespace CalendarSynchronizerWeb.Models
{
    public class GoogleAuthCreds
    {
        public GoogleAuthCreds()
        {
            RefreshToken = "default";
            AccessToken = "default";
            SyncToken = "default";
        }
        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }
        public string SyncToken { get; set; }
    }
}
