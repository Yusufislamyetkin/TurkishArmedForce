using System.ComponentModel.DataAnnotations;
using InterviewPrep.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InterviewPrep.Web.Areas.Identity.Pages.Account.Manage;

public class IndexModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public IndexModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [TempData]
    public string StatusMessage { get; set; } = string.Empty;

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public class InputModel
    {
        [Display(Name = "Ad Soyad")]
        [Required]
        public string FullName { get; set; } = string.Empty;

        [Phone]
        [Display(Name = "Telefon")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Şu anki rol")]
        public string? CurrentRole { get; set; }

        [Display(Name = "Hedef seviye")]
        public string? TargetLevel { get; set; }
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound("Kullanıcı bulunamadı.");
        }

        Input = new InputModel
        {
            FullName = user.FullName ?? string.Empty,
            PhoneNumber = await _userManager.GetPhoneNumberAsync(user),
            CurrentRole = user.CurrentRole,
            TargetLevel = user.TargetLevel
        };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound("Kullanıcı bulunamadı.");
        }

        user.FullName = Input.FullName;
        user.CurrentRole = Input.CurrentRole;
        user.TargetLevel = Input.TargetLevel;

        var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
        if (Input.PhoneNumber != phoneNumber)
        {
            var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
            if (!setPhoneResult.Succeeded)
            {
                StatusMessage = "Telefon numarası güncellenemedi.";
                return RedirectToPage();
            }
        }

        await _userManager.UpdateAsync(user);
        await _signInManager.RefreshSignInAsync(user);
        StatusMessage = "Profil bilgileriniz güncellendi.";
        return RedirectToPage();
    }
}
