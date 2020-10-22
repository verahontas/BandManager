using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyRehearsalManager.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace EasyRehearsalManager.Web.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IReservationService reservationService, ApplicationState applicationState)
            : base(reservationService, applicationState)
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        public FileResult GetUserImage(int? userId)
        {
            if (userId == null)
                return File("~/images/noprofilepicture.jpg", "image/jpeg");

            Byte[] imageContent = _reservationService.GetUserImage(userId);
            if (imageContent == null)
                return File("~/images/noprofilepicture.png", "image/jpeg");

            return File(imageContent, "image/jpeg");
        }
    }
}
