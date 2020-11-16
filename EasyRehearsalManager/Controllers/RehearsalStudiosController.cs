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

namespace EasyRehearsalManager.Web.Controllers
{
    public class RehearsalStudiosController : BaseController
    {
        public int[] OpeningHours = new int[7];
        public int[] ClosingHours = new int[7];

        public int DayIndex;

        private UserManager<User> _userManager;

        public int GetOpeningHour(RehearsalStudio studio, int index)
        {
            OpeningHours[0] = studio.OpeningHourMonday;
            OpeningHours[1] = studio.OpeningHourTuesday;
            OpeningHours[2] = studio.OpeningHourWednesday;
            OpeningHours[3] = studio.OpeningHourThursday;
            OpeningHours[4] = studio.OpeningHourFriday;
            OpeningHours[5] = studio.OpeningHourSaturday;
            OpeningHours[6] = studio.OpeningHourSunday;

            switch (DateTime.Today.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    return OpeningHours[index + 0];
                case DayOfWeek.Tuesday:
                    return OpeningHours[index + 1];
                case DayOfWeek.Wednesday:
                    return OpeningHours[index + 2];
                case DayOfWeek.Thursday:
                    return OpeningHours[index + 3];
                case DayOfWeek.Friday:
                    return OpeningHours[index + 4];
                case DayOfWeek.Saturday:
                    return OpeningHours[index + 5];
                case DayOfWeek.Sunday:
                    return OpeningHours[index + 6];
                default:
                    return -1;
            }
        }

        public int GetClosingHour(RehearsalStudio studio, int index)
        {
            ClosingHours[0] = studio.ClosingHourMonday;
            ClosingHours[1] = studio.ClosingHourTuesday;
            ClosingHours[2] = studio.ClosingHourWednesday;
            ClosingHours[3] = studio.ClosingHourThursday;
            ClosingHours[4] = studio.ClosingHourFriday;
            ClosingHours[5] = studio.ClosingHourSaturday;
            ClosingHours[6] = studio.ClosingHourSunday;

            switch (DateTime.Today.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    return ClosingHours[index + 0];
                case DayOfWeek.Tuesday:
                    return ClosingHours[index + 1];
                case DayOfWeek.Wednesday:
                    return ClosingHours[index + 2];
                case DayOfWeek.Thursday:
                    return ClosingHours[index + 3];
                case DayOfWeek.Friday:
                    return ClosingHours[index + 4];
                case DayOfWeek.Saturday:
                    return ClosingHours[index + 5];
                case DayOfWeek.Sunday:
                    return ClosingHours[index + 6];
                default:
                    return -1;
            }
        }

        public RehearsalStudiosController(IReservationService reservationService, ApplicationState applicationState,
            UserManager<User> userManager)
            : base(reservationService, applicationState)
        {
            DayIndex = 0;
            _userManager = userManager;
        }

        // GET: RehearsalStudios
        public IActionResult Index(string currentFilter, string searchString, int pageNumber = 1, string sortOrder = "")
        {
            IEnumerable<RehearsalStudio> studios;
            if (User.Identity.IsAuthenticated && User.IsInRole("owner"))
            {
                int ownerId = Int32.Parse(_userManager.GetUserId(User));
                return View(_reservationService.GetStudiosByOwner(ownerId));
            }
            else
            {
                studios = _reservationService.Studios;
            }

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            //ha a keresőstring nem üres, először szűrjük az összes könyvet
            if (!String.IsNullOrEmpty(searchString))
            {
                studios = studios.Where(l => l.Name.ToUpper().Contains(searchString.ToUpper()));
            }

            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParam"] = sortOrder == "name_asc" ? "name_desc" : "name_asc";
            ViewData["CurrentFilter"] = searchString;

            List<RehearsalStudio> result = new List<RehearsalStudio>();

            switch (sortOrder)
            {
                case "name_desc":
                    return View(studios.OrderByDescending(l => l.Name).ToList());
                case "name_asc":
                    return View(studios.OrderBy(l => l.Name).ToList());
                default:
                    return View(studios.OrderByDescending(l => l.Name).ToList());
            }

            /*
             int pageSize = 5;
                IQueryable<Book> res = result.AsQueryable();
                //return View(await PaginatedList<Book>.CreateAsync(res.AsNoTracking(), pageNumber ?? 1, pageSize));
                return View(PaginatedList<Book>.CreateAsync(res.AsNoTracking(), pageNumber, pageSize));
             */
        }

        // GET: RehearsalStudios/Details/5
        public IActionResult Details(int? studioId, int? index)
        {
            if (studioId == null)
            {
                return NotFound();
            }

            var rehearsalStudio = _reservationService.GetStudio(studioId);
            if (rehearsalStudio == null)
            {
                return NotFound();
            }

            List<Reservation> list = new List<Reservation>();

            foreach (var r in _reservationService
                .GetReservationsByStudioId(studioId)
                .Where(l => l.Start.CompareTo(DateTime.Now.AddDays(-1)) >= 0))
            {
                list.Add(r);
            }

            ViewBag.Reservations = list;
                
                //csak azokat adom vissza amik legkésőbb egy nappal ezelőtt végetértek
            return View(rehearsalStudio);
        }

