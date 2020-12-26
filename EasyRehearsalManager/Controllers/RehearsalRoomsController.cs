using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyRehearsalManager.Model;
using EasyRehearsalManager.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Syncfusion.Linq;

namespace EasyRehearsalManager.Web.Controllers
{
    public class RehearsalRoomsController : BaseController
    {
        UserManager<User> _userManager;

        public RehearsalRoomsController(IReservationService reservationService, ApplicationState applicationState,
            UserManager<User> userManager)
            : base(reservationService, applicationState)
        {
            _userManager = userManager;
        }

        //currentFilter for search
        //note: could be sortable by properties
        // GET: RehearsalRooms
        public IActionResult Index(DateTime date, int startTime, int endTime, string sortOrder = "")
        {
            var rooms = _reservationService.Rooms.Where(l => l.Available);

            DateTime init = new DateTime();

            if (date != init && startTime != 0 && endTime != 0) //ha dátumra és időpontra keresnek
            {
                DateTime dateSearchStart = new DateTime(date.Year, date.Month, date.Day, startTime, 0, 0);
                DateTime dateSearchEnd = new DateTime(date.Year, date.Month, date.Day, endTime, 0, 0);
                rooms = rooms.Where(l => l.GetReservationForDate(dateSearchStart, dateSearchEnd) == "Foglalás");
            }
            else if (date != init && startTime == 0 && endTime  == 0) //ha dátumra keresnek, de időpontot nem adnak meg, akkor az egész napot nézzük
            {
                DateTime dateSearchStart = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                DateTime dateSearchEnd = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
                rooms = rooms.Where(l => l.GetReservationForDate(dateSearchStart, dateSearchEnd) == "Foglalás");
            }

            if (User.Identity.IsAuthenticated && User.IsInRole("owner"))
            {
                int ownerId = Int32.Parse(_userManager.GetUserId(User));
                return View(_reservationService.GetRoomsByOwnerId(ownerId));
            }

            List<int> hours = new List<int>();
            for (int i = 0; i < 24; ++i)
            {
                hours.Add(i);
            }

            ViewData["Hours"] = new SelectList(hours);

            ViewData["CurrentSort"] = sortOrder;
            ViewData["NumberSortParam"] = sortOrder == "number_asc" ? "number_desc" : "number_asc";
            ViewData["PriceSortParam"] = sortOrder == "price_asc" ? "price_desc" : "price_asc";
            ViewData["SizeSortParam"] = sortOrder == "size_asc" ? "size_desc" : "size_asc";
            ViewData["StudioNameSortParam"] = sortOrder == "studioName_asc" ? "studioName_desc" : "studioName_asc";
            //ViewData["CurrentFilter"] = searchString;

            

            switch (sortOrder)
            {
                case "number_desc":
                    return View(rooms.OrderByDescending(l => l.Number).ToList());
                case "number_asc":
                    return View(rooms.OrderBy(l => l.Number).ToList());
                case "price_desc":
                    return View(rooms.OrderByDescending(l => l.Price).ToList());
                case "price_asc":
                    return View(rooms.OrderBy(l => l.Price).ToList());
                case "size_desc":
                    return View(rooms.OrderByDescending(l => l.Size).ToList());
                case "size_asc":
                    return View(rooms.OrderBy(l => l.Size).ToList());
                case "studioName_desc":
                    return View(rooms.OrderByDescending(l => l.Studio.Name).ToList());
                case "studioName_asc":
                    return View(rooms.OrderBy(l => l.Studio.Name).ToList());
                default:
                    return View(rooms.OrderBy(l => l.Price).ToList());
            }
        }

        // GET: RehearsalRooms/Details/5
        public IActionResult Details(int? roomId)
        {
            if (roomId == null)
            {
                return NotFound();
            }

            var rehearsalRoom = _reservationService.GetRoom(roomId);

            if (rehearsalRoom == null)
            {
                return NotFound();
            }

            return View(rehearsalRoom);
        }

        [Authorize(Roles = "owner, administrator")]
        // GET: RehearsalRooms/Create
        public async Task<IActionResult> Create()
        {
            if (User.IsInRole("owner")) //owner csak a saját stúdiójába hozhat létre termet
            {
                string userId = _userManager.GetUserId(User);
                User user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    TempData["DangerAlert"] = "Hiba!";
                    return View();
                }
                else
                {
                    int userIdInt = Int32.Parse(userId);
                    var studios = _reservationService.GetStudiosByOwner(userIdInt).ToList();
                    ViewData["Studios"] = new SelectList(studios, "Id", "Name");
                    return View();
                }
            }
            else if (User.IsInRole("administrator")) //admin bármelyik stúdióba hozhat létre termet
            {
                var studios = _reservationService.Studios;
                ViewData["Studios"] = new SelectList(studios, "Id", "Name");
                return View();
            }
            else //ha zenész vagy nem authentikált, akkor nem hozhat létre
            {
                TempData["DangerAlert"] = "Hibás hitelesítés!";
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: RehearsalRooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public IActionResult Create([Bind("Id,Number,Description,Price,Size,Available,StudioId")] RehearsalRoom rehearsalRoom)
        public IActionResult Create(RehearsalRoom @rehearsalRoom)
        {
            rehearsalRoom.Studio = _reservationService.GetStudio(rehearsalRoom.StudioId);

            if (ModelState.IsValid)
            {
                if (_reservationService.AddRoom(@rehearsalRoom) 
                    && _reservationService.NewRoomAdded(rehearsalRoom.StudioId))
                {
                    TempData["SuccessAlert"] = "Terem sikeresen létrehozva!";
                    return RedirectToAction(nameof(Index));
                }
            }

            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);

            ViewData["StudioId"] = new SelectList(_reservationService.Studios, "Id", "Address", rehearsalRoom.StudioId);
            TempData["DangerAlert"] = "Terem létrehozása sikertelen!";
            return View(rehearsalRoom);
        }

        [Authorize(Roles = "owner, administrator")]
        [HttpGet]
        // GET: RehearsalRooms/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rehearsalRoom =  _reservationService.GetRoom(id);
            if (rehearsalRoom == null)
            {
                return NotFound();
            }
            ViewData["StudioId"] = new SelectList(_reservationService.Studios, "Id", "Address", rehearsalRoom.StudioId);
            return View(rehearsalRoom);
        }

        // POST: RehearsalRooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Number,Description,Price,Size,Available,StudioId")] RehearsalRoom rehearsalRoom)
        {
            if (id != rehearsalRoom.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _reservationService.UpdateRoom(rehearsalRoom);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RehearsalRoomExists(rehearsalRoom.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["StudioId"] = new SelectList(_reservationService.Studios, "Id", "Address", rehearsalRoom.StudioId);
            return View(rehearsalRoom);
        }

        [Authorize(Roles = "owner, administrator")]
        // GET: RehearsalRooms/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rehearsalRoom = _reservationService.GetRoom(id);

            if (rehearsalRoom != null)
                return View(rehearsalRoom);

            return NotFound();
        }

        // POST: RehearsalRooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (_reservationService.RemoveRoom(id))
            {
                TempData["SuccessAlert"] = "A termet sikeresen töröltük!";
                return RedirectToAction(nameof(Index));
            }

            TempData["DangerAlert"] = "A terem törlése sikertelen!";
            var rehearsalRoom = _reservationService.GetRoom(id);
            return View(rehearsalRoom);
        }

        private bool RehearsalRoomExists(int id)
        {
            return _reservationService.Rooms.Any(e => e.Id == id);
        }
    }
}
