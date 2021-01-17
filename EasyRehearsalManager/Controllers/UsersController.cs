﻿using System;
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
    [Authorize(Roles = "administrator")]
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

        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.AsEnumerable<User>();
            List<UserListItemViewModel> usersWithRoles = new List<UserListItemViewModel>();

            foreach (var user in users)
            {
                UserListItemViewModel viewModel = new UserListItemViewModel();

                if (await _userManager.IsInRoleAsync(user, "musician"))
                {
                    viewModel.Role = "zenész";
                }
                else if (await _userManager.IsInRoleAsync(user, "owner"))
                {
                    viewModel.Role = "tulajdonos";
                }
                else
                {
                    viewModel.Role = "adminisztrátor";
                }

                viewModel.UserOwnName = user.UserOwnName;
                viewModel.UserEmail = user.Email;
                viewModel.UserPhoneNumber = user.PhoneNumber;
                viewModel.UserName = user.UserName;
                viewModel.BandName = user.DefaultBandName;
                viewModel.Id = user.Id;

                usersWithRoles.Add(viewModel);
            }

            return View(usersWithRoles);
        }

        /* I dont need this... really unnecessarry function...
        [Authorize(Roles = "administrator")]
        public async Task<IActionResult> Details(int? userId)
        {
            User user = await _userManager.FindByIdAsync(userId.ToString());

            UserListItemViewModel viewModel = new UserListItemViewModel();

            viewModel.UserOwnName = user.UserOwnName;
            viewModel.UserName = user.UserName;
            viewModel.UserEmail = user.Email;
            viewModel.UserPhoneNumber = user.PhoneNumber;
            viewModel.BandName = user.DefaultBandName;
        }
        */

        [HttpGet]
        public IActionResult Create(string role)
        {
            if (role == "musician")
            {
                return RedirectToAction("RegisterAsMusician", "Account");
            }
            else
            {
                return RedirectToAction("RegisterAsOwner", "Account");
            }
        }

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
            return RedirectToAction("Index", "Users");
        }
    }
}
