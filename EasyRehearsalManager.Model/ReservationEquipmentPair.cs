using System;
using System.Collections.Generic;
using System.Text;

namespace EasyRehearsalManager.Model
{
    public class ReservationEquipmentPair
    {
        public int Id { get; set; }

        public int StudioId { get; set; }

        public int EquipmentId { get; set; }

        public string EquipmentName { get; set; }

        public int ReservationId { get; set; }
    }
}
