using System;
using System.Collections.Generic;
using System.Text;

namespace EasyRehearsalManager.Model
{
    public class RoomImage
    {
        public int Id { get; set; }

        public int RoomId { get; set; }

        public byte[] Image { get; set; }
    }
}
