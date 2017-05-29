namespace SolidCP.WebDav.Core.Config.Entities
{
    public class SolidCPConstantUserParameters : AbstractConfigCollection
    {
        public string Login { get; private set; }
        public string Password { get; private set; }

        public SolidCPConstantUserParameters()
        {
            Login = ConfigSection.SolidCPConstantUser.Login;
            Password = ConfigSection.SolidCPConstantUser.Password;
        }
    }
}