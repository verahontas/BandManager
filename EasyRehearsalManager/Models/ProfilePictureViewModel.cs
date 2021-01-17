using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyRehearsalManager.Web.Models
{
    public class ProfilePictureViewModel
    {
        public int UserId { get; set; }

        public byte[] ProfilePicture { get; set; }
    }
}
