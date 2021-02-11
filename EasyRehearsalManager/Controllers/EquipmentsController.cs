using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyRehearsalManager.Model;
using EasyRehearsalManager.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;

namespace EasyRehearsalManager.Web.Controllers
{
    public class EquipmentsController : BaseController
    {
        UserManager<User> _userManager;

        public EquipmentsController(IReservationService reservationService, ApplicationState applicationState,
            UserManager<User> userManager) 
            : base(reservationService, applicationState)
        {
            _userManager = userManager;
        }

        [Authorize(Roles = "owner, administrator")]
        public IActionResult Index(int? studioId)
        {
            if (studioId == null)
                return NotFound();

            var equipments = _reservationService.Equipments.Where(l => l.StudioId == studioId);

            string studioName = _reservationService.Studios.FirstOrDefault(l => l.Id == studioId).Name;
            ViewBag.StudioName = studioName;

            return View(equipments.ToList());
        }

        [Authorize(Roles="owner, administrator")]
        [HttpGet]
        public IActionResult Create(int? studioId)
        {
            if (studioId == null)
            {
                TempData["DangerAlert"] = "Létrehozás sikertelen, próbálja újra!";
                return RedirectToAction("Index", "Home");
            }

            Equipment equipment = new Equipment
            {
                StudioId = (int)studioId
            };

            string studioName = _reservationService.Studios.FirstOrDefault(l => l.Id == studioId).Name;
            ViewBag.StudioName = studioName;

            return View(equipment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Equipment equipment)
        {
            if (_reservationService.AddEquipment(equipment))
            {
                TempData["SuccessAlert"] = "Eszköz mentése sikeres!";
                return RedirectToAction("Details", "RehearsalStudios", new { studioId = equipment.StudioId });
            }

            TempData["DangerAlert"] = "Eszköz mentése sikertelen!";
            return RedirectToAction("Details", "RehearsalStudios", new { studioId = equipment.StudioId });
        }

        [Authorize(Roles = "owner, administrator")]
        [HttpGet]
        public IActionResult Edit(int? equipmentId)
        {
            if (equipmentId == null)
            {
                TempData["DangerAlert"] = "Eszköz módosítása sikertelen, próbálja újra!";
                return RedirectToAction("Index", "Home");
            }

            Equipment equipment = _reservationService.Equipments.FirstOrDefault(l => l.Id == equipmentId);
            return View(equipment);
        }

        [HttpPost]
        public IActionResult Edit(Equipment equipment)
        {
            if (_reservationService.UpdateEquipment(equipment))
                return RedirectToAction("Index", new { studioId = equipment.StudioId });

            TempData["DangerAlert"] = "Módosítás sikertelen!";
            return RedirectToAction("Edit", new { equipmentId = equipment.StudioId });
        }

        [Authorize(Roles = "owner, administrator")]
        public IActionResult Delete(int? equipmentId)
        {
            if (equipmentId == null)
            {
                return NotFound();
            }

            var equipment = _reservationService.GetEquipment(equipmentId);

            if (equipment != null)
                return View(equipment);

            return NotFound();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int equipmentId)
        {
            //we need to get this ID before the equipment is deleted.
            var studioId = _reservationService.GetStudioIdByEquipment(equipmentId);
            if (_reservationService.RemoveEquipment(equipmentId))
            {
                TempData["SuccessAlert"] = "Az eszközt sikeresen töröltük!";
                if (studioId != null)
                {
                    return RedirectToAction("Index", new { studioId = studioId });
                }
                else
                {
                    return RedirectToAction("Index", "RehearsalStudios");
                }
            }

            TempData["DangerAlert"] = "Az eszköz törlése sikertelen!";
            var equipment = _reservationService.GetEquipment(equipmentId);
            return View(equipment);
        }
    }
}
