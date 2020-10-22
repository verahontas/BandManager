using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EasyRehearsalManager.Model
{
    public class Reservation
    {
        public Reservation()
        {
            Equipments = new List<Equipment>();
        }

        public int Id { get; set; }

        [Required]
        public DateTime Start { get; set; }

        [Required]
        public DateTime End { get; set; }

        //[ForeignKey("User")]
        public int? UserId { get; set; }

        public User User { get; set; }

        public string BandName { get; set; }

        //[ForeignKey("RehearsalRoom")]
        public int RehearsalRoomId { get; set; }

        public RehearsalRoom RehearsalRoom { get; set; }

        //bookable equipments which are not in the room by default
        public List<Equipment> Equipments { get; set; }

        public bool IsConflicting(DateTime start, DateTime end)
        {
            return Start >= start && Start < end ||
                           End >= start && End < end ||
                           Start < start && End > end ||
                           Start > start && End < end;
        }

        public Boolean IsConflicting(DateTime date)
        {
            return IsConflicting(date, date + TimeSpan.FromDays(1));
        }
    }
}
