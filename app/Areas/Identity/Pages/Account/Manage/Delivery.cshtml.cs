// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SSD2600_CDEGP.Models;
using SSD2600_CDEGP.Repositories;

namespace SSD2600_CDEGP.Areas.Identity.Pages.Account.Manage
{
    public class DeliveryModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ContactDetailRepository _contactRepo;

        public DeliveryModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ContactDetailRepository contactDetailRepository
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _contactRepo = contactDetailRepository;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
#nullable enable
            [Display(Name = "Street Address", Prompt = "Street Address")]
            public string? StreetAddress { get; set; } = string.Empty;

            [Display(Name = "Administrative Area", Prompt = "State/Province/Voivodeship")]
            public string? AdministrativeArea { get; set; } = string.Empty;

            [Display(Name = "Country Code", Prompt = "3-letter Country Code")]
            [StringLength(3)]
            [RegularExpression("[A-Z]*")]
            public string? CountryCode { get; set; } = string.Empty;

            [Display(Name = "Postal Code")]
            public string? PostalCode { get; set; } = string.Empty;
#nullable disable
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var contactDetail = await _contactRepo.GetByUserAsync(user, true);

            Input = new InputModel
            {
                StreetAddress = contactDetail.StreetAddress,
                AdministrativeArea = contactDetail.AdministrativeArea,
                PostalCode = contactDetail.PostalCode,
                CountryCode = contactDetail.CountryCode,
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var contactDetail = await _contactRepo.GetByUserAsync(user, true);
            contactDetail.StreetAddress = Input.StreetAddress;
            contactDetail.AdministrativeArea = Input.AdministrativeArea;
            contactDetail.PostalCode = Input.PostalCode;
            contactDetail.CountryCode = Input.CountryCode;

            await _contactRepo.UpdateAsync(contactDetail);

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your delivery details have been updated.";
            return RedirectToPage();
        }
    }
}
