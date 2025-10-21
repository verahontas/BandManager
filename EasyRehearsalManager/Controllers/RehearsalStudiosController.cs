using EasyRehearsalManager.Model;
using EasyRehearsalManager.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EasyRehearsalManager.Web.Controllers
{
    public class RehearsalStudiosController : BaseController
    {
        private int[] OpeningHours = new int[7];
        private int[] ClosingHours = new int[7];

        private UserManager<User> _userManager;

        private int GetOpeningHour(RehearsalStudio studio, int index)
        {
            OpeningHours[0] = studio.OpeningHourMonday;
            OpeningHours[1] = studio.OpeningHourTuesday;
            OpeningHours[2] = studio.OpeningHourWednesday;
            OpeningHours[3] = studio.OpeningHourThursday;
            OpeningHours[4] = studio.OpeningHourFriday;
            OpeningHours[5] = studio.OpeningHourSaturday;
            OpeningHours[6] = studio.OpeningHourSunday;

            return DateTime.Today.DayOfWeek switch
            {
                DayOfWeek.Monday => OpeningHours[(index + 0) % 7],
                DayOfWeek.Tuesday => OpeningHours[(index + 1) % 7],
                DayOfWeek.Wednesday => OpeningHours[(index + 2) % 7],
                DayOfWeek.Thursday => OpeningHours[(index + 3) % 7],
                DayOfWeek.Friday => OpeningHours[(index + 4) % 7],
                DayOfWeek.Saturday => OpeningHours[(index + 5) % 7],
                DayOfWeek.Sunday => OpeningHours[(index + 6) % 7],
                _ => -1,
            };
        }

        private int GetClosingHour(RehearsalStudio studio, int index)
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
                    return ClosingHours[(index + 0) % 7];
                case DayOfWeek.Tuesday:
                    return ClosingHours[(index + 1) % 7];
                case DayOfWeek.Wednesday:
                    return ClosingHours[(index + 2) % 7];
                case DayOfWeek.Thursday:
                    return ClosingHours[(index + 3) % 7];
                case DayOfWeek.Friday:
                    return ClosingHours[(index + 4) % 7];
                case DayOfWeek.Saturday:
                    return ClosingHours[(index + 5) % 7];
                case DayOfWeek.Sunday:
                    return ClosingHours[(index + 6) % 7];
                default:
                    return -1;
            }
        }

        public RehearsalStudiosController(IReservationService reservationService, ApplicationState applicationState,
            UserManager<User> userManager)
            : base(reservationService, applicationState)
        {
            _userManager = userManager;
        }

        [AllowAnonymous]
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

            //if searchString isn't empty then we select first the matching studios
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
        }

        public FileResult GetLogo(int? studioId)
        {
            if (studioId == null)
                return File("~/images/nologo.png", "image/jpeg");

            Byte[] imageContent = _reservationService.GetLogo((int)studioId);

            if (imageContent == null)
                return File("~/images/nologo.png", "image/jpeg");

            return File(imageContent, "image/jpeg");
        }

        [AllowAnonymous]
        // GET: RehearsalStudios/Details/5
        public IActionResult Details(int? studioId)
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

            ViewBag.Images = _reservationService.GetImagesForStudio((int)studioId);

            ViewBag.Reservations = list;
            
            return View(rehearsalStudio);
        }

        [Authorize(Roles = "administrator, owner")]
        // GET: RehearsalStudios/Create
        public async Task<IActionResult> Create()
        {
            if (User.IsInRole("administrator"))
            {
                List<User> owners = new List<User>();
                var users = _reservationService.Users;
                foreach (var user in users)
                {
                    if (await _userManager.IsInRoleAsync(user, "owner"))
                    {
                        owners.Add(user);
                    }
                }

                ViewData["Owners"] = new SelectList(owners, "Id", "UserOwnName");
                return View();
            }

            //if current user is an owner
            RehearsalStudioViewModel viewModel = new RehearsalStudioViewModel
            {
                UserId = Int32.Parse(_userManager.GetUserId(User))
            };
            return View(viewModel);
        }

        // POST: RehearsalStudios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(RehearsalStudioViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                RehearsalStudio studio = viewModel;

                if (viewModel.LogoImage != null)
                {
                    byte[] fileBytes = null;

                    if (viewModel.LogoImage.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            viewModel.LogoImage.CopyTo(ms);
                            fileBytes = ms.ToArray();
                        }
                    }

                    studio.Logo = fileBytes;
                }

                if (!_reservationService.AddStudio(studio))
                {
                    TempData["DangerAlert"] = "Próbahely létrehozása sikertelen, próbálja újra!";
                    return View(viewModel);
                }
                else
                {
                    TempData["SuccessAlert"] = "Próbahely létrehozása sikeres!";
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(viewModel);
        }

        [Authorize(Roles = "owner, administrator")]
        // GET: RehearsalStudios/Edit/5
        public IActionResult Edit(int? studioId)
        {
            if (studioId == null)
            {
                return NotFound();
            }

            RehearsalStudio rehearsalStudio = _reservationService.GetStudio(studioId);
            if (rehearsalStudio == null)
            {
                return NotFound();
            }

            //if the current user is an owner
            rehearsalStudio.UserId = Int32.Parse(_userManager.GetUserId(User));

            return View(rehearsalStudio);
        }

        // POST: RehearsalStudios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(RehearsalStudio rehearsalStudio)
        { 
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
                TempData["SuccessAlert"] = "A próbahely módosítása sikeresen megtörtént!";
                return RedirectToAction("Details", new { studioId = rehearsalStudio.Id });
            }
            TempData["SuccessAlert"] = "Hiba történt, próbálja újra!";
            return View(rehearsalStudio);
        }

        [Authorize(Roles = "owner, administrator")]
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
            if (_reservationService.RemoveStudio(studioId))
            {
                TempData["SuccessAlert"] = "Próbahely törlése sikeresen megtörtént! A hozzá tartozó termek, és a termek foglalásai is törlődtek.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["DangerAlert"] = "Próbahely törlése sikertelen!";
                return RedirectToAction("Details", studioId);
            }
        }

        private bool RehearsalStudioExists(int studioId)
        {
            return _reservationService.RehearsalStudioExist(studioId);
        }

        [Authorize(Roles = "administrator")]
        //Generating a random studio.
        public IActionResult AddStudio()
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
                Description = "No description"
            };

            _reservationService.AddStudio(rehearsalStudio);

            return RedirectToAction("Index");
        }

        public PartialViewResult GetTableOfReservations(int studioId, int dayIndex)
        {
            if (studioId == 0)
                return null;

            if (dayIndex < 0)
                dayIndex = 0;

            if (dayIndex > 9)
                dayIndex = 9;

            RehearsalStudio studio = _reservationService.Studios.FirstOrDefault(l => l.Id == studioId);

            if (studio == null)
                return null;

            DateTime currentStartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            currentStartDate = currentStartDate.AddDays(dayIndex);

            DateTime currentEndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
            currentEndDate = currentEndDate.AddDays(dayIndex);


            List<Reservation> reservations = _reservationService.Reservations
                                                .Where(l => l.RehearsalRoom.StudioId == studioId && 
                                                       l.IsConflicting(currentStartDate, currentEndDate))
                                                .ToList();

            ReservationTableViewModel reservationTableViewModel = new ReservationTableViewModel {
                OpeningHour = GetOpeningHour(studio, dayIndex),
                ClosingHour = GetClosingHour(studio, dayIndex),
                NumberOfAvailableRooms = studio.Rooms.Where(l => l.Available).Count(),
                Rooms = studio.Rooms.ToList(),
                Reservations = reservations,
                Studio = studio,
                Index = dayIndex
            };
            return PartialView("_ReservationsTable", reservationTableViewModel);
        }

        public PartialViewResult GetStudioReservationsTablePartial(int? studioId)
        {
            if (studioId == null)
                return null;

            return PartialView("_StudioReservationsTablePartial", studioId);
        }

        public PartialViewResult AddPicturesToStudioPartial(int? studioId)
        {
            if (studioId == null)
                return null;

            ImageUploadViewModel viewModel = new ImageUploadViewModel
            {
                EntityId = (int)studioId
            };


            return PartialView("_AddPicturesToStudioPartial", viewModel);
        }

        public PartialViewResult ChangeLogoPartial(int? studioId)
        {
            if (studioId == null)
                return null;

            ImageUploadViewModel viewModel = new ImageUploadViewModel
            {
                EntityId = (int)studioId
            };


            return PartialView("_UploadLogoPartial", viewModel);
        }

        [HttpPost]
        public IActionResult AddImages(ImageUploadViewModel viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                    return RedirectToAction("Details", "RehearsalStudios", new { studioId = viewModel.EntityId });

                if (viewModel.Images == null || viewModel.Images.Count == 0)
                {
                    TempData["DangerAlert"] = "Válasszon ki képeket!";
                    return RedirectToAction("Details", "RehearsalStudios", new { studioId = viewModel.EntityId });
                }

                //the input field allows us to upload multiple files, so we have to handle all of them

                foreach (var image in viewModel.Images)
                {
                    string ext = Path.GetExtension(image.FileName);
                    if (ext != ".jpg" && ext != ".JPG" && ext != ".png" && ext != ".PNG")
                    {
                        TempData["DangerAlert"] = "A feltölteni kívánt fájlok formátuma csak PNG vagy JPG lehet.";
                        ModelState.AddModelError("", "A feltölteni kívánt fájlok formátuma csak PNG vagy JPG lehet.");
                        return RedirectToAction("Details", "RehearsalStudios", new { studioId = viewModel.EntityId });
                    }
                }

                if (!_reservationService.UploadImagesForStudio(viewModel.EntityId, viewModel.Images))
                {
                    TempData["DangerAlert"] = "Valami hiba történt, próbálja újra!";
                    ModelState.AddModelError("", "Valami hiba történt, próbálja újra!");
                    return RedirectToAction("Details", "RehearsalStudios", new { studioId = viewModel.EntityId });
                }
            }
            catch
            {
                TempData["DangerAlert"] = "Valami hiba történt, próbálja újra!";
                ModelState.AddModelError("", "Valami hiba történt, próbálja újra!");
                return RedirectToAction("Details", "RehearsalStudios", new { studioId = viewModel.EntityId });
            }

            TempData["SucessAlert"] = "Képek feltöltése sikeres!";
            return RedirectToAction("Details", "RehearsalStudios", new { studioId = viewModel.EntityId });
        }

        [HttpPost]
        public IActionResult ChangeLogo(ImageUploadViewModel viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                    return RedirectToAction("Details", "RehearsalStudios", new { studioId = viewModel.EntityId });

                if (viewModel.Images == null || viewModel.Images.Count == 0)
                {
                    TempData["DangerAlert"] = "Válasszon ki egy képet!";
                    return RedirectToAction("Details", "RehearsalStudios", new { studioId = viewModel.EntityId });
                }

                string ext = Path.GetExtension(viewModel.Images.First().FileName);
                if (ext != ".jpg" && ext != ".JPG" && ext != ".png" && ext != ".PNG")
                {
                    TempData["DangerAlert"] = "A logo kép formátuma csak PNG vagy JPG lehet.";
                    ModelState.AddModelError("", "A logo kép formátuma csak PNG vagy JPG lehet.");
                    return RedirectToAction("Details", "RehearsalStudios", new { studioId = viewModel.EntityId });
                }

                if (!_reservationService.ChangeLogoForStudio(viewModel.EntityId, viewModel.Images.First()))
                {
                    TempData["DangerAlert"] = "Valami hiba történt, próbálja újra!";
                    ModelState.AddModelError("", "Valami hiba történt, próbálja újra!");
                    return RedirectToAction("Details", "RehearsalStudios", new { studioId = viewModel.EntityId });
                }
            }
            catch
            {
                TempData["DangerAlert"] = "Valami hiba történt, próbálja újra!";
                ModelState.AddModelError("", "Valami hiba történt, próbálja újra!");
                return RedirectToAction("Details", "RehearsalStudios", new { studioId = viewModel.EntityId });
            }

            TempData["SucessAlert"] = "Logo feltöltése sikeres!";
            return RedirectToAction("Details", "RehearsalStudios", new { studioId = viewModel.EntityId });
        }

        public FileResult GetImage(int? imageId)
        {
            if (imageId == null)
                return null;

            byte[] image = _reservationService.GetStudioImage(imageId);

            if (image == null)
                return null;

            return File(image, "image/png");
        }

        [HttpGet]
        public IActionResult DeleteStudioImages(int? studioId)
        {
            if (studioId == null)
                return NotFound();

            DeleteStudioImagesViewModel viewModel = new DeleteStudioImagesViewModel
            {
                StudioId = (int)studioId
            };

            List<int> imageIds = _reservationService.GetImagesForStudio((int)studioId);
            viewModel.Images = new Dictionary<int, bool>();

            foreach (var id in imageIds)
            {
                viewModel.Images.Add(id, true);
            }

            return View("DeleteStudioImages", viewModel);
        }

        [HttpPost]
        public IActionResult DeleteStudioImages(DeleteStudioImagesViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData["DangerAlert"] = "Hiba történt, kérem próbálja újra!";
                return View(viewModel);
            }

            List<int> imagesToDelete = new List<int>();
            foreach (var image in viewModel.Images)
            {
                if (image.Value == false) //then we have to delete it
                {
                    imagesToDelete.Add(image.Key);
                }
            }

            if (!_reservationService.DeleteImagesForStudio(viewModel.StudioId, imagesToDelete))
            {
                TempData["DangerAlert"] = "Képek törlése sikertelen, kérem próbálja újra!";
                return View(viewModel);
            }

            TempData["SuccessAlert"] = "Képek törlése sikeres!";
            return RedirectToAction("Details", new { studioId = viewModel.StudioId });
        }

        public PartialViewResult GetAboutStudioPartial(int? studioId)
        {
            if (studioId == null)
                return null;

            var studio = _reservationService.GetStudio(studioId);

            if (studio == null)
                return null;
            
            return PartialView("_AboutStudioPartial", studio);
        }

        public PartialViewResult GetImagesForStudioPartial(int? studioId)
        {
            if (studioId == null)
                return null;

            ViewBag.Images = _reservationService.GetImagesForStudio((int)studioId);
            return PartialView("_StudioImagesPartial");
        }

        public PartialViewResult GetStudioContactsPartial(int? studioId)
        {
            if (studioId == null)
                return null;

            var studio = _reservationService.GetStudio(studioId);

            if (studio == null)
                return null;

            return PartialView("_StudioContactsPartial", studio);
        }

        public PartialViewResult GetRoomsOfStudioPartial(int? studioId)
        {
            if (studioId == null)
                return null;

            List<RehearsalRoom> rooms = new List<RehearsalRoom>();
            if (User.IsInRole("administrator"))
            {
                rooms = _reservationService.Rooms.Where(l => l.StudioId == studioId).ToList();
            }
            else if (User.IsInRole("owner"))
            {
                rooms = _reservationService.Rooms
                    .Where(l => l.StudioId == studioId)
                    .Where(l => l.Studio.UserId == Int32.Parse(_userManager.GetUserId(User)))
                    .ToList();
            }
            else
            {
                rooms = _reservationService.Rooms
                    .Where(l => l.StudioId == studioId && l.Available).ToList();
            }

            return PartialView("_RoomsOfStudioPartial", rooms);
        }
    }
}
