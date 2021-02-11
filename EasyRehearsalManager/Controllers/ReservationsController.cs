using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EasyRehearsalManager.Model;
using EasyRehearsalManager.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.AspNetCore.Authorization;

namespace EasyRehearsalManager.Web.Controllers
{
    public class ReservationsController : BaseController
    {
        private readonly UserManager<User> _userManager;

        public ReservationsController(IReservationService reservationService, ApplicationState applicationState,
            UserManager<User> userManager)
            : base(reservationService, applicationState)
        {
            _userManager = userManager;
        }

        [Authorize]
        // GET: Reservations
        public IActionResult Index()
        {
            /*
            if (!User.Identity.IsAuthenticated)
            {
                TempData["DangerAlert"] = "A foglalások listázásához jelentkezzen be!";
                return RedirectToAction("Login", "Account");
            }
            */
            string userId = _userManager.GetUserId(User);

            if (User.IsInRole("administrator"))
                return View(_reservationService.Reservations);
            else if (User.IsInRole("owner")) //a saját stúdióiba rögzített foglalások
                return View(_reservationService.GetReservations(Int32.Parse(userId), "owner"));
            else
                return View(_reservationService.GetReservations(Int32.Parse(userId), "musician"));
        }

        [Authorize]
        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(int? reservationId)
        {
            if (reservationId == null)
            {
                return NotFound();
            }

            var reservation = _reservationService.GetReservation(reservationId);
            
            if (reservation == null)
            {
                return NotFound();
            }
            if (User.IsInRole("musician"))
            {
                reservation.User = await _userManager.FindByIdAsync(_userManager.GetUserId(User));
            }
            else //owner or admin
            {
                int userId = (int)reservation.UserId;
                User user = await _userManager.FindByIdAsync(userId.ToString());
                reservation.User = user;
            }
            List<string> equipmentNames = _reservationService.GetEquipmentNamesForReservation(reservationId);

            ViewBag.Equipments = equipmentNames;

            return View(reservation);
        }

        /// <summary>
        /// Only authenticated users can make a reservation.
        /// Musicians can book only for themselves.
        /// Owners can book for anybody but only to their own rooms.
        /// Administrator can book for anybody to any room.
        /// DayIndex and Hour is used when reservation is made from table.
        /// </summary>
        /// <param name="roomId">Cannot be null. Reservation can be only made from those page that know the roomId.</param>
        /// <param name="dayIndex">If not null then we know which day to book to. If 0 then the default day is today.</param>
        /// <param name="hour">If it is null then the default starting hour is next hour.</param>
        /// <returns></returns>
        [Authorize]
        public async Task<IActionResult> Create(int? roomId, int? dayIndex, int? hour)
        {
            if (roomId == null)
            {
                TempData["DangerAlert"] = "Hiba lépett fel a foglalás során!";
                return RedirectToAction("Index", "RehearsalRooms");
            }

            RehearsalRoom room = _reservationService.GetRoom(roomId);

            if (room == null)
            {
                TempData["DangerAlert"] = "Hiba lépett fel a foglalás során!";
                return RedirectToAction("Index", "RehearsalRooms");
            }

            ReservationViewModel viewModel = new ReservationViewModel
            {
                RoomId = room.Id,
                RoomNumber = room.Number,
                StudioName = room.Studio.Name,
                Day = dayIndex == null ? DateTime.Now : DateTime.Now.AddDays((int)dayIndex),
                StartHour = hour == null ? DateTime.Now.Hour + 1 : (int)hour,
                EndHour = hour == null ? DateTime.Now.Hour + 3 : (int)hour + 2
            };

            if (User.IsInRole("musician"))
            {
                User user = await _userManager.FindByNameAsync(User.Identity.Name);

                if (user == null) //just to be sure
                {
                    TempData["DangerAlert"] = "Hiba lépett fel a foglalás során!";
                    return RedirectToAction("Index", "RehearsalRooms");
                }

                viewModel.UserId = user.Id;
                viewModel.BandName = user.DefaultBandName;
                viewModel.UserOwnName = user.UserOwnName;
            }
            else //owner or administrator
            {
                List<User> users = _reservationService.Users.ToList();
                ViewData["Users"] = new SelectList(users, "Id", "UserOwnName");
            }

            RehearsalStudio studio = _reservationService.GetStudioByRoomId(roomId);

            //get all equipment that can be booked
            foreach (var equipment in _reservationService.GetEquipmentsForStudio(studio.Id).ToList())
            {
                viewModel.Equipments[equipment.Name] = false;
            }

            List<int> hours = new List<int>();

            //itt attól függően legyenek benne a hours-ban az órák hogy melyik napra akar a felhasználó foglalni
            //ez a fv csak akkor hívódik meg, amikor nem konkrétan táblázatból foglalunk, hanem pl a termek felsorolásánál van közvetlenül foglalás gomb.
            //tehát vagy ahhoz kell igazítani a viewbag elemeit, hogy a felhasználó milyen dátumot válaszott ki,
            //vagy a foglalás ellenőrzésekor kell ellenőrizni azt is, hogy adott napon a megadott időpontban nyitva van-e a terem.
            //we need to add all opening hours, because the date can be modified in the create form
            //so when validating the reservation we have to check whether on the selected date in the selected hour the room is open
            for (int i = studio.EarliestOpeningHour(); i <= studio.LatestClosingHour(); ++i)
            {
                hours.Add(i);
            }

            ViewData["Hours"] = new SelectList(hours);
            return View(viewModel);
        }

