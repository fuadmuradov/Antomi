using Antomi.DataAccsessLayer;
using Antomi.Models.Entity;
using Antomi.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Antomi.Controllers
{
    public class AccountController : Controller
    {
        private readonly AntomiDbContext context;
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountController(AntomiDbContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;

        }


        #region Login
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM login)
        {
            if (!ModelState.IsValid) return View();

            AppUser user = await userManager.FindByEmailAsync(login.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "Email or Password is incorrect");
                return View();
            }

            Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(user, login.Password, login.RememberMe, true);

            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", "Your account has been blocked for 5 minitues due to overtrying");
                    return View();
                }
                ModelState.AddModelError("", "Email or Password is incorrect");
                return View();
            }

            ViewBag.username = user.Email;


            return RedirectToAction("Index", "Home");
        }

        #endregion

        //********************  REGISTER  *********************
        #region Register

        //public async Task<IActionResult> CreateRole()
        //{
        //    await roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
        //    await roleManager.CreateAsync(new IdentityRole("Admin"));
        //    await roleManager.CreateAsync(new IdentityRole("Member"));
        //    return Content("Roles Creadted");
        //}


        //public async Task<IActionResult> CreateAdmin()
        //{
        //    AppUser user = new AppUser()
        //    {
        //        Firstname = "Fuad",
        //        Lastname = "Muradov",
        //        UserName = "fuadmuradov",
        //        City = "Baku",
        //        Country = "Azerbaijan",
        //        StreetAddress = "Nasimi",
        //        PhoneNumber = "+994553923264",
        //        Email = "fuadmuradov570@gmail.com"
        //    };

        //    await userManager.CreateAsync(user, "Fuad@12345");
        //    await userManager.AddToRoleAsync(user, "SuperAdmin");

        //    return Content("Done");
        //}

        public IActionResult Register()
        {
            return View();
        }
            

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM register)
        {
            if (!ModelState.IsValid) return View();

            AppUser user = new AppUser()
            {
                Firstname = register.FirstName,
                Lastname = register.LastName,
                Country = register.Country,
                City = register.City,
                StreetAddress = register.Street,
                UserName = register.FirstName+"_"+register.LastName,
                Email = register.Email,
                PhoneNumber = register.Phone                  
            };

            AppUser user1 = await userManager.FindByEmailAsync(user.Email);
            if (user1 != null)
            {
                ModelState.AddModelError("", "This Emali Already Exist");
                return View();
            }

            IdentityResult result = await userManager.CreateAsync(user, register.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }
            await userManager.AddToRoleAsync(user, "Member");

            string token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            string link = Url.Action(nameof(VerifyEmail), "Account", new { email = user.Email, token }, Request.Scheme, Request.Host.ToString());

            //string link = Url.Link("", new { Area = "Admin", email = user.Email, token }, Request.Scheme, Request.Host.ToString());
            // string link = Url.RouteUrl("/Admin/account/VerifyEmail", new { email = user.Email, token }, Request.Scheme, Request.Host.ToString());
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("hrmshrms2000@gmail.com", "Antomi Confirm");
            mail.To.Add(new MailAddress(user.Email));
            mail.Subject = "Verify Email";
            string body = string.Empty;
            using (StreamReader reader = new StreamReader("wwwroot/template/VerifyEmail.html"))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{{link}}", link);
            mail.Body = body;
            mail.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;

            smtp.Credentials = new NetworkCredential("hrmshrms2000@gmail.com", "hrms12345");
            smtp.Send(mail);

            TempData["Verify"] = true;

            return LocalRedirect("/Home");
        }

        public async Task<IActionResult> VerifyEmail(string email, string token)
        {
            AppUser user = await userManager.FindByEmailAsync(email);
            if (user == null) return BadRequest();
            await userManager.ConfirmEmailAsync(user, token);

            await signInManager.SignInAsync(user, true);
            ViewBag.username = user.Email;

            return LocalRedirect("/Home");
        }

        #endregion

        #region ForgetPassword
        //***************** Forget Password *********************
        #region Forget Password
        public IActionResult ForgetPassword()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgetPassword(AccountVM account)
        {
            AppUser user = await userManager.FindByEmailAsync(account.User.Email);
            if (user == null) BadRequest();
            string token = await userManager.GeneratePasswordResetTokenAsync(user);

            string link = Url.Action(nameof(ResetPassword), "Account", new { email = user.Email, token },
                Request.Scheme, Request.Host.ToString());

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("hrmshrms2000@gmail.com", "Antomi Reset Password");
            mail.To.Add(new MailAddress(user.Email));
            mail.Subject = "Reset Password";
            string body = string.Empty;
            using (StreamReader reader = new StreamReader("wwwroot/template/ResetPassword.html"))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{{link}}", link);
            mail.Body = body;
            mail.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient();
            //  SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.Credentials = new NetworkCredential("hrmshrms2000@gmail.com", "hrms12345");
            smtp.Send(mail);

            return RedirectToAction(nameof(Login), "Account");
        }

        // ********************* Reset Pasword *********************

        public async Task<IActionResult> ResetPassword(string email, string token)
        {
            AppUser user = await userManager.FindByEmailAsync(email);
            if (user == null) return BadRequest();

            AccountVM account = new AccountVM
            {
                User = user,
                Token = token
            };

            return View(account);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(AccountVM account)
        {
            AppUser user = userManager.FindByEmailAsync(account.User.Email).Result;
            if (user == null) return BadRequest();

            AccountVM model = new AccountVM
            {
                User = user,
                Token = account.Token
            };
            if (!ModelState.IsValid) return View(model);
            IdentityResult result = await userManager.ResetPasswordAsync(user, account.Token, account.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }


            return RedirectToAction(nameof(Login), "Account");
        }

        #endregion

        #endregion

    }
}
