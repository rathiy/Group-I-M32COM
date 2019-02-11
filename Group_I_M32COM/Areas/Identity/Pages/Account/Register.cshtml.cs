﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Group_I_M32COM.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Linq;

namespace Group_I_M32COM.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        private readonly Data.ApplicationDbContext _context;
        private readonly Helpers.ICountryDataService _countryData;

        public RegisterModel(
            Helpers.ICountryDataService countryData,
            Data.ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
            _countryData = countryData;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [StringLength(50, ErrorMessage = "The minimum {2} and Maximum {1} characters are allowed", MinimumLength = 3)]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [StringLength(50, ErrorMessage = "The minimum {2} and Maximum {1} characters are allowed", MinimumLength = 3)]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [DataType(DataType.Text)]
            [MaxLength(50)]
            public string Address { get; set; }

            [DataType(DataType.Text)]
            [MaxLength(50)]
            public string City { get; set; }

            [DataType(DataType.Text)]
            [MaxLength(15)]
            [Display(Name = "Postal Code")]
            public string PostalCode { get; set; }

            [DataType(DataType.Text)]
            [MaxLength(35)]
            public string Country{ get; set; }
        }

        /*public void LoadCountryList()
        {
            List<string> CountryList = new List<string>();
            CultureInfo[] CInfoList = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            foreach (CultureInfo CInfo in CInfoList)
            {
                RegionInfo R = new RegionInfo(CInfo.LCID);
                if (!(CountryList.Contains(R.EnglishName)))
                {
                    CountryList.Add(R.EnglishName);
                    Console.WriteLine("Loaded countries: ", CountryList);
                }
            }

            CountryList.Sort();
            ViewData["CountryList"] = CountryList;
        }*/

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            //LoadCountryList();
            ViewData["CountryList"] = _countryData.LoadCountryList();
        }

        public async Task<IActionResult> OnPostAsync(string CountryList, string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                //var user = new ApplicationUser { UserName = Input.Email, Email = Input.Email };

                string selected_country = CountryList.ToString().Trim();
                Console.WriteLine("Selected Country: " + selected_country);

                // The below NETCORE class is used to separate the email string into parts such as user and domain
                System.Net.Mail.MailAddress addr = new System.Net.Mail.MailAddress(Input.Email);
                string email_username = addr.User;

                var user = new ApplicationUser
                {
                    UserName = email_username,
                    Email = Input.Email,
                    FirstName = Input.FirstName,
                    LastName = Input.LastName,
                    Address = Input.Address,
                    City = Input.City,
                    PostalCode = Input.PostalCode,
                    Country = selected_country,
                    Created_At = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Trim())
                };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    // Add role to user after successful insert
                    result = await _userManager.AddToRoleAsync(user, "User");

                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = user.Id, code = code },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}