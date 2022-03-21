namespace ExplorJobAPI.Domain.Commands.AuthUsers
{
    public class UserForgottenPasswordCommand
    {
        public string Email { get; set; }
        public string FallbackUrl { get; set; }
    }
}
