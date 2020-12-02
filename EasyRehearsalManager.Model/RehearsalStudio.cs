using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EasyRehearsalManager.Model
{
    public class RehearsalStudio
    {
        public RehearsalStudio()
        {
            Rooms = new HashSet<RehearsalRoom>();
            Equipments = new HashSet<Equipment>();
        }

        [Key]
        public int Id { get; set; }

        //owner of the studio
        [ForeignKey("User")]
        public int UserId { get; set; }

        public User User { get; set; }

        [Required]
        [MaxLength(80)]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        public double LocationX { get; set; }

        public double LocationY { get; set; }

        [Range(1, 23)]
        public int District { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Url]
        public string Web { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public ICollection<RehearsalRoom> Rooms { get; set; }

        //Equipments that you can book with a reservation and that aren't in the rooms by default (eg. keyboard, crash, double pedal, ...)
        public ICollection<Equipment> Equipments { get; set; }


        //opening and closing hours every day

        [Range(0, 24)]
        public int OpeningHourMonday { get; set; }

        [Range(0, 24)]
        public int ClosingHourMonday { get; set; }

        [Range(0, 24)]
        public int OpeningHourTuesday { get; set; }

        [Range(0, 24)]
        public int ClosingHourTuesday { get; set; }

        [Range(0, 24)]
        public int OpeningHourWednesday { get; set; }

        [Range(0, 24)]
        public int ClosingHourWednesday { get; set; }

        [Range(0, 24)]
        public int OpeningHourThursday { get; set; }

        [Range(0, 24)]
        public int ClosingHourThursday { get; set; }

        [Range(0, 24)]
        public int OpeningHourFriday { get; set; }

        [Range(0, 24)]
        public int ClosingHourFriday { get; set; }

        [Range(0, 24)]
        public int OpeningHourSaturday { get; set; }

        [Range(0, 24)]
        public int ClosingHourSaturday { get; set; }

        [Range(0, 24)]
        public int OpeningHourSunday { get; set; }

        [Range(0, 24)]
        public int ClosingHourSunday { get; set; }

        public int EarliestOpeningHour()
        {
            int min = 24;
            if (OpeningHourMonday <= min) min = OpeningHourMonday;
            if (OpeningHourTuesday <= min) min = OpeningHourTuesday;
            if (OpeningHourWednesday <= min) min = OpeningHourWednesday;
            if (OpeningHourThursday <= min) min = OpeningHourThursday;
            if (OpeningHourFriday <= min) min = OpeningHourFriday;
            if (OpeningHourSaturday <= min) min = OpeningHourSaturday;
            if (OpeningHourSunday <= min) min = OpeningHourSunday;

            return min;
        }

        public int LatestClosingHour()
        {
            int max = 0;
            if (ClosingHourMonday >= max) max = ClosingHourMonday;
            if (ClosingHourTuesday >= max) max = ClosingHourTuesday;
            if (ClosingHourWednesday >= max) max = ClosingHourWednesday;
            if (ClosingHourThursday >= max) max = ClosingHourThursday;
            if (ClosingHourFriday >= max) max = ClosingHourFriday;
            if (ClosingHourSaturday >= max) max = ClosingHourSaturday;
            if (ClosingHourSunday >= max) max = ClosingHourSunday;

            return max;
        }

        public int GetOpeningHour(DateTime date)
        {
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    return OpeningHourSunday;
                case DayOfWeek.Monday:
                    return OpeningHourMonday;
                case DayOfWeek.Tuesday:
                    return OpeningHourTuesday;
                case DayOfWeek.Wednesday:
                    return OpeningHourWednesday;
                case DayOfWeek.Thursday:
                    return OpeningHourThursday;
                case DayOfWeek.Friday:
                    return OpeningHourFriday;
                case DayOfWeek.Saturday:
                    return OpeningHourSaturday;
                default:
                    return 0;
            }
        }

        public int GetClosingHour(DateTime date)
        {
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    return ClosingHourSunday;
                case DayOfWeek.Monday:
                    return ClosingHourMonday;
                case DayOfWeek.Tuesday:
                    return ClosingHourTuesday;
                case DayOfWeek.Wednesday:
                    return ClosingHourWednesday;
                case DayOfWeek.Thursday:
                    return ClosingHourThursday;
                case DayOfWeek.Friday:
                    return ClosingHourFriday;
                case DayOfWeek.Saturday:
                    return ClosingHourSaturday;
                default:
                    return 0;
            }
        }
    }
}
