namespace Billing.Api.Models
{
    public class CurrentUserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string[] Roles { get; set; }
        public string Username { get; set; }
    }
}