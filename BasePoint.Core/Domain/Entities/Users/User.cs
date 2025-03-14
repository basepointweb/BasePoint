namespace BasePoint.Core.Domain.Entities.Users
{
    public class User : BaseEntity
    {
        public virtual string Login { get; protected set; }

        public virtual string Password { get; protected set; }

        public virtual string Email { get; protected set; }

        protected User() : base()
        {

        }

        public User(string login, string password, string email) : base()
        {
            Login = login;
            Password = password;
            Email = email;
        }
    }
}