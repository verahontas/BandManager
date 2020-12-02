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

        // GET: Reservations
        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["DangerAlert"] = "A foglalások listázásához jelentkezzen be!";
                return RedirectToAction("Login", "Account");
            }

            string userId = _userManager.GetUserId(User);
            User user = await _userManager.FindByIdAsync(userId);

            if (User.IsInRole("administrator"))
                return View(_reservationService.Reservations);
            else if (User.IsInRole("owner")) //a saját stúdióiba rögzített foglalások
                return View(_reservationService.GetReservations(Int32.Parse(userId), "owner"));
            else
                return View(_reservationService.GetReservations(Int32.Parse(userId), "musician"));
        }

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
            string userId = _userManager.GetUserId(User);
            User user = await _userManager.FindByIdAsync(userId);

            reservation.User = user;

            return View(reservation);
        }

        // GET: Reservations/Create
        public async Task<IActionResult> Create(int? roomId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["DangerAlert"] = "A foglaláshoz bejelentkezés szükséges!";
                return RedirectToAction("Login", "Account");
            }

            ReservationViewModel reservation = _reservationService.NewReservation(roomId);

            if (reservation == null)
            {
                TempData["DangerAlert"] = "Hiba lépett fel a foglalás során!";
                return RedirectToAction("Index", "Home");
            }

            if (User.Identity.IsAuthenticated)
            {
                User user = await _userManager.FindByNameAsync(User.Identity.Name);

                if (user != null)
                {
                    reservation.UserOwnName = user.UserOwnName;
                    reservation.BandName = user.DefaultBandName;
                    reservation.UserEmail = user.Email;
                    reservation.UserPhoneNumber = user.PhoneNumber;
                }
            }

            foreach (var equipment in _reservationService.GetEquipmentsForStudio(reservation.Room.StudioId).ToList())
            {
                /*reservation.Equipments.Add(new EquipmentToBook { 
                    IsChecked = false,
                    Equipment = equipment
                });*/

                reservation.Equipments[equipment.Name] = false;
            }

            List<int> hours = new List<int>();

            //itt attól függően legyenek benne a hours-ban az órák hogy melyik napra akar a felhasználó foglalni
            //some killing dynamic stuff.....
            for (int i = reservation.Room.Studio.OpeningHourMonday; i <= reservation.Room.Studio.ClosingHourMonday; ++i)
            {
                hours.Add(i);
            }

            ViewData["Hours"] = new SelectList(hours);
            return View(reservation);
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(int? roomId, [Bind("StartHour,EndHour,Day,BandName,UserOwnName,UserPhoneNumber,UserEmail,Equipments")] ReservationViewModel reservation)
        public async Task<IActionResult> Create(int? roomId, ReservationViewModel reservation)
        {
            if (roomId == null || reservation == null)
            {
                TempData["DangerAlert"] = "Hiba lépett fel a foglalás során, kérjük próbálja újra!";
                return RedirectToAction("Index", "Home");
            }

            reservation.Room = _reservationService.GetRoom(roomId);

            User user = await _userManager.FindByNameAsync(User.Identity.Name);

            reservation.UserEmail = user.Email;
            reservation.UserPhoneNumber = user.PhoneNumber;

            DateTime day = reservation.Day;
            DateTime startDate = new DateTime(day.Year, day.Month, day.Day, reservation.StartHour, 0, 0);
            DateTime endDate = new DateTime(day.Year, day.Month, day.Day, reservation.EndHour, 0, 0);

            //a reservationDateErrorban kéne azt is ellenőrizni hogy az adott időpontban foglalt-e a terem
            //valahol ellenőrizni kell azt is hogy az eszközofoglalás valid-e
            //itt mindegy mi a reservation id-ja, mert létrehozáskor minden foglalást megvizsgálja
            switch (_reservationService.ValidateReservation(startDate, endDate, "create", 1, roomId.Value))
            {
                case ReservationDateError.StartInvalid:
                    ModelState.AddModelError("StartHour", "A kezdés dátuma nem megfelelő!");
                    break;
                case ReservationDateError.EndInvalid:
                    ModelState.AddModelError("EndHour", "A megadott foglalási idő érvénytelen (a foglalás vége korábban van, mint a kezdete)!");
                    break;
                case ReservationDateError.Conflicting:
                    ModelState.AddModelError("Start", "A megadott időpontban a terembe már van foglalás!");
                    break;
                case ReservationDateError.LengthInvalid:
                    ModelState.AddModelError("End", "Üres az időintervallum!");
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

                //itt attól függően legyenek benne a hours-ban az órák hogy melyik napra akar a felhasználó foglalni
                //some killing dynamic stuff.....
                for (int i = reservation.Room.Studio.OpeningHourMonday; i <= reservation.Room.Studio.ClosingHourMonday; ++i)
                {
                    hours.Add(i);
                }

                ViewData["Hours"] = new SelectList(hours);
                return View("Create", reservation);
            }

            /*User user;

            if (User.Identity.IsAuthenticated)
                user = await _userManager.FindByNameAsync(User.Identity.Name);
            else
                return RedirectToAction("Login", "Account");
            */

            if (!await _reservationService.SaveReservationAsync(roomId, user.UserName, reservation))
            {
                ModelState.AddModelError("", "A foglalás sikertelen!");
                TempData["DangerAlert"] = "A foglalás sikertelen!";
                List<int> hours = new List<int>();

                //itt attól függően legyenek benne a hours-ban az órák hogy melyik napra akar a felhasználó foglalni
                //some killing dynamic stuff.....
                for (int i = reservation.Room.Studio.OpeningHourMonday; i <= reservation.Room.Studio.ClosingHourMonday; ++i)
                {
                    hours.Add(i);
                }

                ViewData["Hours"] = new SelectList(hours);
                return View("Create", reservation);
            }

            TempData["SuccessAlert"] = "A foglalást sikeresen rögzítettük!";
            return View("Result", reservation);
        }

        [HttpGet]
        public async Task<IActionResult> CreateFromTable(int dayIndex, int? roomId, int? hour)
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["DangerAlert"] = "A foglaláshoz bejelentkezés szükséges!";
                return RedirectToAction("Login", "Account");
            }

            if (roomId == null || hour == null)
                return NotFound();

            //dayIndex: shows the day of the wanted reservation. 0 means today, 1 means tomorrow, etc.
            DateTime day = DateTime.Now.AddDays(dayIndex);
            DateTime dateForReservation = new DateTime(day.Year, day.Month, day.Day, (int)hour, 0, 0);
            User user = await _userManager.FindByNameAsync(User.Identity.Name);

            Reservation reservation = new Reservation
            {
                RehearsalRoomId = (int)roomId,
                //RehearsalRoom = _reservationService.GetRoom(roomId),
                Start = dateForReservation,
                End = dateForReservation.AddHours(1),
                UserId = Int32.Parse(_userManager.GetUserId(User)),
                BandName = User.IsInRole("musician") ? user.DefaultBandName : ""
            };

            if (_reservationService.AddReservation(reservation))
            {
                ReservationViewModel viewModel = _reservationService.NewReservation(roomId);

                viewModel.BandName = User.IsInRole("musician") ? user.DefaultBandName : "";
                viewModel.UserOwnName = user.UserOwnName;
                viewModel.UserEmail = user.Email;
                viewModel.UserPhoneNumber = user.PhoneNumber;

                TempData["SuccessAlert"] = "Foglalását sikeresen rögzítettük!";
                return View("Result", viewModel);
            }
            else
            {
                TempData["DangerAlert"] = "Hiba lépett fel a foglalás során!";
                return RedirectToAction("Details", "RehearsalRooms", roomId);
            }
        }
        
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

            string userId = _userManager.GetUserId(User);
            User user = await _userManager.FindByIdAsync(userId);

            reservation.User = user;

            ViewData["RehearsalRoomId"] = new SelectList(_reservationService.Rooms, "Id", "Number", reservation.RehearsalRoomId);
            
            List<int> hours = new List<int>();

            //itt attól függően legyenek benne a hours-ban az órák hogy melyik napra akar a felhasználó foglalni
            //some killing dynamic stuff.....
            for (int i = reservation.RehearsalRoom.Studio.OpeningHourMonday; i <= reservation.RehearsalRoom.Studio.ClosingHourMonday; ++i)
            {
                hours.Add(i);
            }

            ViewData["Hours"] = new SelectList(hours);

            ReservationViewModel viewModel = new ReservationViewModel
            {
                UserOwnName = reservation.User.UserOwnName,
                UserPhoneNumber = reservation.User.PhoneNumber,
                UserEmail = reservation.User.Email,
                Room = reservation.RehearsalRoom,
                BandName = reservation.BandName,
                Day = reservation.Start,
                StartHour = reservation.Start.Hour,
                EndHour = reservation.End.Hour,
                Id = reservation.Id
            };

            Dictionary<string, int> equipments = new Dictionary<string, int>();
            
            foreach (var e in reservation.Equipments)
            {
                viewModel.Equipments[e.Name] = true;
            }

            RehearsalRoom room = _reservationService.GetRoom(reservation.RehearsalRoomId);
            RehearsalStudio studio = _reservationService.Studios.FirstOrDefault(l => l.Rooms.Contains(room));

            foreach (var e in studio.Equipments)
            {
                if (!viewModel.Equipments.ContainsKey(e.Name)) //ha foglalva van az aktuális foglaláshoz
                {
                    viewModel.Equipments[e.Name] = false;
                }
            }

            return View(viewModel);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int reservationId, ReservationViewModel reservationViewModel)
        {
            Reservation reservation = _reservationService.Reservations.FirstOrDefault(l => l.Id == reservationId);

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

            switch (_reservationService.ValidateReservation(startDate, endDate, "action", reservation.Id, reservation.RehearsalRoomId))
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
            }

            RehearsalRoom room = _reservationService.GetRoom(reservation.RehearsalRoomId);
            RehearsalStudio studio = _reservationService.Studios.FirstOrDefault(l => l.Rooms.Contains(room));

            foreach (var e in reservationViewModel.Equipments) //ebben az összes stúdióban bérelhető eszköz benne van
            {
                Equipment eq = studio.Equipments.FirstOrDefault(l => l.Name == e.Key);
                //ha már benne van a foglalásban de lemondtuk:
                if (!e.Value && reservation.Equipments.Contains(eq))
                {
                    reservation.Equipments.Remove(eq);
                }
                else if (e.Value && !reservation.Equipments.Contains(eq))
                {
                    reservation.Equipments.Add(eq);
                }
            }

            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var error in allErrors)
            {
                Debug.WriteLine(error);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _reservationService.UpdateReservation(reservation);
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

            reservationViewModel.Room = reservation.RehearsalRoom;
            reservationViewModel.Id = reservation.Id;
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

        // GET: Reservations/Delete/5
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

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int reservationId)
        {
            var reservation = _reservationService.GetReservation(reservationId);

            User user = await _userManager.FindByNameAsync(User.Identity.Name);

            ReservationViewModel model = new ReservationViewModel
            {
                Day = reservation.Start.Date,
                StartHour = reservation.Start.Hour,
                EndHour = reservation.End.Hour,
                Room = reservation.RehearsalRoom,
                BandName = reservation.BandName,
                UserOwnName = user.UserOwnName,
                UserPhoneNumber = user.PhoneNumber
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
