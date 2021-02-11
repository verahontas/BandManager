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

        /// <summary>
        /// List of users.
        /// </summary>
        /// <hu>Az összes regisztrált felhasználó listája.</hu>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "administrator")]
        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.AsEnumerable<User>();
            List<UserViewModel> viewModel = new List<UserViewModel>();

            foreach (var user in users)
            {
                UserViewModel item = new UserViewModel();

                if (await _userManager.IsInRoleAsync(user, "musician"))
                {
                    item.UserRole = "zenész";
                    item.BandName = user.DefaultBandName;
                }
                else if (await _userManager.IsInRoleAsync(user, "owner"))
                {
                    item.UserRole = "tulajdonos";
                }
                else
                {
                    item.UserRole = "adminisztrátor";
                }

                item.UserOwnName = user.UserOwnName;
                item.UserEmail = user.Email;
                item.UserPhoneNumber = user.PhoneNumber;
                item.UserName = user.UserName;
                item.UserId = user.Id;

                viewModel.Add(item);
            }

            return View(viewModel);
        }

        /// <summary>
        /// Gets user details.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Authorize]
        public async Task<IActionResult> Details(int? userId)
        {
            if (userId == null)
            {
                if (User.IsInRole("administrator"))
                {
                    TempData["DangerAlert"] = "Hiba történt, próbálja újra!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["DangerAlert"] = "Hiba történt, próbálja újra!";
                    return RedirectToAction("Index", "RehearsalRooms");
                }
            }

            User user = await _userManager.FindByIdAsync(userId.ToString());

            UserViewModel viewModel = new UserViewModel();
            viewModel.UserId = user.Id;
            viewModel.UserOwnName = user.UserOwnName;
            viewModel.UserName = user.UserName;
            viewModel.UserEmail = user.Email;
            viewModel.UserPhoneNumber = user.PhoneNumber;

            if (await _userManager.IsInRoleAsync(user, "musician"))
            {
                viewModel.UserRole = "musician";
                viewModel.BandName = user.DefaultBandName;
            }
            else if (await _userManager.IsInRoleAsync(user, "owner"))
                viewModel.UserRole = "owner";
            else if (await _userManager.IsInRoleAsync(user, "administrator"))
                viewModel.UserRole = "administrator";

            return View(viewModel);
        }

        /// <summary>
        /// This function is to modify a profile.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Edit form with the user's details.</returns>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(int? userId)
        {
            if (userId == null)
            {
                if (User.IsInRole("administrator"))
                {
                    TempData["DangerAlert"] = "Hiba történt, próbálja újra!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["DangerAlert"] = "Hiba történt, próbálja újra!";
                    return RedirectToAction("Index", "RehearsalRooms");
                }
            }

            User user = await _userManager.FindByIdAsync(userId.ToString());

            UserViewModel viewModel = new UserViewModel
            {
                UserOwnName = user.UserOwnName,
                UserName = user.UserName,
                UserPhoneNumber = user.PhoneNumber,
                UserEmail = user.Email,
                BandName = "",
                UserId = user.Id
            };

            if (await _userManager.IsInRoleAsync(user, "musician"))
            {
                viewModel.UserRole = "musician";
                viewModel.BandName = user.DefaultBandName;
            }
            else if (await _userManager.IsInRoleAsync(user, "owner"))
            {
                viewModel.UserRole = "owner";
            }

            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(UserViewModel model)
        {
            User user = await _userManager.FindByIdAsync(model.UserId.ToString());

            user.UserOwnName = model.UserOwnName;
            user.UserName = model.UserName;
            user.PhoneNumber = model.UserPhoneNumber;
            user.Email = model.UserEmail;

            if (model.UserRole == "musician")
                user.DefaultBandName = model.BandName;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                TempData["DangerAlert"] = "Adatok módosítása sikertelen!";
                if (User.IsInRole("administrator"))
                    return RedirectToAction("Index");
                else
                    return RedirectToAction("Details", new { userId = model.UserId });
            }

            TempData["SuccessAlert"] = "Adatok módosítása sikeres!";
            return RedirectToAction("Details", new { userId = model.UserId });
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

            return RedirectToLocal(returnUrl);
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

        /*
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
        */

        [AllowAnonymous]
        [HttpGet]
        public IActionResult RegisterAsMusician()
        {
            RegistrationViewModel viewModel = new RegistrationViewModel
            {
                UserRole = "musician"
            };

            return View("Registration", viewModel);
        }

        [Authorize(Roles = "administrator")]
        [HttpGet]
        public IActionResult RegisterAsOwner()
        {
            RegistrationViewModel viewModel = new RegistrationViewModel
            {
                UserRole = "owner"
            };

            return View("Registration", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration(RegistrationViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.DangerAlert = "Sikertelen regisztráció";
                return View("Registration", viewModel);
            }

            User user = new User
            {
                UserName = viewModel.UserName,
                Email = viewModel.UserEmail,
                UserOwnName = viewModel.UserOwnName,
                PhoneNumber = viewModel.UserPhoneNumber
            };

            if (viewModel.UserRole == "musician")
                user.DefaultBandName = viewModel.BandName;

            var result = await _userManager.CreateAsync(user, viewModel.UserPassword);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
                TempData["DangerAlert"] = "Sikertelen regisztráció";
                return View("Registration", viewModel);
            }

            if (viewModel.UserRole == "musician")
                await _userManager.AddToRoleAsync(user, "musician");
            else if (viewModel.UserRole == "owner")
                await _userManager.AddToRoleAsync(user, "owner");

            //if the administrator added a user then no need to log in him
            if (User.IsInRole("administrator"))
            {
                if (viewModel.UserRole == "musician")
                    TempData["SuccessAlert"] = "Zenész regisztrálása sikeresen megtörtént!";
                else if (viewModel.UserRole == "owner")
                    TempData["SuccessAlert"] = "Tulajdonos regisztrálása sikeresen megtörtént!";

                return RedirectToAction("Index");
            }

            await _signInManager.SignInAsync(user, false);
            _applicationState.UserCount++;
            TempData["SuccessAlert"] = "Sikeresen bejelentkezve!";

            if (viewModel.UserRole == "musician")
                return RedirectToAction("Index", "RehearsalRooms");
            else //role = owner
                return RedirectToAction("Index", "RehearsalStudios");

        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            _applicationState.UserCount--;
            TempData["SuccessAlert"] = "Sikeres kijelentkezés!";
            return RedirectToAction("Index", "RehearsalRooms");
        }

        [Authorize]
        public PartialViewResult ChangeProfilePicturePartial(int? userId)
        {
            if (userId == null)
                return null;

            ImageUploadViewModel viewModel = new ImageUploadViewModel
            {
                EntityId = (int)userId
            };


            return PartialView("_ChangeProfilePicturePartial", viewModel);
        }

        [Authorize]
        [HttpPost]
        public IActionResult ChangeProfilePicture(ImageUploadViewModel viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["DangerAlert"] = "Hiba történt, próbálja újra.";
                    return RedirectToAction("Details", new { userId = viewModel.EntityId });
                }
                    

                if (viewModel.Images == null || viewModel.Images.Count == 0)
                {
                    TempData["DangerAlert"] = "Válasszon ki egy képet!";
                    return RedirectToAction("Details", new { userId = viewModel.EntityId });
                }

                //since we want to change the user's profile picture, the viewModel.Images can contain only 1 element
                //because we don't let the user to upload multiple files
                //it is restricted in the frontend page
                string ext = Path.GetExtension(viewModel.Images.First().FileName);
                if (ext != ".jpg" && ext != ".JPG" && ext != ".png" && ext != ".PNG")
                {
                    TempData["DangerAlert"] = "A profilkép formátuma JPG vagy PNG lehet.";
                    ModelState.AddModelError("", "A profilkép formátuma JPG vagy PNG lehet.");
                    return RedirectToAction("Details", new { userId = viewModel.EntityId });
                }

                if (!_reservationService.UpdateProfilePicture(viewModel.EntityId, viewModel.Images.First()))
                {
                    TempData["DangerAlert"] = "Hiba történt, próbálja újra!";
                    ModelState.AddModelError("", "Valami hiba történt, próbálja újra!");
                    return RedirectToAction("Details", new { userId = viewModel.EntityId });
                }
            }
            catch
            {
                TempData["DangerAlert"] = "Hiba történt, próbálja újra!";
                ModelState.AddModelError("", "Hiba történt, próbálja újra!");
                return RedirectToAction("Details", new { userId = viewModel.EntityId });
            }

            TempData["SuccessAlert"] = "Profilkép megváltoztatása sikeres!";
            return RedirectToAction("Details", new { userId = viewModel.EntityId });
        }

        /// <summary>
        /// As the admin cannot change anyone's password,
        /// anyone who calls this function will change his own password.
        /// So no need for any parameter.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            ChangePasswordViewModel viewModel = new ChangePasswordViewModel
            {
                UserId = user.Id
            };

            return View("ChangePassword", viewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel viewModel)
        {
            User user = await _userManager.FindByIdAsync(viewModel.UserId.ToString());

            var result = await _userManager.ChangePasswordAsync(user, viewModel.OldPassword, viewModel.NewPassword);

            if (!result.Succeeded)
            {
                TempData["DangerAlert"] = "Jelszó módosítása sikertelen, próbálja újra!";
                return View("ChangePassword", viewModel);
            }

            TempData["SuccessAlert"] = "Jelszó módosítása sikeres!";
            return RedirectToAction("Details", new { userId = viewModel.UserId });
        }

        [Authorize(Roles = "administrator")]
        [HttpGet]
        public IActionResult Create(string role)
        {
            if (role == "musician")
            {
                return RedirectToAction("RegisterAsMusician");
            }
            else
            {
                return RedirectToAction("RegisterAsOwner");
            }
        }

        [AllowAnonymous]
        public FileResult GetUserImage(int? userId)
        {
            if (userId == null)
                return File("~/images/noprofilepicture.jpg", "image/jpeg");

            byte[] imageContent = _reservationService.GetUserImage(userId);

            if (imageContent == null)
                return File("~/images/noprofilepicture.jpg", "image/jpeg");

            return File(imageContent, "image/jpeg");
        }

        [Authorize(Roles = "administrator")]
        [HttpGet]
        public async Task<IActionResult> Delete(int? userId)
        {
            if (userId == null)
                return NotFound();

            string stringId = userId.ToString();
            User user = await _userManager.FindByIdAsync(stringId);

            if (user == null)
                return NotFound();

            return View(user);
        }

        [Authorize(Roles = "administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                TempData["DangerAlert"] = "A felhasználó törlése sikertelen, kérjük próbálja újra!";
                return View(user);
            }

            TempData["SuccessAlert"] = "Felhasználó törlése sikeres!";
            return RedirectToAction("Index");
        }
    }
}
