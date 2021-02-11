using System;
using System.Collections.Generic;
using System.Text;

namespace EasyRehearsalManager.Model
{
    public class StudioImage
    {
        public int Id { get; set; }

        public int StudioId { get; set; }

        public byte[] Image { get; set; }
    }
}
