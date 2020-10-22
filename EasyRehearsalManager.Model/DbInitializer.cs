using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EasyRehearsalManager.Model
{
    public static class DbInitializer
    {
        private static EasyRehearsalManagerContext _context;
        private static UserManager<User> _userManager;
        private static RoleManager<IdentityRole<int>> _roleManager;

        public static void Initialize(IServiceProvider serviceProvider, string imageDirectory)
        {
            _context = serviceProvider.GetRequiredService<EasyRehearsalManagerContext>();
            _userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();

            _context.Database.Migrate();

            if (!_context.Users.Any())
                SeedUsersAndRoles();

            if (!_context.Studios.Any())
                SeedStudios();

            if(!_context.Equipments.Any())
                SeedEquipments();

            if (!_context.Rooms.Any())
                SeedRooms();

            if (!_context.Reservations.Any())
                SeedReservations();

            if (!_context.UserImages.Any())
                SeedUserImages(imageDirectory);
        }

        private static void SeedEquipments()
        {
            IList<Equipment> defaultEquipments = new List<Equipment>();

            defaultEquipments.Add(new Equipment
            {
                Name = "nagy szinti",
                QuantityAvailable = 3,
                StudioId = 1
            });

            defaultEquipments.Add(new Equipment
            {
                Name = "crash",
                QuantityAvailable = 8,
                StudioId = 1
            });

            defaultEquipments.Add(new Equipment
            {
                Name = "duplázó",
                QuantityAvailable = 2,
                StudioId = 1
            });

            foreach (Equipment equipment in defaultEquipments)
                _context.Equipments.Add(equipment);

            _context.SaveChanges();
        }

        private static void SeedStudios()
        {
            IList<RehearsalStudio> defaultStudios = new List<RehearsalStudio>();

            defaultStudios.Add(new RehearsalStudio
            {
                UserId = 3,
                Name = "Próbazóna",
                Address = "1055 Budapest,Vajkay u. 1., 13-as kapucsengő",
                Phone = "06306263251",
                Email = "info@probazona.hu",
                Web = "https://www.probazona.hu/",
                LocationX = 47.507507,
                LocationY = 19.049175,
                District = 5,
                OpeningHourMonday = 10,
                ClosingHourMonday = 22,
                OpeningHourTuesday = 10,
                ClosingHourTuesday = 22,
                OpeningHourWednesday = 10,
                ClosingHourWednesday = 22,
                OpeningHourThursday = 10,
                ClosingHourThursday = 22,
                OpeningHourFriday = 10,
                ClosingHourFriday = 22,
                OpeningHourSaturday = 10,
                ClosingHourSaturday = 22,
                OpeningHourSunday = 10,
                ClosingHourSunday = 22,
                Description = "Próbahely­ünkön 6 próbaterem áll készen a zenészek fogadására, 23 m2-től 38 m2-ig. A próbahely­en légcserélő működik, a két legnagyobb próbaterem­ben pedig légkondi is van.",
                NumberOfRooms = 6
            });

            foreach (RehearsalStudio studio in defaultStudios)
                _context.Studios.Add(studio);

            _context.SaveChanges();
        }

        private static void SeedRooms()
        {
            IList<RehearsalRoom> defaultRooms = new List<RehearsalRoom>();

            defaultRooms.Add(new RehearsalRoom
            {
                Number = 1,
                Description = "Standard próbaterem",
                Price = 2500,
                Size = 25,
                Available = true,
                StudioId = 1
            });

            defaultRooms.Add(new RehearsalRoom
            {
                Number = 2,
                Description = "A legolcsóbb próbaterem",
                Price = 2200,
                Size = 23,
                Available = true,
                StudioId = 1
            });

            defaultRooms.Add(new RehearsalRoom
            {
                Number = 3,
                Description = "Standard próbaterem",
                Price = 2500,
                Size = 27,
                Available = true,
                StudioId = 1
            });

            defaultRooms.Add(new RehearsalRoom
            {
                Number = 4,
                Description = "légkondis próbaterem",
                Price = 2900,
                Size = 33,
                Available = true,
                StudioId = 1
            });

            defaultRooms.Add(new RehearsalRoom
            {
                Number = 5,
                Description = "prémium, légkondis próbaterem",
                Price = 2900,
                Size = 38,
                Available = true,
                StudioId = 1
            });

            defaultRooms.Add(new RehearsalRoom
            {
                Number = 6,
                Description = "hangulatos próbaterem",
                Price = 2500,
                Size = 25,
                Available = true,
                StudioId = 1
            });

            foreach (RehearsalRoom room in defaultRooms)
                _context.Rooms.Add(room);

            _context.SaveChanges();
        }

        private static void SeedReservations()
        {
            
        }

        private static void SeedUsersAndRoles()
        {
            var adminUser = new User
            {
                UserOwnName = "Adminisztrátor",
                UserName = "admin",
                Email = "admin@admin.com",
                PhoneNumber = "+36123456789"
            };

            var adminPassword = "Almafa123";
            var adminRole = new IdentityRole<int>("administrator");

            var result1 = _userManager.CreateAsync(adminUser, adminPassword).Result;
            var result2 = _roleManager.CreateAsync(adminRole).Result;
            var result3 = _userManager.AddToRoleAsync(adminUser, adminRole.Name).Result;

            var musician1 = new User
            {
                UserOwnName = "Herczegfalvy Veronika",
                UserName = "szintigurl",
                Email = "veronika@example.com",
                PhoneNumber = "+36301234567",
                DefaultBandName = "Petricor"
            };

            musician1.SecurityStamp = Guid.NewGuid().ToString();

            var pwd1 = "Szintigurl123";
            var musicianRole = new IdentityRole<int>("musician");

            var result4 = _userManager.CreateAsync(musician1, pwd1).Result;
            var result5 = _roleManager.CreateAsync(musicianRole).Result;
            var result6 = _userManager.AddToRoleAsync(musician1, musicianRole.Name).Result;

            var owner1 = new User
            {
                UserOwnName = "Takács Pétek Tak",
                UserName = "durerszervezo",
                Email = "tak@durer.com",
                PhoneNumber = "+36301234567"
            };

            owner1.SecurityStamp = Guid.NewGuid().ToString();

            var pwd2 = "Durerszervezo123";
            var ownerRole = new IdentityRole<int>("owner");

            var result7 = _userManager.CreateAsync(owner1, pwd2).Result;
            var result8 = _roleManager.CreateAsync(ownerRole).Result;
            var result9 = _userManager.AddToRoleAsync(owner1, ownerRole.Name).Result;
        }

        private static void SeedUserImages(string imageDirectory)
        {
            if (Directory.Exists(imageDirectory))
            {
                var images = new List<UserImage>();

                var largePath = Path.Combine(imageDirectory, "kovacspetertak.jpg");
                var smallPath = Path.Combine(imageDirectory, "kovacspetertak.jpg");

                if (File.Exists(largePath) && File.Exists(smallPath))
                {
                    images.Add(new UserImage
                    {
                        UserId = _context.Users.FirstOrDefault(l => l.UserName == "durerszervezo").Id,
                        ImageLarge = File.ReadAllBytes(largePath),
                        ImageSmall = File.ReadAllBytes(smallPath)
                    });
                }

                foreach (var image in images)
                {
                    _context.UserImages.Add(image);
                }

                _context.SaveChanges();
            }
        }
    }
}
