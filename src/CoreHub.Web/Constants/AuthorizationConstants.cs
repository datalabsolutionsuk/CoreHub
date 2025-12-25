namespace CoreHub.Web.Constants;

public static class Roles
{
    public const string SuperAdmin = "SuperAdmin";
    public const string Admin = "Admin";
    public const string Manager = "Manager";
    public const string Practitioner = "Practitioner";
    public const string User = "User";
}

public static class Policies
{
    public const string SuperAdminOnly = "SuperAdminOnly";
    public const string AdminAccess = "AdminAccess";
    public const string CanViewOwnCases = "CanViewOwnCases";
    public const string CanViewTeamCases = "CanViewTeamCases";
    public const string CanViewAllCases = "CanViewAllCases";
    public const string CanConfigureSystem = "CanConfigureSystem";
    public const string CanManageUsers = "CanManageUsers";
    public const string CanManageRoles = "CanManageRoles";
    public const string CanDeleteAnything = "CanDeleteAnything";
    public const string CanViewAuditLogs = "CanViewAuditLogs";
}
