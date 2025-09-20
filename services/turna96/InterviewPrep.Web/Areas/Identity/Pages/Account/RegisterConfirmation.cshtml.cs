using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InterviewPrep.Web.Areas.Identity.Pages.Account;

public class RegisterConfirmationModel : PageModel
{
    private readonly IEmailSender _emailSender;

    public RegisterConfirmationModel(IEmailSender emailSender)
    {
        _emailSender = emailSender;
    }

    public string Email { get; set; } = string.Empty;
    public bool DisplayConfirmAccountLink { get; set; }
    public string EmailConfirmationUrl { get; set; } = string.Empty;

    public async Task<IActionResult> OnGetAsync(string? email, string? returnUrl = null)
    {
        if (email == null)
        {
            return RedirectToPage("./Register");
        }
        Email = email;
        DisplayConfirmAccountLink = _emailSender == null;

        EmailConfirmationUrl = Url.Page(
            "/Account/ConfirmEmail",
            pageHandler: null,
            values: new { area = "Identity", returnUrl },
            protocol: Request.Scheme) ?? string.Empty;

        await Task.CompletedTask;
        return Page();
    }
}
