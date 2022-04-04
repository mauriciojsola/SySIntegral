using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text.Encodings.Web;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using SySIntegral.Core.Entities.Users;

namespace SySIntegral.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ForgotPasswordModel(UserManager<ApplicationUser> userManager, IEmailSender emailSender, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _webHostEnvironment= webHostEnvironment;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Email es requerido")]
            [EmailAddress]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByEmailAsync(Input.Email);
                    if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                    {
                        // Don't reveal that the user does not exist or is not confirmed
                        return RedirectToPage("./ForgotPasswordConfirmation");
                    }

                    // For more information on how to enable account confirmation and password reset please 
                    // visit https://go.microsoft.com/fwlink/?LinkID=532713
                    var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ResetPassword",
                        pageHandler: null,
                        values: new { area = "Identity", code },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(
                        Input.Email,
                        "SySIntegral - Resetear contraseña", PrepareForgotPasswordEmailBody(callbackUrl));
                    //$"Para resetear su contraseña, por favor siga el siguiente link: <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>RESETEAR CONTRASEÑA</a>."

                    return RedirectToPage("./ForgotPasswordConfirmation");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error: {ex.Message}");
                    return Page();
                }

            }

            return Page();
        }

        private string PrepareForgotPasswordEmailBody(string callbackUrl)
        {
            //var path = Server.MapPath("~/Content/EmailTemplates/ForgotPassword.html");
            var webRootPath = _webHostEnvironment.WebRootPath;
            var contentRootPath = _webHostEnvironment.ContentRootPath;
            var path = Path.Combine(contentRootPath, @"Content\EmailTemplates\ForgotPassword.html");

            var body = System.IO.File.ReadAllText(path);
            // replace tokens
            body = body.Replace("{{COMPANY_NAME}}", "SySIntegral");
            body = body.Replace("{{RESET_PASSWORD_URL}}", HtmlEncoder.Default.Encode(callbackUrl));
            body = body.Replace("{{YEAR}}", DateTime.Now.Year.ToString());
            return body;
        }
    }
}
