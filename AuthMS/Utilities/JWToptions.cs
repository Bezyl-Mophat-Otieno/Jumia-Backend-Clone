namespace AuthMS.Utilities
{
    public class JWToptions
    {
        public string SecretKey {get;set;}
        public string Issuer { get;set;}

        public string Audience { get;set;}
    }
}
