using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Threading;
using System.Threading.Tasks;
using EasyRehearsalManager.Model;
using EasyRehearsalManager.Web.Models;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        public IActionResult Index(MusicianRegistrationViewModel model)
        {
            return View(model);
        }

        [Authorize]
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

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> UpdateProfile(int? userId)
        {
            //if the admin wants to do some modification with that profile, then the given parameret is not null
            if (User.IsInRole("administrator") && userId != null)
            {
                User userToModify = await _userManager.FindByIdAsync(userId.ToString());


                MusicianRegistrationViewModel _model = new MusicianRegistrationViewModel
                {
                    UserOwnName = userToModify.UserOwnName,
                    UserName = userToModify.UserName,
                    UserPhoneNumber = userToModify.PhoneNumber,
                    UserEmail = userToModify.Email,
                    BandName = "",
                    UserId = userToModify.Id
                };

                if (await _userManager.IsInRoleAsync(userToModify, "musician"))
                    _model.BandName = userToModify.DefaultBandName;

                return View(_model);
            }

            User user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (user == null)
                return RedirectToAction("Login");

            MusicianRegistrationViewModel model = new MusicianRegistrationViewModel
            {
                UserOwnName = user.UserOwnName,
                UserName = user.UserName,
                UserPhoneNumber = user.PhoneNumber,
                UserEmail = user.Email,
                BandName = "",
                UserId = user.Id
            };

            if (await _userManager.IsInRoleAsync(user, "musician"))
                model.BandName = user.DefaultBandName;

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdateProfile(MusicianRegistrationViewModel model)
        {
            if (User.IsInRole("administrator"))
            {
                User _user = await _userManager.FindByIdAsync(model.UserId.ToString());

                _user.UserOwnName = model.UserOwnName;
                _user.UserName = model.UserName;
                _user.PhoneNumber = model.UserPhoneNumber;
                _user.Email = model.UserEmail;

                if (await _userManager.IsInRoleAsync(_user, "musician"))
                    _user.DefaultBandName = model.BandName;

                await _userManager.UpdateAsync(_user);

                TempData["SuccessAlert"] = "Adatok módosítása sikeres!";
                return RedirectToAction("Index", "Users");
            }

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

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            TempData["returnUrl"] = returnUrl;
            return View("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel user, string returnUrl)
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

            TempData["SuccessAlert"] = "Sikeres bejelentkezés!";

            
            //return RedirectToAction("LoginSuccessful");
            return RedirectToLocal(returnUrl);


            //return RedirectToAction("Index", "RehearsalRooms");
            //viewbag becomes empty when calling 'RedirectToAction'
            //return View("~/Views/Home/Index.cshtml");

            //return RedirectToAction("LoginSuccessful");
            //must return RedirectToAction bc User.Identity gets the Username this way
            //then in LoginSuccessful method a view is returned so
            //i can put information into the viewbag without getting empty
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "RehearsalRooms");
            }
        }


        public IActionResult LoginSuccessful()
        {
            TempData["SuccessAlert"] = "Sikeres bejelentkezés!";

            
            if (User.IsInRole("musician")) //ez kell ide? a basecontrollerben is bele van téve
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
        
        [AllowAnonymous]
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

            //ha a zenész regisztrálását az adminisztrátor végzi, akkor nem kell a regisztráltat bejelentkeztetni
            if (User.IsInRole("administrator"))
            {
                TempData["SuccessAlert"] = "Zenész regisztrálása sikeresen megtörtént!";
                return RedirectToAction("Index", "Users");
            }

            await _signInManager.SignInAsync(guest, false);
            _applicationState.UserCount++;
            TempData["SuccessAlert"] = "Sikeresen bejelentkezve!";
            //ViewBag.CurrentUserName = user.UserName;
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
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
                TempData["DangerAlert"] = "Sikertelen regisztráció";
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
                TempData["DangerAlert"] = "Sikertelen regisztráció";
                return View("Register", user);
            }

            await _userManager.AddToRoleAsync(guest, "Owner");

            //ha a tulajdonos regisztrálását az adminisztrátor végzi, akkor nem kell a regisztráltat bejelentkeztetni
            if (User.IsInRole("administrator"))
            {
                TempData["SuccessAlert"] = "Tulajdonos regisztrálása sikeresen megtörtént!";
                return RedirectToAction("Index", "Users");
            }

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
            return RedirectToAction("Index", "RehearsalRooms");
        }

        [HttpPost]
        public IActionResult ChangeProfilePicture(FileUpload obj)
        {
            /*
            if (viewModel.ProfilePicture != null)
            {
                if (_reservationService.UpdateProfilePicture(viewModel))
                {
                    TempData["SuccessAlert"] = "Profilkép megváltoztatása sikeres!";
                    return RedirectToAction("Index", "Account");
                }
            }
            else
            {
                TempData["DangerAlert"] = "Válasszon ki egy képet!";
            }
            */
            /*
            using (var binaryReader = new BinaryReader(Request.Files[0].InputStream))
            {
                obj.Photo = binaryReader.ReadBytes(Request.Files[0].ContentLength);
            }
            */
            return RedirectToAction("GetProfileDetails");
        }

        [HttpPost]
        public ActionResult Upload(System.Web.HttpPostedFileBase file)
        {
            if (file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/App_Data"), fileName);
                file.SaveAs(path);
            }
            return RedirectToAction("Index");
        }
    }
}
