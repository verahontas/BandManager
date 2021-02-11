using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyRehearsalManager.Web.Models
{
    public class DeleteRoomImagesViewModel
    {
        public int RoomId { get; set; }

        /// <summary>
        /// int: represents the image's id
        /// bool: false if image is to delete
        /// </summary>
        public Dictionary<int, bool> Images { get; set; }
    }
}
