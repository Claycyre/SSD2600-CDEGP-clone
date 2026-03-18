// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using SSD2600_CDEGP.Models;
using SSD2600_CDEGP.Repositories;
using SSD2600_CDEGP.Services.Interfaces;

namespace SSD2600_CDEGP.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailService _emailService;
        private readonly ContactDetailRepository _contactDetailRepo;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailService emailService,
            ContactDetailRepository contactDetailRepo
        )
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailService = emailService;
            _contactDetailRepo = contactDetailRepo;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [Display(Name = "First Name")]
            public string NameFirst { get; set; }

            [Display(Name = "Last Name")]
            public string NameLast { get; set; }

            [Required]
            [StringLength(100)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare(
                "Password",
                ErrorMessage = "The password and confirmation password do not match."
            )]
            public string ConfirmPassword { get; set; }

            [Required(ErrorMessage = "Please select an account type.")]
            [Display(Name = "Account Type")]
            // SiteAdmin cannot be self-selected during registration; admin accounts are provisioned separately.
            [RegularExpression(
                "^(PrivateCitizen|Supplier|PurchaseManager)$",
                ErrorMessage = "Invalid account type."
            )]
            public string UserRole { get; set; } = "PrivateCitizen";
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (
                await _signInManager.GetExternalAuthenticationSchemesAsync()
            ).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (
                await _signInManager.GetExternalAuthenticationSchemesAsync()
            ).ToList();

            if (!ModelState.IsValid)
                return Page();

            var user = CreateUser();
            user.UserRole = Input.UserRole ?? "PrivateCitizen";
            user.RegisteredAt = DateTime.UtcNow;

            await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
            await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
            var result = await _userManager.CreateAsync(user, Input.Password);

            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");

                // Assign the corresponding Identity role for Supplier accounts
                if (user.UserRole == "Supplier")
                {
                    await _userManager.AddToRoleAsync(user, "Supplier");
                }

                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new
                    {
                        area = "Identity",
                        userId = userId,
                        code = code,
                        returnUrl = returnUrl,
                    },
                    protocol: Request.Scheme
                );

                // callbackUrl cannot be null in normal Razor Pages operation
                if (string.IsNullOrEmpty(callbackUrl))
                {
                    _logger.LogError("Failed to generate email confirmation callback URL");
                    ModelState.AddModelError(
                        string.Empty,
                        "An error occurred during registration. Please try again."
                    );
                    return Page();
                }

                var response = await _emailService.SendEmailAsync(
                    new EmailModel
                    {
                        Subject = "Confirm your email",
                        RecipientEmail = Input.Email,
                        Body =
                            $"Please confirm your account by <a "
                            + $"href='{HtmlEncoder.Default.Encode(callbackUrl)}'> "
                            + $"clicking here</a>.",
                    }
                );

                var contactDetail = new ContactDetail
                {
                    EmailAddress = Input.Email,
                    NameFirst = Input.NameFirst,
                    NameLast = Input.NameLast,
                    User = user,
                };
                await _contactDetailRepo.AddAsync(contactDetail);

                return RedirectToPage(
                    "RegisterConfirmation",
                    new { email = Input.Email, returnUrl = returnUrl }
                );
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException(
                    $"Can't create an instance of '{nameof(ApplicationUser)}'. "
                        + $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively "
                        + $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml"
                );
            }
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException(
                    "The default UI requires a user store with email support."
                );
            }

            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
    }
}
