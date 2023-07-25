namespace WebServer.Authentication
{
    public class JWTToken
    {
        public string Token { get; set; }
        public string ReferenceToken { get; set; }
        public bool IsAuthSuccessful { get; set; }
        public string ErrorMessage { get; set; }
    }
}
