using System;
using System.Collections.Generic;
using System.Text;

namespace EasyRehearsalManager.Model
{
    public class UserImage
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public byte[] ImageSmall { get; set; }

        public byte[] ImageLarge { get; set; }
    }
}
