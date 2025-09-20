using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using InterviewPrep.Web.Models;

namespace InterviewPrep.Web.Areas.Identity.Pages.Account;

public class LoginWith2faModel : PageModel
{
    private readonly SignInManager<ApplicationUser> _signInManager;

    public LoginWith2faModel(SignInManager<ApplicationUser> signInManager)
    {
        _signInManager = signInManager;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public bool RememberMe { get; set; }

    public class InputModel
    {
        [Required]
        [StringLength(7, ErrorMessage = "Kod en az {2}, en fazla {1} karakter olmalıdır.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "Doğrulama kodu")]
        public string TwoFactorCode { get; set; } = string.Empty;

        [Display(Name = "Bu cihazı hatırla")]
        public bool RememberMachine { get; set; }
    }

    public async Task<IActionResult> OnGetAsync(bool rememberMe, string? returnUrl = null)
    {
        var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
        if (user == null)
        {
            throw new InvalidOperationException("İki faktörlü doğrulama kullanıcısı bulunamadı.");
        }

        RememberMe = rememberMe;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(bool rememberMe, string? returnUrl = null)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        returnUrl ??= Url.Content("~/");

        var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(Input.TwoFactorCode.Replace(" ", string.Empty), rememberMe, Input.RememberMachine);

        if (result.Succeeded)
        {
            return LocalRedirect(returnUrl);
        }
        if (result.IsLockedOut)
        {
            return RedirectToPage("./Lockout");
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Geçersiz doğrulama kodu.");
            return Page();
        }
    }
}
