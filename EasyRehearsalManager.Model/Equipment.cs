using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EasyRehearsalManager.Model
{
    public class Equipment
    {
        public int Id { get; set; }

        public int StudioId { get; set; }

        public RehearsalStudio Studio { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int QuantityAvailable { get; set; }
    }
}
