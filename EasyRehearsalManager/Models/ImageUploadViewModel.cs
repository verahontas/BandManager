using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyRehearsalManager.Web.Models
{
    /// <summary>
    /// This class is made for image upload.
    /// In case you want to upload one image to an entity that has one image property,
    /// the <input> field on the frontend page doesn't enable to upload multiple images.
    /// In case you want to upload multiple images e.g. for a studio,
    /// then the input enables multiple file upload.
    /// </summary>
    public class ImageUploadViewModel
    {
        public int EntityId { get; set; }

        public List<IFormFile> Images { get; set; }
    }
}
