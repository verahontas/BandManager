using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EasyRehearsalManager.Model
{
    public class RehearsalRoom
    {

        public RehearsalRoom()
        {
            Reservations = new HashSet<Reservation>();
            Equipments = new HashSet<Equipment>();
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

        //size of the room in m2
        [Range(0, int.MaxValue, ErrorMessage = "Pozitív számot adjon meg!")]
        public int Size { get; set; }

        //can you make a reservation or not (eg. not available because of renovation)
        public bool Available { get; set; }

        //[Required]
        //[DisplayName("Studio")]
        [ForeignKey("RehearsalStudio")]
        public int StudioId { get; set; }

        public RehearsalStudio Studio { get; set; }

        public ICollection<Reservation> Reservations { get; set; }


        //default equipments in the room that you dont need to reserve
        public ICollection<Equipment> Equipments { get; set; }
    }
}
