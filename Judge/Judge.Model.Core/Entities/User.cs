namespace Judge.Model.Core.Entities
{
    public sealed class User
    {
        public long Id { get; internal set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
}
