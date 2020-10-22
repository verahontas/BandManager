using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Threading;
using System.Threading.Tasks;
using EasyRehearsalManager.Model;
using EasyRehearsalManager.Web.Models;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;
using SQLitePCL;

namespace EasyRehearsalManager.Web.Controllers
{
    public class AccountController : BaseController
    {
        private readonly UserManager<User> _userManager;

        private readonly SignInManager<User> _signInManager;

        public AccountController(IReservationService reservationService, ApplicationState applicationState,
            UserManager<User> userManager, SignInManager<User> signInManager)
            : base(reservationService, applicationState)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index(MusicianRegistrationViewModel model)
        {
            return View(model);
        }

        public async Task<IActionResult> GetProfileDetails()
        {
            User user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login");

            MusicianRegistrationViewModel model = new MusicianRegistrationViewModel
            {
                UserOwnName = user.UserOwnName,
                UserName = user.UserName,
                UserPhoneNumber = user.PhoneNumber,
                UserEmail = user.Email,
                BandName = user.DefaultBandName
            };

            return RedirectToAction("Index", model);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateProfile()
        {
            User user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (user == null)
                return RedirectToAction("Login");

            MusicianRegistrationViewModel model = new MusicianRegistrationViewModel
            {
                UserOwnName = user.UserOwnName,
                UserName = user.UserName,
                UserPhoneNumber = user.PhoneNumber,
                UserEmail = user.Email,
                BandName = ""
            };

            if (await _userManager.IsInRoleAsync(user, "musician"))
                model.BandName = user.DefaultBandName;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(MusicianRegistrationViewModel model)
        {
            User user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (user == null)
                return RedirectToAction("Login");

            user.UserOwnName = model.UserOwnName;
            user.UserName = model.UserName;
            user.PhoneNumber = model.UserPhoneNumber;
            user.Email = model.UserEmail;

            if (await _userManager.IsInRoleAsync(user, "musician"))
                user.DefaultBandName = model.BandName;

            await _userManager.UpdateAsync(user);

            TempData["SuccessAlert"] = "Adatok módosítása sikeres!";
            return RedirectToAction("Index", model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel user)
        {
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            if (!ModelState.IsValid)
            {
                TempData["DangerAlert"] = "Sikertelen bejelentkezés!";
                return View("Login", user);
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, user.UserPassword, user.RememberLogin, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Hibás felhasználónév, vagy jelszó.");
                TempData["DangerAlert"] = "Sikertelen bejelentkezés!";
                return View("Login", user);
            }

            _applicationState.UserCount++;

            //ViewBag.SuccessAlert = "Sikeres bejelentkezés!";


            
            return RedirectToAction("LoginSuccessful");
            //return RedirectToAction("Index", "RehearsalRooms");
            //viewbag becomes empty when calling 'RedirectToAction'
            //return View("~/Views/Home/Index.cshtml");

            //return RedirectToAction("LoginSuccessful");
            //must return RedirectToAction bc User.Identity gets the Username this way
            //then in LoginSuccessful method a view is returned so
            //i can put information into the viewbag without getting empty
        }

        
        public IActionResult LoginSuccessful()
        {
            TempData["SuccessAlert"] = "Sikeres bejelentkezés!";

            
            if (User.IsInRole("musician"))
            {
                TempData["CurrentUserRole"] = "musician";
                return RedirectToAction("Index", "RehearsalRooms");
            }
            else if (User.IsInRole("owner"))
            {
                TempData["CurrentUserRole"] = "owner";
                return RedirectToAction("Index", "RehearsalStudios");
            }
            else
            {
                TempData["CurrentUserRole"] = "administrator";
                return RedirectToAction("Index", "Home");
            }
            
        }
        
        [HttpGet]
        public IActionResult RegisterAsMusician()
        {
            return View("MusicianRegister");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterAsMusician(MusicianRegistrationViewModel user)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.DangerAlert = "Sikertelen regisztráció";
                return View("Register", user);
            }

            User guest = new User
            {
                UserName = user.UserName,
                Email = user.UserEmail,
                UserOwnName = user.UserOwnName,
                PhoneNumber = user.UserPhoneNumber,
                DefaultBandName = user.BandName
            };
            var result = await _userManager.CreateAsync(guest, user.UserPassword);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
                ViewBag.DangerAlert = "Sikertelen regisztráció";
                return View("Register", user);
            }
            await _userManager.AddToRoleAsync(guest, "Musician");

            await _signInManager.SignInAsync(guest, false);
            _applicationState.UserCount++;
            ViewBag.SuccessAlert = "Sikeresen bejelentkezve!";
            //ViewBag.CurrentUserName = user.UserName;
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult RegisterAsOwner()
        {
            return View("OwnerRegister");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterAsOwner(OwnerRegistrationViewModel user)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.DangerAlert = "Sikertelen regisztráció";
                return View("Register", user);
            }

            User guest = new User
            {
                UserName = user.UserName,
                Email = user.UserEmail,
                UserOwnName = user.UserOwnName,
                PhoneNumber = user.UserPhoneNumber
            };
            var result = await _userManager.CreateAsync(guest, user.UserPassword);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
                ViewBag.DangerAlert = "Sikertelen regisztráció";
                return View("Register", user);
            }

            await _userManager.AddToRoleAsync(guest, "Owner");

            await _signInManager.SignInAsync(guest, false);
            _applicationState.UserCount++;
            TempData["SuccessAlert"] = "Sikeresen bejelentkezve!";
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            _applicationState.UserCount--;
            TempData["SuccessAlert"] = "Sikeres kijelentkezés!";
            return RedirectToAction("Index", "Home");
        }
    }
}
