using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyRehearsalManager.Model;
using EasyRehearsalManager.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EasyRehearsalManager.Web.Controllers
{
    public class UsersController : BaseController
    {
        private readonly UserManager<User> _userManager;

        private readonly SignInManager<User> _signInManager;

        public UsersController(IReservationService reservationService, ApplicationState applicationState,
            UserManager<User> userManager, SignInManager<User> signInManager)
            : base(reservationService, applicationState)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [Authorize(Roles = "administrator")]
        public IActionResult Index()
        {
            var users = _userManager.Users.AsEnumerable<User>();
            return View(users);
        }

        [Authorize(Roles = "administrator")]
        [HttpGet]
        public async Task<IActionResult> Delete(int? userId)
        {
            if (userId == null)
                return NotFound();

            string stringId = userId.ToString();
            var user = await _userManager.FindByIdAsync(stringId);

            if (user == null)
                return NotFound();

            return View(user);
        }

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
            return RedirectToAction("Index", "Home");
        }
    }
}
