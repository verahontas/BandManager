using EasyRehearsalManager.Model;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyRehearsalManager.Web.Models
{
    public class ReservationTableViewModel
    {
        public int OpeningHour { get; set; }

        public int ClosingHour { get; set; }

        public int NumberOfAvailableRooms { get; set; }

        public int Index { get; set; }

        public List<RehearsalRoom> Rooms { get; set; }

        public List<Reservation> Reservations { get; set; }

        public RehearsalStudio Studio { get; set; }
    }
}
