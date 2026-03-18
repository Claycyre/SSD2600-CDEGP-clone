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
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ContactDetailRepository _contactRepo;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ContactDetailRepository contactDetailRepository
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _contactRepo = contactDetailRepository;
        }

        public string Username { get; set; }

        /// <summary>The account type (UserRole) of the current user.</summary>
        public string UserRole { get; set; }

        /// <summary>When the account was registered.</summary>
        public DateTime RegisteredAt { get; set; }

        // VerificationDocumentPath commented out — document storage not in use.
        // /// <summary>Path to the uploaded verification document, if any.</summary>
        // public string VerificationDocumentPath { get; set; }

        /// <summary>Whether the verification has been submitted by the user.</summary>
        public bool VerificationSubmitted { get; set; }

        /// <summary>Whether the admin has approved the verification.</summary>
        public bool VerificationApproved { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "First Name")]
            public string NameFirst { get; set; }

#nullable enable
            [Display(Name = "Last Name")]
            public string? NameLast { get; set; } = string.Empty;

#nullable disable

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            var contactDetail = await _contactRepo.GetByUserAsync(user, true);

            Username = userName;
            UserRole = user.UserRole;
            RegisteredAt = user.RegisteredAt;
            // VerificationDocumentPath = user.VerificationDocumentPath; // commented out — document storage not in use
            VerificationSubmitted = user.VerificationSubmitted;
            VerificationApproved = user.VerificationApproved;

            Input = new InputModel
            {
                NameFirst = contactDetail.NameFirst,
                NameLast = contactDetail.NameLast,
                PhoneNumber = phoneNumber,
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

            var contactDetail = await _contactRepo.GetByUserAsync(user);
            contactDetail.NameFirst = Input.NameFirst;
            contactDetail.NameLast = Input.NameLast;

            await _contactRepo.UpdateAsync(contactDetail);

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(
                    user,
                    Input.PhoneNumber
                );
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
