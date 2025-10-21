using System;
using System.Collections.Generic;
using System.IO;
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

        /// <summary>
        /// Returns the list of rehearsal rooms.
        /// </summary>
        /// <param name="date">The given day for search</param>
        /// <param name="startTime">The given start time (hour) for search</param>
        /// <param name="endTime">The given end time (hour) for search</param>
        /// <param name="sortOrder">Type of the given sort</param>
        /// <returns></returns>
        public IActionResult Index(DateTime date, DateTime startTime, DateTime endTime, string sortOrder = "")
        {
            var rooms = _reservationService.Rooms.Where(l => l.Available);

            DateTime init = new DateTime();

            if (date != init && startTime != init && endTime != init) //if there was date and time set for search
            {
                DateTime dateSearchStart = new DateTime(date.Year, date.Month, date.Day, startTime.Hour, startTime.Minute, 0);
                DateTime dateSearchEnd = new DateTime(date.Year, date.Month, date.Day, endTime.Hour, endTime.Minute, 0);
                rooms = rooms.Where(l => l.GetReservationForDate(dateSearchStart, dateSearchEnd) == "Foglalás");
            }
            else if (date != init && startTime == init && endTime  == init) //if date is set for search but time isn't, then we search the whole day
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

            ViewData["CurrentSort"] = sortOrder;
            ViewData["NumberSortParam"] = sortOrder == "number_asc" ? "number_desc" : "number_asc";
            ViewData["PriceSortParam"] = sortOrder == "price_asc" ? "price_desc" : "price_asc";
            ViewData["SizeSortParam"] = sortOrder == "size_asc" ? "size_desc" : "size_asc";
            ViewData["StudioNameSortParam"] = sortOrder == "studioName_asc" ? "studioName_desc" : "studioName_asc";
            ViewData["DateFilter"] = date;
            ViewData["StartTimeFilter"] = startTime;
            ViewData["EndTimeFilter"] = endTime;

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

        [Authorize(Roles = "administrator, owner")]
        public PartialViewResult GetUnavailableRooms()
        {
            if (User.IsInRole("owner"))
            {
                int userId = Int32.Parse(_userManager.GetUserId(User));
                var roomsOfOwner = _reservationService.Rooms.Where(l => l.Studio.UserId == userId && !l.Available).ToList();

                return PartialView("_UnavailableRooms", roomsOfOwner);
            }

            var rooms = _reservationService.Rooms.Where(l => !l.Available).ToList();
            return PartialView("_UnavailableRooms", rooms);
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

            ViewBag.Images = _reservationService.GetImagesForRoom((int)roomId);

            return View(rehearsalRoom);
        }

        /// <summary>
        /// Returns the create view for a room creation.
        /// </summary>
        /// <param name="studioId">If not null: we clicked the "Terem hozzáadása" button on the 'Details' page of a studio.</param>
        /// <returns></returns>
        [Authorize(Roles = "owner, administrator")]
        // GET: RehearsalRooms/Create
        public async Task<IActionResult> Create(int? studioId)
        {
            if (User.IsInRole("owner")) //owner can add a room only to a studio that belongs to him
            {
                string userId = _userManager.GetUserId(User);
                User user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    TempData["DangerAlert"] = "Hiba!";
                    return View();
                }

                if (studioId == null)
                {
                    var studios = _reservationService.GetStudiosByOwner(Int32.Parse(userId)).ToList();
                    ViewData["Studios"] = new SelectList(studios, "Id", "Name");
                    return View();
                }
                else
                {
                    RehearsalRoom room = new RehearsalRoom
                    {
                        StudioId = (int)studioId
                    };
                    return View(room);
                }
                
            }

            //if the current user is the admin
            if (studioId == null)
            {
                var studios = _reservationService.Studios;
                ViewData["Studios"] = new SelectList(studios, "Id", "Name");
                return View();
            }
            else
            {
                RehearsalRoom room = new RehearsalRoom
                {
                    StudioId = (int)studioId
                };
                return View(room);
            }
        }

        // POST: RehearsalRooms/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(RehearsalRoom @rehearsalRoom)
        {
            rehearsalRoom.Studio = _reservationService.GetStudio(rehearsalRoom.StudioId);

            if (ModelState.IsValid)
            {
                if (_reservationService.AddRoom(@rehearsalRoom) )
                    //&& _reservationService.NewRoomAdded(rehearsalRoom.StudioId))
                {
                    TempData["SuccessAlert"] = "Terem sikeresen létrehozva!";
                    return RedirectToAction(nameof(Index));
                }
            }

            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            
            TempData["DangerAlert"] = "Terem létrehozása sikertelen!";
            return View(rehearsalRoom);
        }

        [Authorize(Roles = "owner, administrator")]
        [HttpGet]
        // GET: RehearsalRooms/Edit/5
        public IActionResult Edit(int? roomId)
        {
            if (roomId == null)
            {
                return NotFound();
            }

            var rehearsalRoom =  _reservationService.GetRoom(roomId);
            if (rehearsalRoom == null)
            {
                return NotFound();
            }

            return View(rehearsalRoom);
        }

        [Authorize(Roles = "administrator, owner")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int roomId, RehearsalRoom rehearsalRoom)
        {
            if (roomId != rehearsalRoom.Id)
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
                TempData["SuccessAlert"] = "A próbaterem módosítása sikeresen megtörtént!";
                return RedirectToAction("Details", new { roomId = rehearsalRoom.Id });
            }

            TempData["DangerAlert"] = "A próbaterem módosítása sikertelen!";
            return View(rehearsalRoom);
        }

        [Authorize(Roles = "owner, administrator")]
        // GET: RehearsalRooms/Delete/5
        public IActionResult Delete(int? roomId)
        {
            if (roomId == null)
            {
                return NotFound();
            }

            var rehearsalRoom = _reservationService.GetRoom(roomId);

            if (rehearsalRoom != null)
                return View(rehearsalRoom);

            return NotFound();
        }

        // POST: RehearsalRooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int roomId)
        {
            if (_reservationService.RemoveRoom(roomId))
            {
                TempData["SuccessAlert"] = "A termet sikeresen töröltük!";
                return RedirectToAction(nameof(Index));
            }

            TempData["DangerAlert"] = "A terem törlése sikertelen!";
            var rehearsalRoom = _reservationService.GetRoom(roomId);
            return View(rehearsalRoom);
        }

        private bool RehearsalRoomExists(int id)
        {
            return _reservationService.Rooms.Any(e => e.Id == id);
        }

        public PartialViewResult AddPicturesToRoomPartial(int? roomId)
        {
            if (roomId == null)
                return null;

            ImageUploadViewModel viewModel = new ImageUploadViewModel
            {
                EntityId = (int)roomId
            };


            return PartialView("_AddPicturesToRoomPartial", viewModel);
        }

        public PartialViewResult GetAboutRoomPartial(int? roomId)
        {
            if (roomId == null)
                return null;

            var room = _reservationService.GetRoom(roomId);

            if (room == null)
                return null;
            
            return PartialView("_AboutRoomPartial", room);
        }

        public PartialViewResult GetEquipmentsListPartial(int? roomId)
        {
            if (roomId == null)
                return null;

            var room = _reservationService.GetRoom(roomId);

            if (room == null)
                return null;
            
            return PartialView("_EquipmentsListPartial", room);
        }

        public PartialViewResult GetImagesForRoomPartial(int? roomId)
        {
            if (roomId == null)
                return null;

            ViewBag.Images = _reservationService.GetImagesForRoom((int)roomId);
            return PartialView("_RoomImagesPartial");
        }

        public PartialViewResult GetReservationsTableForRoomPartial(int? roomId)
        {
            if (roomId == null)
                return null;

            var room = _reservationService.GetRoom(roomId);

            if (room == null)
                return null;
            
            return PartialView("_ReservationsTableRoomPartial", room);
        }

        [HttpPost]
        public IActionResult AddImages(ImageUploadViewModel viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                    return RedirectToAction("Details", "RehearsalRooms", new { roomId = viewModel.EntityId });

                if (viewModel.Images == null || viewModel.Images.Count == 0)
                {
                    TempData["DangerAlert"] = "Válasszon ki képeket!";
                    return RedirectToAction("Details", "RehearsalRooms", new { roomId = viewModel.EntityId });
                }

                //the input field allows us to upload multiple files, so we have to handle all of them

                foreach (var image in viewModel.Images)
                {
                    string ext = Path.GetExtension(image.FileName);
                    if (ext != ".jpg" && ext != ".JPG" && ext != ".png" && ext != ".PNG")
                    {
                        TempData["DangerAlert"] = "A feltölteni kívánt fájlok formátuma csak PNG vagy JPG lehet.";
                        ModelState.AddModelError("", "A feltölteni kívánt fájlok formátuma csak PNG vagy JPG lehet.");
                        return RedirectToAction("Details", "RehearsalRooms", new { roomId = viewModel.EntityId });
                    }
                }
                
                if (!_reservationService.UploadImagesForRoom(viewModel.EntityId, viewModel.Images))
                {
                    TempData["DangerAlert"] = "Valami hiba történt, próbálja újra!";
                    ModelState.AddModelError("", "Valami hiba történt, próbálja újra!");
                    return RedirectToAction("Details", "RehearsalRooms", new { roomId = viewModel.EntityId });
                }
            }
            catch
            {
                TempData["DangerAlert"] = "Valami hiba történt, próbálja újra!";
                ModelState.AddModelError("", "Valami hiba történt, próbálja újra!");
                return RedirectToAction("Details", "RehearsalRooms", new { roomId = viewModel.EntityId });
            }

            TempData["SuccessAlert"] = "Képek feltöltése sikeres!";
            return RedirectToAction("Details", "RehearsalRooms", new { roomId = viewModel.EntityId });
        }

        public FileResult GetImage(int? imageId)
        {
            if (imageId == null)
                return File("~/images/noimage.png", "image/jpeg");

            byte[] image = _reservationService.GetRoomImage(imageId);

            if (image == null)
                return File("~/images/noimage.png", "image/jpeg");

            return File(image, "image/png"); //returns the image correctly even it is jpg
        }

        public FileResult GetDefaultImage(int? roomId)
        {
            if (roomId == null)
                return File("~/images/noimage.png", "image/jpeg");

            byte[] image = _reservationService.GetDefaultRoomImage(roomId);

            if (image == null)
                return File("~/images/noimage.png", "image/jpeg");

            return File(image, "image/png"); //returns the image correctly even it is jpg
        }

        [HttpGet]
        public IActionResult DeleteRoomImages(int? roomId)
        {
            if (roomId == null)
                return NotFound();

            DeleteRoomImagesViewModel viewModel = new DeleteRoomImagesViewModel
            {
                RoomId = (int)roomId
            };

            List<int> imageIds = _reservationService.GetImagesForRoom((int)roomId);
            viewModel.Images = new Dictionary<int, bool>();

            foreach (var id in imageIds)
            {
                viewModel.Images.Add(id, true);
            }

            return View("DeleteRoomImages", viewModel);
        }

        [HttpPost]
        public IActionResult DeleteRoomImages(DeleteRoomImagesViewModel viewModel)
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

            if (!_reservationService.DeleteImagesForRoom(viewModel.RoomId, imagesToDelete))
            {
                TempData["DangerAlert"] = "Képek törlése sikertelen, kérem próbálja újra!";
                return View(viewModel);
            }

            TempData["SuccessAlert"] = "Képek törlése sikeres!";
            return RedirectToAction("Details", new { roomId = viewModel.RoomId });
        }
    }
}
