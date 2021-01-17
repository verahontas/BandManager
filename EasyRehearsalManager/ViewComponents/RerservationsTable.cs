//using AspNetCore;
/*using EasyRehearsalManager.Model;
using EasyRehearsalManager.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyRehearsalManager.Web.ViewComponents
{
    public class RerservationsTableViewComponent : ViewComponent
    {
        public int DayIndex;
        public ReservationService _reservationService;
        public int[] OpeningHours = new int[7];
        public int[] ClosingHours = new int[7];

        public RerservationsTableViewComponent(ReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        public IViewComponentResult InvokeAsync(int studioId, bool isNext)
        {
            //if (DayIndex < 0)
            //    return NotFound();

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


            return View(viewModel);
        }

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
    }
}
*/