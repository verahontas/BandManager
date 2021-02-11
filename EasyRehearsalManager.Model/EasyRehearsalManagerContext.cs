using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyRehearsalManager.Model
{
    public class EasyRehearsalManagerContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public EasyRehearsalManagerContext() { }

        public EasyRehearsalManagerContext(DbContextOptions<EasyRehearsalManagerContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>().ToTable("Users");
        }

        public virtual DbSet<RehearsalStudio> Studios { get; set; }

        public virtual DbSet<RehearsalRoom> Rooms { get; set; }

        public virtual DbSet<Equipment> Equipments { get; set; }

        public virtual DbSet<Reservation> Reservations { get; set; }

        public virtual DbSet<RoomImage> RoomImages { get; set; }

        public virtual DbSet<StudioImage> StudioImages { get; set; }

        public virtual DbSet<ReservationEquipmentPair> ReservationEquipmentPairs { get; set; }
    }
}
