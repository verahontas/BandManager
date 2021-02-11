﻿using System;
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

        public byte[] Logo { get; set; }

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

        public int CheapestRoom()
        {
            if (Rooms == null || Rooms.Count() == 0)
                return 0;

            return Rooms.OrderBy(l => l.Price).First().Price;
        }

        public int MostExpensiveRoom()
        {
            if (Rooms == null || Rooms.Count() == 0)
                return 0;

            return Rooms.OrderByDescending(l => l.Price).First().Price;
        }

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

        /// <summary>
        /// Gets the opening hour for a specific day.
        /// </summary>
        /// <param name="date">Date contains the day of week so we can determine the proper hour.</param>
        /// <returns></returns>
        public int GetOpeningHour(DateTime date)
        {
            return date.DayOfWeek switch
            {
                DayOfWeek.Sunday => OpeningHourSunday,
                DayOfWeek.Monday => OpeningHourMonday,
                DayOfWeek.Tuesday => OpeningHourTuesday,
                DayOfWeek.Wednesday => OpeningHourWednesday,
                DayOfWeek.Thursday => OpeningHourThursday,
                DayOfWeek.Friday => OpeningHourFriday,
                DayOfWeek.Saturday => OpeningHourSaturday,
                _ => -1,
            };
        }

        /// <summary>
        /// Gets the closing hour for a specific day.
        /// </summary>
        /// <param name="date">Date contains the day of week so we can determine the proper hour.</param>
        /// <returns></returns>
        public int GetClosingHour(DateTime date)
        {
            return date.DayOfWeek switch
            {
                DayOfWeek.Sunday => ClosingHourSunday,
                DayOfWeek.Monday => ClosingHourMonday,
                DayOfWeek.Tuesday => ClosingHourTuesday,
                DayOfWeek.Wednesday => ClosingHourWednesday,
                DayOfWeek.Thursday => ClosingHourThursday,
                DayOfWeek.Friday => ClosingHourFriday,
                DayOfWeek.Saturday => ClosingHourSaturday,
                _ => -1,
            };
        }
    }
}
