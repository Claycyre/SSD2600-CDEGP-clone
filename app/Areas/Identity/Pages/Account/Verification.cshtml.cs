// Identity Verification upload page
#nullable disable

using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SSD2600_CDEGP.Models;

// using SSD2600_CDEGP.Services; // commented out — verification document upload not in use

namespace SSD2600_CDEGP.Areas.Identity.Pages.Account
{
    [Authorize]
    public class VerificationModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        // private readonly IFileStorageService _storage; // commented out — verification document storage not in use

        public VerificationModel(
            UserManager<ApplicationUser> userManager
        // IFileStorageService storage // commented out
        )
        {
            _userManager = userManager;
            // _storage = storage; // commented out
        }

        [TempData]
        public string StatusMessage { get; set; }

        public ApplicationUser CurrentUser { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            CurrentUser = await _userManager.GetUserAsync(User);
            if (CurrentUser == null)
                return NotFound();

            return Page();
        }

        /* OnPostUploadAsync commented out — verification document upload not in use.
        public async Task<IActionResult> OnPostUploadAsync(IFormFile verificationDocument)
        {
            CurrentUser = await _userManager.GetUserAsync(User);
            if (CurrentUser == null)
                return NotFound();

            if (verificationDocument == null || verificationDocument.Length == 0)
            {
                StatusMessage = "Error: Please select a file to upload.";
                return RedirectToPage();
            }

            const long maxBytes = 10 * 1024 * 1024; // 10 MB
            if (verificationDocument.Length > maxBytes)
            {
                StatusMessage = "Error: File size must not exceed 10 MB.";
                return RedirectToPage();
            }

            var ext = Path.GetExtension(verificationDocument.FileName).ToLowerInvariant();
            var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png" };
            var allowedContentTypes = new[] { "application/pdf", "image/jpeg", "image/png" };
            if (!allowedExtensions.Contains(ext) || !allowedContentTypes.Contains(verificationDocument.ContentType))
            {
                StatusMessage = "Error: Only PDF, JPG, and PNG files are accepted.";
                return RedirectToPage();
            }

            var key = $"verification/{CurrentUser.Id}/verification{ext}";
            using (var stream = verificationDocument.OpenReadStream())
            {
                await _storage.UploadAsync(stream, key, verificationDocument.ContentType);
            }

            CurrentUser.VerificationDocumentPath = key;
            CurrentUser.VerificationSubmitted = true;
            CurrentUser.VerificationApproved = false;
            await _userManager.UpdateAsync(CurrentUser);

            StatusMessage = "Your verification document has been submitted and is awaiting review.";
            return RedirectToPage("/Account/Manage/Index", new { area = "Identity" });
        }
        */

        public async Task<IActionResult> OnPostSkipAsync()
        {
            return RedirectToPage("/Account/Manage/Index", new { area = "Identity" });
        }

        /* OnGetDocumentAsync commented out — verification document storage not in use.
        /// <summary>
        /// Generates a download URL for the current user's verification document and redirects to it.
        /// </summary>
        public async Task<IActionResult> OnGetDocumentAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || string.IsNullOrEmpty(user.VerificationDocumentPath))
                return NotFound();

            var url = await _storage.GetDownloadUrlAsync(user.VerificationDocumentPath);
            return Redirect(url);
        }
        */
    }
}