        public ActionResult ReservationsTable(int studioId, bool isNext)
        {
            if (DayIndex < 0)
                return NotFound();

            DayIndex = isNext ? DayIndex + 1 : DayIndex - 1;

            ReservationTableViewModel viewModel = new ReservationTableViewModel();

            RehearsalStudio studio = _reservationService.GetStudio(studioId);

            viewModel.Studio = studio;
            viewModel.NumberOfAvailableRooms = studio.Rooms.Count;
            viewModel.Index = DayIndex;
            viewModel.OpeningHour = GetOpeningHour(studio, DayIndex);
            viewModel.ClosingHour = GetClosingHour(studio, DayIndex);

            viewModel.Reservations = _reservationService.GetReservationsByStudioId(studioId).ToList();
            viewModel.Rooms = _reservationService.Rooms.Where(l => l.StudioId == studioId).ToList();


            return PartialView("ReservationsTable", viewModel);
        }

        // GET: RehearsalStudios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RehearsalStudios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Name,Address,LocationX,LocationY,District,Phone,Email,Web,NumberOfRooms,Description,OpeningHourMonday,ClosingHourMonday,OpeningHourTuesday,ClosingHourTuesday,OpeningHourWednesday,ClosingHourWednesday,OpeningHourThursday,ClosingHourThursday,OpeningHourFriday,ClosingHourFriday,OpeningHourSaturday,ClosingHourSaturday,OpeningHourSunday,ClosingHourSunday")] RehearsalStudio rehearsalStudio)
        {
            if (ModelState.IsValid)
            {
                string userId = _userManager.GetUserId(User);
                rehearsalStudio.UserId = Int32.Parse(userId);
                _reservationService.AddStudio(rehearsalStudio);
                return RedirectToAction(nameof(Index));
            }
            return View(rehearsalStudio);
        }

        // GET: RehearsalStudios/Edit/5
        public IActionResult Edit(int? studioId)
        {
            if (studioId == null)
            {
                return NotFound();
            }

            var rehearsalStudio = _reservationService.GetStudio(studioId);
            if (rehearsalStudio == null)
            {
                return NotFound();
            }
            return View(rehearsalStudio);
        }

        // POST: RehearsalStudios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Name,Address,LocationX,LocationY,District,Phone,Email,Web,NumberOfRooms,Description,OpeningHourMonday,ClosingHourMonday,OpeningHourTuesday,ClosingHourTuesday,OpeningHourWednesday,ClosingHourWednesday,OpeningHourThursday,ClosingHourThursday,OpeningHourFriday,ClosingHourFriday,OpeningHourSaturday,ClosingHourSaturday,OpeningHourSunday,ClosingHourSunday")] RehearsalStudio rehearsalStudio)
        {
            if (id != rehearsalStudio.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _reservationService.UpdateStudio(rehearsalStudio);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RehearsalStudioExists(rehearsalStudio.Id))
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
            return View(rehearsalStudio);
        }

        // GET: RehearsalStudios/Delete/5
        public IActionResult Delete(int? studioId)
        {
            if (studioId == null)
            {
                return NotFound();
            }

            var rehearsalStudio = _reservationService.GetStudio(studioId);
            if (rehearsalStudio == null)
            {
                return NotFound();
            }

            return View(rehearsalStudio);
        }

        // POST: RehearsalStudios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int studioId)
        {
            var rehearsalStudio = _reservationService.GetStudio(studioId);
            _reservationService.RemoveStudio(studioId);
            return RedirectToAction(nameof(Index));
        }

        private bool RehearsalStudioExists(int studioId)
        {
            return _reservationService.RehearsalStudioExist(studioId);
        }

        public void AddStudio()
        {
            string userId = _userManager.GetUserId(User);
            int count = _reservationService.Studios.Count();
            string nr = (count + 1).ToString();
            RehearsalStudio rehearsalStudio = new RehearsalStudio
            {
                UserId = Int32.Parse(userId),
                Name = "Studio Nr. " + nr,
                Address = "default address of studio nr " + nr,
                Phone = "06301234567",
                Email = "email@email.hu",
                Web = "https://www.probazona.hu/",
                LocationX = 0,
                LocationY = 0,
                District = 1,
                OpeningHourMonday = 10,
                ClosingHourMonday = 22,
                OpeningHourTuesday = 10,
                ClosingHourTuesday = 22,
                OpeningHourWednesday = 10,
                ClosingHourWednesday = 22,
                OpeningHourThursday = 10,
                ClosingHourThursday = 22,
                OpeningHourFriday = 10,
                ClosingHourFriday = 22,
                OpeningHourSaturday = 10,
                ClosingHourSaturday = 22,
                OpeningHourSunday = 10,
                ClosingHourSunday = 22,
                Description = "No description",
                NumberOfRooms = 1
            };

            _reservationService.AddStudio(rehearsalStudio);
        }
    }
}
