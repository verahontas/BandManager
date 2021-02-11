using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EasyRehearsalManager.Model
{
    public class User : IdentityUser<int>
    {
        public User()
        {
            Reservations = new HashSet<Reservation>();
            RehearsalStudios = new HashSet<RehearsalStudio>();
        }

        [Key]
        public override int Id { get; set; }

        public string UserOwnName { get; set; }

        public string DefaultBandName { get; set; }

        public byte[] ProfilePicture { get; set; }

        //only for musicians, not required
        public ICollection<Reservation> Reservations { get; set; }

        //only for owners, can be empty
        public ICollection<RehearsalStudio> RehearsalStudios { get; set; } 
    }
}
