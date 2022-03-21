namespace ExplorJobAPI.Domain.Commands.AuthUsers
{
    public class UserChangePasswordCommand
    {
        public string Email { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
