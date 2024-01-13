namespace AccountingSystem.Web.Models
{
    public class LoginViewModel
    {
        public required string username { get; set; }
        public required string password { get; set; }
        public bool rememberMe { get; set; }
    }
}
