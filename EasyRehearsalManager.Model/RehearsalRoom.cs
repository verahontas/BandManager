using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace EasyRehearsalManager.Model
{
    public class RehearsalRoom
    {
        public RehearsalRoom()
        {
            Reservations = new HashSet<Reservation>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public int Number { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Pozitív számot adjon meg!")]
        public int Price { get; set; }

        /// <summary>
        /// Size of the room in m2.
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Pozitív számot adjon meg!")]
        public int Size { get; set; }

        /// <summary>
        /// Indicates whether a reservation is enabled to make in this room or not.
        /// E. g. a room can be unavailable because of its renovation.
        /// </summary>
        public bool Available { get; set; }

        [ForeignKey("RehearsalStudio")]
        public int StudioId { get; set; }

        public RehearsalStudio Studio { get; set; }

        public ICollection<Reservation> Reservations { get; set; }

        /// <summary>
        /// Description of stuff in the room.
        /// Note: equipments written here cannot be booked. These are fixed parts of the room.
        /// </summary>
        [DataType(DataType.MultilineText)]
        public string Equipments { get; set; }

        public string GetReservationForDate(DateTime start, DateTime end)
        {
            foreach (Reservation reservation in Reservations)
            {
                if (reservation.Start == start && reservation.End >= end
                    || reservation.Start <= start && reservation.End == end
                    || reservation.Start >= start && reservation.End <= end)
                    return reservation.BandName;
            }

            return "Foglalás";
        }
    }
}
