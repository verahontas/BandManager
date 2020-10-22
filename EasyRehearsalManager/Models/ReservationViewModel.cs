using EasyRehearsalManager.Model;
using EasyRehearsalManager.Web.Migrations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EasyRehearsalManager.Web.Models
{
    public class ReservationViewModel : MusicianViewModel
    {
        public ReservationViewModel()
        {
            Equipments = new Dictionary<string, bool>();
            //Equipments = new List<EquipmentToBook>();
        }

        public RehearsalRoom Room { get; set; }

        //day of the reservation
        public DateTime Day { get; set; }

        [Required(ErrorMessage = "A foglalás kezdetének megadása kötelező.")]
        [Range(0, 23)]
        public int StartHour { get; set; }

        [Required(ErrorMessage = "A foglalás végének megadása kötelező.")]
        [Range(0, 23)]
        public int EndHour { get; set; }

        public int Id { get; set; }

        public Dictionary<string, bool> Equipments { get; set; }

        //public List<EquipmentToBook> Equipments { get; set; }
    }

    public struct EquipmentToBook
    {
        public bool IsChecked { get; set; }

        public Equipment Equipment { get; set; }
    };
}
