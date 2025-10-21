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
        public Reservation() { }

        public int Id { get; set; }

        [Required]
        public DateTime Start { get; set; }

        [Required]
        public DateTime End { get; set; }

        public int? UserId { get; set; }

        public User User { get; set; }

        public string BandName { get; set; }

        public int RehearsalRoomId { get; set; }

        public RehearsalRoom RehearsalRoom { get; set; }

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
