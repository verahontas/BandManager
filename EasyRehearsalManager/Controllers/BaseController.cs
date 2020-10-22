using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EasyRehearsalManager.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EasyRehearsalManager.Web.Controllers
{
    public class BaseController : Controller
    {
        protected readonly IReservationService _reservationService;
        protected readonly ApplicationState _applicationState;

        public BaseController(IReservationService reservationService, ApplicationState applicationState)
        {
            _applicationState = applicationState;
            _reservationService = reservationService;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);

            ViewBag.UserCount = _applicationState.UserCount;
            ViewBag.CurrentUserName = String.IsNullOrEmpty(User.Identity.Name) ? null : User.Identity.Name;
            ViewBag.CurrentUserId = String.IsNullOrEmpty(User.Identity.Name) ? null : User.FindFirst(ClaimTypes.NameIdentifier).Value;

            ViewBag.Rooms = _reservationService.Rooms;
            ViewBag.Reservations = _reservationService.Reservations;

            if (User.IsInRole("musician"))
                ViewBag.CurrentUserRole = "musician";
            else if (User.IsInRole("owner"))
                ViewBag.CurrentUserRole = "owner";
            else if (User.IsInRole("administrator"))
                ViewBag.CurrentUserRole = "administrator";
            else
                ViewBag.CurrentUserRole = null;
        }
    }
}
