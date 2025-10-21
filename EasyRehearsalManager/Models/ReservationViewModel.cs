using EasyRehearsalManager.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EasyRehearsalManager.Web.Models
{
    public class ReservationViewModel
    {
        public ReservationViewModel()
        {
            Equipments = new Dictionary<string, bool>();
        }

        public int UserId { get; set; }

        public string BandName { get; set; }

        public string UserOwnName { get; set; }

        public int RoomId { get; set; }

        public int RoomNumber { get; set; }

        public string StudioName { get; set; }

        /// <summary>
        /// Day of the reservation.
        /// </summary>
        public DateTime Day { get; set; }

        [Required(ErrorMessage = "A foglalás kezdetének megadása kötelező.")]
        [Range(0, 23)]
        public int StartHour { get; set; }

        [Required(ErrorMessage = "A foglalás végének megadása kötelező.")]
        [Range(0, 23)]
        public int EndHour { get; set; }

        /// <summary>
        /// Id of the reservation.
        /// </summary>
        public int ReservationId { get; set; }

        public Dictionary<string, bool> Equipments { get; set; }
    }
}
