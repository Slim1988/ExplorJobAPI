namespace ExplorJobAPI.Domain.Commands.AuthUsers
{
    public class UserGetNewEmailTokenConfirmationCommand
    {
        public string Email { get; set; }
        public string FallbackUrl { get; set; }
    }
}