        /// <summary>
        /// If the administrator made a reservation for a selected user,
        /// then all properties of the user will be unset,
        /// except the UserId. So in this case we have to find the user and set the properties.
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReservationViewModel viewModel)
        {
            if (viewModel == null)
            {
                TempData["DangerAlert"] = "Hiba lépett fel a foglalás során, kérjük próbálja újra!";
                return RedirectToAction("Index", "RehearsalRooms");
            }

            DateTime day = viewModel.Day;
            DateTime startDate = new DateTime(day.Year, day.Month, day.Day, viewModel.StartHour, 0, 0);
            DateTime endDate = new DateTime(day.Year, day.Month, day.Day, viewModel.EndHour, 0, 0);

            //a reservationDateErrorban kéne azt is ellenőrizni hogy az adott időpontban foglalt-e a terem
            //ellenőrizni kell azt is, hogy nyitva van-e a terem
            //valahol ellenőrizni kell azt is hogy az eszközofoglalás valid-e
            //itt mindegy mi a reservation id-ja, mert létrehozáskor minden foglalást megvizsgálja
            switch (_reservationService.ValidateReservation(startDate, endDate, "create", 1, viewModel.Equipments, viewModel.RoomId))
            {
                case ReservationDateError.StartInvalid:
                    ModelState.AddModelError("StartHour", "A kezdés dátuma nem megfelelő!");
                    break;
                case ReservationDateError.EndInvalid:
                    ModelState.AddModelError("EndHour", "A megadott foglalási idő érvénytelen (a foglalás vége korábban van, mint a kezdete)!");
                    break;
                case ReservationDateError.Conflicting:
                    ModelState.AddModelError("StartHour", "A megadott időpontban a terembe már van foglalás!");
                    break;
                case ReservationDateError.LengthInvalid:
                    ModelState.AddModelError("EndHour", "Üres az időintervallum!");
                    break;
                case ReservationDateError.EquipmentNotAvailable:
                    ModelState.AddModelError("EndHour", "A kiválasztott eszköz már foglalt ebben az időpontban!"); //itt nem tudom hogyan lehetne az equipments-hez hozzárendelni az üzenetet
                    break;
            }

            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var error in allErrors)
            {
                Debug.WriteLine(error);
            }

            if (!ModelState.IsValid)
            {
                List<int> hours = new List<int>();

                RehearsalStudio studio = _reservationService.GetStudioByRoomId(viewModel.RoomId);

                for (int i = studio.EarliestOpeningHour(); i <= studio.LatestClosingHour(); ++i)
                {
                    hours.Add(i);
                }

                ModelState.AddModelError("", "A foglalás sikertelen!");
                TempData["DangerAlert"] = "A foglalás sikertelen!";
                ViewData["Hours"] = new SelectList(hours);
                ViewData["Users"] = new SelectList(_reservationService.Users, "Id", "Name");
                return View("Create", viewModel);
            }

            if (!await _reservationService.SaveReservationAsync(viewModel.RoomId, viewModel.UserId, viewModel))
            {
                ModelState.AddModelError("", "A foglalás sikertelen!");
                TempData["DangerAlert"] = "A foglalás sikertelen!";
                List<int> hours = new List<int>();

                RehearsalStudio studio = _reservationService.GetStudioByRoomId(viewModel.RoomId);

                for (int i = studio.EarliestOpeningHour(); i <= studio.LatestClosingHour(); ++i)
                {
                    hours.Add(i);
                }

                ViewData["Hours"] = new SelectList(hours);
                ViewData["Users"] = new SelectList(_reservationService.Users, "Id", "Name");
                return View("Create", viewModel);
            }

