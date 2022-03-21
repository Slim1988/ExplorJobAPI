namespace ExplorJobAPI.Auth.Roles
{
    public static class Authorizations
    {
        public const string Administrator = Roles.Administrator;
        public const string AdministratorOrEmployee = Roles.Administrator + "," + Roles.Employee;
        public const string AdministratorOrUser = Roles.Administrator + "," + Roles.User;
        public const string AdministratorOrEmployeeOrUser = Roles.Administrator + "," + Roles.Employee + "," + Roles.User;
        public const string Employee = Roles.Administrator + "," + Roles.Employee;
        public const string User = Roles.Administrator + "," + Roles.User;
    }
}
