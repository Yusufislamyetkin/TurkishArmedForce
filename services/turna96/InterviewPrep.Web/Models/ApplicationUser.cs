using Microsoft.AspNetCore.Identity;

namespace InterviewPrep.Web.Models;

public class ApplicationUser : IdentityUser
{
    public string? FullName { get; set; }
    public string? CurrentRole { get; set; }
    public string? TargetLevel { get; set; }
}