            User user = await _userManager.FindByIdAsync(viewModel.UserId.ToString());
            viewModel.UserOwnName = user.UserOwnName;
            viewModel.BandName = user.DefaultBandName;
            TempData["SuccessAlert"] = "A foglalást sikeresen rögzítettük!";
            return View("Result", viewModel);
        }

        [Authorize]
        [HttpGet]
        public IActionResult CreateFromTable(int dayIndex, int? roomId, int? hour)
        {
            if (roomId == null || hour == null)
                return NotFound();

            return RedirectToAction("Create", new { roomId = roomId, dayIndex = dayIndex, hour = hour });
            //ez ugye a create get methodra irányít át?
        }

        [Authorize]
        // GET: Reservations/Edit/5
        public async Task<IActionResult> Edit(int? reservationId)
        {
            if (reservationId == null)
            {
                return NotFound();
            }

            var reservation = _reservationService.GetReservation(reservationId);

            if (reservation == null)
            {
                return NotFound();
            }
            /*
            string userId = _userManager.GetUserId(User);
            User user = await _userManager.FindByIdAsync(userId);
            */
            reservation.User = await _userManager.FindByIdAsync(reservation.UserId.ToString());

            //a musician can edit only his own reservatrions.
            if (User.IsInRole("musician") && reservation.User.Id != Int32.Parse(_userManager.GetUserId(User)))
            {
                TempData["DangerAlert"] = "Más foglalását nem módosíthatja!";
                return RedirectToAction("Index");
            }

            ViewData["RehearsalRoomId"] = new SelectList(_reservationService.Rooms, "Id", "Number", reservation.RehearsalRoomId);
            
            List<int> hours = new List<int>();

            for (int i = reservation.RehearsalRoom.Studio.EarliestOpeningHour(); i <= reservation.RehearsalRoom.Studio.LatestClosingHour(); ++i)
            {
                hours.Add(i);
            }

            ViewData["Hours"] = new SelectList(hours);

            ReservationViewModel viewModel = new ReservationViewModel
            {
                UserId = reservation.User.Id,
                UserOwnName = reservation.User.UserOwnName,
                RoomId = reservation.RehearsalRoom.Id,
                RoomNumber = reservation.RehearsalRoom.Number,
                StudioName = reservation.RehearsalRoom.Studio.Name,
                BandName = reservation.BandName,
                Day = reservation.Start,
                StartHour = reservation.Start.Hour,
                EndHour = reservation.End.Hour,
                ReservationId = reservation.Id
            };

            Dictionary<string, int> equipments = new Dictionary<string, int>();
            /*
            foreach (var e in reservation.Equipments)
            {
                viewModel.Equipments[e.Name] = true;
            }
            */
            foreach (var pair in _reservationService.GetReservationEquipmentPairsForReservarion(reservationId))
            {
                viewModel.Equipments[pair.EquipmentName] = true;
            }
            
            RehearsalRoom room = _reservationService.GetRoom(reservation.RehearsalRoomId);
            RehearsalStudio studio = _reservationService.Studios.FirstOrDefault(l => l.Rooms.Contains(room));

            foreach (var e in studio.Equipments)
            {
                if (!viewModel.Equipments.ContainsKey(e.Name)) //ha nincs foglalva az aktuális foglaláshoz
                {
                    viewModel.Equipments[e.Name] = false;
                }
            }

            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ReservationViewModel reservationViewModel)
        {
            Reservation reservation = _reservationService.Reservations.FirstOrDefault(l => l.Id == reservationViewModel.ReservationId);

            if (reservation == null)
            {
                TempData["DangerAlert"] = "Foglalás módosítása sikertelen, kérjük próbálja újra!";
                return RedirectToAction("Index");
            }

            reservation.Start = new DateTime(
                reservationViewModel.Day.Year,
                reservationViewModel.Day.Month,
                reservationViewModel.Day.Day,
                reservationViewModel.StartHour, 0, 0
            );

            reservation.End = new DateTime(
                reservationViewModel.Day.Year,
                reservationViewModel.Day.Month,
                reservationViewModel.Day.Day,
                reservationViewModel.EndHour, 0, 0
            );

            string userId = _userManager.GetUserId(User);
            User user = await _userManager.FindByIdAsync(userId);
            reservation.User = user;

            reservation.BandName = reservationViewModel.BandName;

            DateTime day = reservationViewModel.Day;
            DateTime startDate = new DateTime(day.Year, day.Month, day.Day, reservation.Start.Hour, 0, 0);
            DateTime endDate = new DateTime(day.Year, day.Month, day.Day, reservation.End.Hour, 0, 0);

            switch (_reservationService.ValidateReservation(startDate, endDate, "edit", reservation.Id, reservationViewModel.Equipments, reservation.RehearsalRoomId))
            {
                case ReservationDateError.StartInvalid:
                    ModelState.AddModelError("StartHour", "A kezdés dátuma nem megfelelő!");
                    break;
                case ReservationDateError.EndInvalid:
                    ModelState.AddModelError("EndHour", "A megadott foglalási idő érvénytelen (a foglalás vége korábban van, mint a kezdete)!");
                    break;
                case ReservationDateError.Conflicting:
                    ModelState.AddModelError("StartHour", "A megadott időpontban a terembe már van foglalás!");
                    break;
                case ReservationDateError.LengthInvalid:
                    ModelState.AddModelError("EndHour", "Üres az időintervallum!");
                    break;
                case ReservationDateError.EquipmentNotAvailable:
                    ModelState.AddModelError("Endhour", "A bérelni kívánt eszközből a megadott időpontban már az összes foglalt!");
                    break;
            }

            RehearsalRoom room = _reservationService.GetRoom(reservation.RehearsalRoomId);
            RehearsalStudio studio = _reservationService.Studios.FirstOrDefault(l => l.Rooms.Contains(room));
            /*
            foreach (var e in reservationViewModel.Equipments) //ebben az összes stúdióban bérelhető eszköz benne van
            {
                Equipment eq = studio.Equipments.FirstOrDefault(l => l.Name == e.Key);
                //ha már benne van a foglalásban de lemondtuk:
                if (!e.Value && reservation.Equipments.Contains(eq))
                {
                    reservation.Equipments.Remove(eq);
                }
                else if (e.Value && !reservation.Equipments.Contains(eq)) //ehelyett az egész helyett kellene a pairs táblát módosítani
                {
                    reservation.Equipments.Add(eq);
                }
            }
            */

            if (ModelState.IsValid)
            {
                try
                {
                    _reservationService.UpdateReservation(reservation);
                    _reservationService.UpdateReservationEquipmentTable(reservationViewModel.ReservationId, studio.Id, reservationViewModel.Equipments);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["SuccessAlert"] = "Foglalását sikeresen módosította!";
                return RedirectToAction(nameof(Index));
            }

            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);

            //reservationViewModel.RoomId = reservation.RehearsalRoom;
            reservationViewModel.ReservationId = reservation.Id;
            TempData["DangerAlert"] = "Foglalás módosítása sikertelen!";

            List<int> hours = new List<int>();
            //itt attól függően legyenek benne a hours-ban az órák hogy melyik napra akar a felhasználó foglalni
            //some killing dynamic stuff.....
            for (int i = reservation.RehearsalRoom.Studio.OpeningHourMonday; i <= reservation.RehearsalRoom.Studio.ClosingHourMonday; ++i)
            {
                hours.Add(i);
            }

            ViewData["Hours"] = new SelectList(hours);
            return View(reservationViewModel);
        }

        [Authorize]
        public async Task<IActionResult> Delete(int? reservationId)
        {
            if (reservationId == null)
            {
                return NotFound();
            }

            var reservation = _reservationService.GetReservation(reservationId);

            string userId = _userManager.GetUserId(User);
            User user = await _userManager.FindByIdAsync(userId);

            reservation.User = user;

            if (reservation != null)
                return View(reservation);

            return NotFound();
        }

        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int reservationId)
        {
            var reservation = _reservationService.GetReservation(reservationId);

            //if the reservation starts in the next 72 hour then we cannot delete it
            //but the administrator and the owner can delete it anytime
            if (User.IsInRole("musician"))
            {
                if (reservation.Start < DateTime.Now.AddDays(3))
                {
                    TempData["DangerAlert"] = "Foglalás lemondása sikertelen! Próbát lemondani legkésőbb a foglalás előtt 72 órával lehetséges!";
                    return RedirectToAction("Details", new { reservationId = reservationId });
                }
            }

            User user = await _userManager.FindByNameAsync(User.Identity.Name);

            ReservationViewModel model = new ReservationViewModel
            {
                Day = reservation.Start.Date,
                StartHour = reservation.Start.Hour,
                EndHour = reservation.End.Hour,
                RoomId = reservation.RehearsalRoom.Id,
                RoomNumber = reservation.RehearsalRoom.Number,
                StudioName = reservation.RehearsalRoom.Studio.Name,
                BandName = reservation.BandName,
                UserOwnName = user.UserOwnName
            };

            if (_reservationService.RemoveReservation(reservationId))
            {
                TempData["SuccessAlert"] = "Foglalását sikeresen töröltük!";
                return View("Result", model);
            }

            TempData["DangerAlert"] = "Foglalás törlése sikertelen, kérjük próbálja újra!";
            return View(reservation);
        }

        private bool ReservationExists(int reservationId)
        {
            return _reservationService.ReservationExist(reservationId);
        }
    }
}
