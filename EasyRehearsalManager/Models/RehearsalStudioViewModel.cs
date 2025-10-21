using EasyRehearsalManager.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyRehearsalManager.Web.Models
{
    public class RehearsalStudioViewModel : RehearsalStudio
    {
        public IFormFile LogoImage { get; set; }
    }
}
