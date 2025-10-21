using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
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
                SeedUsersAndRoles(imageDirectory);

            if (!_context.Studios.Any())
                SeedStudios();

            if(!_context.Equipments.Any())
                SeedEquipments();

            if (!_context.Rooms.Any())
                SeedRooms();

            if (!_context.Reservations.Any())
                SeedReservations();

            if (!_context.ReservationEquipmentPairs.Any())
                SeedReservationEquipmentPairs();

            if (!_context.StudioImages.Any())
                SeedRehearsalStudioImages(imageDirectory);

            if (!_context.RoomImages.Any())
                SeedRehearsalRoomImages(imageDirectory);
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
                Description = "Próbahely­ünkön 6 próbaterem áll készen a zenészek fogadására, 23 m2-től 38 m2-ig. A próbahely­en légcserélő működik, a két legnagyobb próbaterem­ben pedig légkondi is van."
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

        private static void SeedUsersAndRoles(string imageDirectory)
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

            var result1 =_userManager.CreateAsync(adminUser, adminPassword).Result;
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

            if (Directory.Exists(imageDirectory))
            {
                var imagePath = Path.Combine(imageDirectory, "kovacspetertak.jpg");
                if (File.Exists(imagePath))
                {
                    owner1.ProfilePicture = File.ReadAllBytes(imagePath);
                }
            }
            
            owner1.SecurityStamp = Guid.NewGuid().ToString();

            var pwd2 = "Durerszervezo123";
            var ownerRole = new IdentityRole<int>("owner");

            var result7 = _userManager.CreateAsync(owner1, pwd2).Result;
            var result8 = _roleManager.CreateAsync(ownerRole).Result;
            var result9 = _userManager.AddToRoleAsync(owner1, ownerRole.Name).Result;
        }
        
        private static void SeedRehearsalStudioImages(string imageDirectory)
        {
            if (Directory.Exists(imageDirectory))
            {
                List<StudioImage> defaultImages = new List<StudioImage>();

                var imagePath = Path.Combine(imageDirectory, "probazona1.jpg");
                if (File.Exists(imagePath))
                {
                    defaultImages.Add(new StudioImage
                    {
                        StudioId = 1,
                        Image = File.ReadAllBytes(imagePath)
                    });
                }

                imagePath = Path.Combine(imageDirectory, "probazona2.png");
                if (File.Exists(imagePath))
                {
                    defaultImages.Add(new StudioImage
                    {
                        StudioId = 1,
                        Image = File.ReadAllBytes(imagePath)
                    });
                }

                imagePath = Path.Combine(imageDirectory, "probazona3.png");
                if (File.Exists(imagePath))
                {
                    defaultImages.Add(new StudioImage
                    {
                        StudioId = 1,
                        Image = File.ReadAllBytes(imagePath)
                    });
                }

                foreach (StudioImage image in defaultImages)
                    _context.StudioImages.Add(image);

                _context.SaveChanges();
            }
        }

        private static void SeedRehearsalRoomImages(string imageDirectory)
        {
            if (Directory.Exists(imageDirectory))
            {
                List<RoomImage> defaultImages = new List<RoomImage>();

                var imagePath = Path.Combine(imageDirectory, "probazona_terem_1.jpg");
                if (File.Exists(imagePath))
                {
                    defaultImages.Add(new RoomImage
                    {
                        RoomId = 3,
                        Image = File.ReadAllBytes(imagePath)
                    });
                }

                imagePath = Path.Combine(imageDirectory, "probazona_terem_2.jpg");
                if (File.Exists(imagePath))
                {
                    defaultImages.Add(new RoomImage
                    {
                        RoomId = 2,
                        Image = File.ReadAllBytes(imagePath)
                    });
                }

                imagePath = Path.Combine(imageDirectory, "probazona_terem_3.jpg");
                if (File.Exists(imagePath))
                {
                    defaultImages.Add(new RoomImage
                    {
                        RoomId = 1,
                        Image = File.ReadAllBytes(imagePath)
                    });
                }

                imagePath = Path.Combine(imageDirectory, "probazona_terem_4.jpg");
                if (File.Exists(imagePath))
                {
                    defaultImages.Add(new RoomImage
                    {
                        RoomId = 4,
                        Image = File.ReadAllBytes(imagePath)
                    });
                }

                imagePath = Path.Combine(imageDirectory, "probazona_terem_5.jpg");
                if (File.Exists(imagePath))
                {
                    defaultImages.Add(new RoomImage
                    {
                        RoomId = 5,
                        Image = File.ReadAllBytes(imagePath)
                    });
                }

                imagePath = Path.Combine(imageDirectory, "probazona_terem_6.jpg");
                if (File.Exists(imagePath))
                {
                    defaultImages.Add(new RoomImage
                    {
                        RoomId = 6,
                        Image = File.ReadAllBytes(imagePath)
                    });
                }

                foreach (RoomImage image in defaultImages)
                    _context.RoomImages.Add(image);

                _context.SaveChanges();
            }
        }

        private static void SeedReservationEquipmentPairs()
        {
            IList<ReservationEquipmentPair> defaultReservations = new List<ReservationEquipmentPair>();

            defaultReservations.Add(new ReservationEquipmentPair
            {
                StudioId = 1,
                ReservationId = 1,
                EquipmentId = 2,
                EquipmentName = "crash"
            });
        }
    }
}
