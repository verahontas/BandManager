using EasyRehearsalManager.Model;
using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EasyRehearsalManager.Web.Models
{
    public class ReservationService : IReservationService
    {
        private readonly EasyRehearsalManagerContext _context;
        private readonly ReservationDateValidator _reservationDateValidator;
        private readonly UserManager<User> _userManager;

        //ez a teszthez kell (ha a másik van akkor ez kell?)
        /*public ReservationService(EasyRehearsalManagerContext context)
        {
            _context = context;
        }*/

        //ez is a teszthez kell
        public ReservationService(EasyRehearsalManagerContext context, ReservationDateValidator validator)
        {
            _context = context;
            _reservationDateValidator = validator;
        }

        public ReservationService(EasyRehearsalManagerContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
            _reservationDateValidator = new ReservationDateValidator(_context);
        }

        #region Data sets

        public IEnumerable<RehearsalStudio> Studios => _context.Studios
                                                            .Include(l => l.Rooms)
                                                            .Include(l => l.Equipments)
                                                            .Include(l => l.User)
                                                            .OrderBy(l => l.Name);

        public IEnumerable<RehearsalRoom> Rooms => _context.Rooms
                                                        .Include(l => l.Reservations)
                                                        .Include(l => l.Studio)
                                                        .Include(l => l.Studio.Equipments)
                                                        .OrderBy(l => l.Number);

        public IEnumerable<Reservation> Reservations => _context.Reservations
                                                            .Include(l => l.RehearsalRoom)
                                                            .Include(l => l.RehearsalRoom.Studio)
                                                            .OrderByDescending(l => l.Start);

        public IEnumerable<Equipment> Equipments => _context.Equipments
                                                        .Include(l => l.Studio);

        public IEnumerable<ReservationEquipmentPair> ReservationEquipmentPairs => _context.ReservationEquipmentPairs;

        public IEnumerable<User> Users => _context.Users
                                            .Include(l => l.Reservations)
                                            .Include(l => l.RehearsalStudios);

        #endregion

        #region Operations with studios

        public RehearsalStudio GetStudio(int? studioId)
        {
            if (studioId == null)
                return null;

            return Studios.FirstOrDefault(l => l.Id == studioId);
        }

        public RehearsalStudio GetStudioByRoomId(int? roomId)
        {
            if (roomId == null)
                return null;

            foreach (var room in Rooms)
            {
                if (room.Id == roomId)
                    return room.Studio;
            }

            return null;
        }

        public bool AddStudio(RehearsalStudio studio)
        {
            _context.Studios.Add(studio);
            try
            {
                _context.SaveChanges();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool RemoveStudio(int? studioId)
        {
            if (studioId == null)
                return false;

            var studio = _context.Studios.FirstOrDefault(l => l.Id == studioId);

            _context.Studios.Remove(studio);

            try
            {
                _context.SaveChanges();
            }
            catch
            {
                return false;
            }
            
            //deleting all rooms that belonged to this studio
            //deleting all reservations that were in this studio (for any room)
            foreach (var room in Rooms)
            {
                if (room.StudioId == studioId)
                {
                    foreach (var reservation in Reservations)
                    {
                        if (reservation.RehearsalRoomId == room.Id)
                        {
                            _context.Reservations.Remove(reservation);
                        }
                    }
                    _context.Rooms.Remove(room);
                }
            }

            //foglalásokat is törölni
            try
            {
                _context.SaveChanges();
            }
            catch
            {

                return false;
            }
            
            return true;
        }

        public bool UpdateStudio(RehearsalStudio studio)
        {
            _context.Studios.Update(studio);

            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public IEnumerable<RehearsalStudio> GetStudiosByOwner(int? ownerId)
        {
            if (ownerId == null)
                return Studios;

            return Studios.Where(l => l.UserId == ownerId);
        }

        public int? GetStudioIdByEquipment(int? equipmentId)
        {
            if (equipmentId == null)
                return null;

            var equipment = Equipments.FirstOrDefault(l => l.Id == equipmentId);

            if (equipment == null)
                return null;

            return equipment.StudioId;
        }

        public bool RehearsalStudioExist(int? studioId)
        {
            if (studioId == null)
                return false;

            return _context.Studios.Select(l => l.Id).ToList().Contains((int)studioId);
        }

        public byte[] GetStudioImage(int? imageId)
        {
            if (imageId == null)
                return null;

            byte[] image = _context.StudioImages.FirstOrDefault(l => l.Id == imageId).Image;

            return image;
        }

        public byte[] GetLogo(int studioId)
        {
            byte[] image = Studios.FirstOrDefault(l => l.Id == studioId).Logo;

            return image;
        }

        public bool UploadImagesForStudio(int studioId, List<IFormFile> images)
        {
            foreach (var image in images)
            {
                byte[] fileBytes = null;
                if (image.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        image.CopyTo(ms);
                        fileBytes = ms.ToArray();
                    }
                }

                _context.StudioImages.Add(new StudioImage
                {
                    StudioId = studioId,
                    Image = fileBytes
                });
            }

            try
            {
                _context.SaveChanges();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool ChangeLogoForStudio(int studioId, IFormFile logo)
        {
            byte[] fileBytes = null;
            if (logo.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    logo.CopyTo(ms);
                    fileBytes = ms.ToArray();
                }
            }

            RehearsalStudio studio = Studios.FirstOrDefault(l => l.Id == studioId);
            studio.Logo = fileBytes;

            _context.Studios.Update(studio);

            try
            {
                _context.SaveChanges();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public List<int> GetImagesForStudio(int studioId)
        {
            return _context.StudioImages.Where(l => l.StudioId == studioId).Select(l => l.Id).ToList();
        }

        public bool DeleteImagesForStudio(int studioId, List<int> images)
        {
            foreach (var id in images)
            {
                StudioImage imageToDelete = _context.StudioImages.Where(l => l.StudioId == studioId).FirstOrDefault(l => l.Id == id);
                _context.StudioImages.Remove(imageToDelete);
            }

            try
            {
                _context.SaveChanges();
            }
            catch
            {
                return false;
            }

            return true;
        }

        #endregion

        #region Operations with rooms

        public RehearsalRoom GetRoom(int? roomId)
        {
            if (roomId == null)
                return null;

            return Rooms.FirstOrDefault(l => l.Id == roomId);
        }

        public bool AddRoom(RehearsalRoom room)
        {
            _context.Rooms.Add(room);
            try
            {
                _context.SaveChanges();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool RemoveRoom(int? roomId)
        {
            if (roomId == null)
                return false;

            var room = _context.Rooms.FirstOrDefault(l => l.Id == roomId);

            _context.Rooms.Remove(room);

            try
            {
                _context.SaveChanges();
            }
            catch
            {

                return false;
            }

            //deleting all reservations to this room
            foreach (var reservation in Reservations)
            {
                if (reservation.RehearsalRoomId == roomId)
                {
                    _context.Reservations.Remove(reservation);
                }
            }

            try
            {
                _context.SaveChanges();
            }
            catch
            {

                return false;
            }

            return true;
        }

        public bool UpdateRoom(RehearsalRoom room)
        {
            _context.Rooms.Update(room);

            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public IEnumerable<RehearsalRoom> GetRoomsByOwnerId(int? ownerId)
        {
            if (ownerId == null)
                return Rooms;

            return Rooms.Where(l => l.Studio.UserId == ownerId);
        }

        public byte[] GetRoomImage(int? imageId)
        {
            if (imageId == null)
                return null;

            byte[] image = _context.RoomImages.FirstOrDefault(l => l.Id == imageId).Image;

            return image;
        }

        public bool UploadImagesForRoom(int roomId, List<IFormFile> images)
        {
            foreach (var image in images)
            {
                byte[] fileBytes = null;
                if (image.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        image.CopyTo(ms);
                        fileBytes = ms.ToArray();
                    }
                }

                _context.RoomImages.Add(new RoomImage
                {
                    RoomId = roomId,
                    Image = fileBytes
                });
            }

            try
            {
                _context.SaveChanges();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public List<int> GetImagesForRoom(int roomId)
        {
            return _context.RoomImages.Where(l => l.RoomId == roomId).Select(l => l.Id).ToList();
        }

        public byte[] GetDefaultRoomImage(int? roomId)
        {
            if (roomId == null)
                return null;

            if (RoomImageExist((int)roomId))
            {
                return _context.RoomImages.FirstOrDefault(l => l.RoomId == roomId).Image;
            }

            return null;
        }

        /// <summary>
        /// Determines whether a room has any picture uploaded.
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
        private bool RoomImageExist(int roomId)
        {
            return _context.RoomImages.Where(l => l.RoomId == roomId).Any();
        }

        public bool DeleteImagesForRoom(int roomId, List<int> images)
        {
            foreach (var id in images)
            {
                RoomImage imageToDelete = _context.RoomImages.Where(l => l.RoomId == roomId).FirstOrDefault(l => l.Id == id); //just to be sure we're deleting the right picture
                if (imageToDelete != null)
                    _context.RoomImages.Remove(imageToDelete);
            }

            try
            {
                _context.SaveChanges();
            }
            catch
            {
                return false;
            }

            return true;
        }

        #endregion

        #region Operations with reservations

        /// <summary>
        /// Gets the reservation by ID.
        /// </summary>
        /// <param name="reservationId"></param>
        /// <returns>Returns the reservation, including the room and the studio.</returns>
        public Reservation GetReservation(int? reservationId)
        {
            if (reservationId == null)
                return null;

            return Reservations.FirstOrDefault(l => l.Id == reservationId);
        }

        public bool RemoveReservation(int? reservationId)
        {
            if (reservationId == null)
                return false;

            var reservation = _context.Reservations.FirstOrDefault(l => l.Id == reservationId);

            if (reservation == null)
                return false;

            _context.Reservations.Remove(reservation);

            try
            {
                _context.SaveChanges();
            }
            catch
            {

                return false;
            }

            return true;
        }

        public bool UpdateReservation(Reservation reservation)
        {
            _context.Reservations.Update(reservation);

            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public IEnumerable<Reservation> GetReservations(int userId, string role)
        {
            switch (role)
            {
                case "owner":
                    List<Reservation> result = new List<Reservation>();
                    foreach (var reservation in Reservations)
                    {
                        if (reservation.RehearsalRoom.Studio.UserId == userId)
                            result.Add(reservation);
                    }
                    return result;
                case "musician":
                    List<Reservation> res = new List<Reservation>();
                    foreach (var reservation in Reservations)
                    {
                        if (reservation.UserId == userId)
                            res.Add(reservation);
                    }
                    return res;
                default:
                    return Reservations;
            }
        }

        public IEnumerable<Reservation> GetReservationsByStudioId(int? studioId)
        {
            if (studioId == null)
                return null;

            return Reservations.Where(l => l.RehearsalRoom.StudioId == studioId);
        }

        public bool ReservationExist(int? reservationId)
        {
            if (reservationId == null)
                return false;

            var reservation = GetReservation(reservationId);

            if (reservation == null)
                return false;

            return true;
        }

        /// <summary>
        /// This function is only called when creating a reservation.
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="userId"></param>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public async Task<bool> SaveReservationAsync(int? roomId, int userId, ReservationViewModel viewModel)
        {
            if (roomId == null || viewModel == null)
                return false;

            if (!Validator.TryValidateObject(viewModel, new ValidationContext(viewModel, null, null), null))
                return false;

            DateTime start = viewModel.Day.AddHours(viewModel.StartHour);
            DateTime end = viewModel.Day.AddHours(viewModel.EndHour);

            if (_reservationDateValidator.Validate(start, end, roomId.Value, "create", viewModel.ReservationId, viewModel.Equipments) != ReservationDateError.None)
                return false;

            User user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
                return false;

            _context.Reservations.Add(new Reservation
            {
                RehearsalRoomId = (int)roomId,
                UserId = userId,
                User = user,
                Start = viewModel.Day.AddHours(viewModel.StartHour),
                End = viewModel.Day.AddHours(viewModel.EndHour),
                BandName = viewModel.BandName != "" ? viewModel.BandName : user.DefaultBandName
            });

            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }

            if (!AddReservationEquipmentPair((int)roomId, viewModel, user.Id))
                return false;

            return true;
        }

        //roomId cannot be null, because it is checked before this function is called
        public ReservationDateError ValidateReservation(DateTime start, DateTime end, string action, int reservationId, Dictionary<string, bool> equipments, int roomId)
        {
            return _reservationDateValidator.Validate(start, end, roomId, action, reservationId, equipments);
        }

        /*
        public IEnumerable<DateTime> GetReservationDates(int roomId, int year, int month)
        {
            List<DateTime> isAvailable = new List<DateTime>();

            DateTime start = new DateTime(year, month, 1) - TimeSpan.FromDays(50);
            DateTime end = new DateTime(year, month, 1) + TimeSpan.FromDays(80);

            if (end < DateTime.Today)
                return isAvailable;

            List<Reservation> possibleConflicts = _context.Reservations.Where(l => l.RehearsalRoomId == roomId && l.Start <= end && l.End >= start).ToList();

            for (DateTime date = start; date < end; date += TimeSpan.FromDays(1))
            {
                if (date > DateTime.Today &&
                    possibleConflicts.All(l => !l.IsConflicting(date)))
                {
                    isAvailable.Add(date);
                }
            }

            return isAvailable;
        }
        */

        #endregion

        #region Operations with equipments

        public Equipment GetEquipment(int? equipmentId)
        {
            if (equipmentId == null)
                return null;

            return Equipments.FirstOrDefault(l => l.Id == equipmentId);
        }

        public IEnumerable<Equipment> GetEquipmentsForStudio(int? studioId)
        {
            //return Equipments.Where(l => l.StudioId == studioId); why isn't it working?
            return _context.Equipments.Where(l => l.StudioId == studioId);
        }

        public bool AddEquipment(Equipment equipment)
        {
            _context.Equipments.Add(equipment);

            try
            {
                _context.SaveChanges();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool RemoveEquipment(int? equipmentId)
        {
            if (equipmentId == null)
                return false;

            var equipment = _context.Equipments.FirstOrDefault(l => l.Id == equipmentId);

            if (equipment == null)
                return false;

            _context.Equipments.Remove(equipment);

            try
            {
                _context.SaveChanges();
            }
            catch
            {

                return false;
            }

            return true;
        }

        public bool UpdateEquipment(Equipment equipment)
        {
            _context.Equipments.Update(equipment);

            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        #endregion

        #region Operations with ReservationEquipmentPairs

        public ReservationEquipmentPair GetReservationEquipmentPair(int? pairId)
        {
            if (pairId == null)
                return null;

            return ReservationEquipmentPairs.FirstOrDefault(l => l.Id == pairId);
        }

        public bool AddReservationEquipmentPair(int roomId, ReservationViewModel viewModel, int userId)
        {
            int studioId = GetStudioByRoomId(roomId).Id;

            List<ReservationEquipmentPair> pairs = new List<ReservationEquipmentPair>();
            foreach (var eq in viewModel.Equipments)
            {
                if (eq.Value == true) //if we want to book this equipment
                {
                    ReservationEquipmentPair pair = new ReservationEquipmentPair
                    {
                        StudioId = studioId,
                        EquipmentId = _context.Equipments.Where(l => l.StudioId == studioId).FirstOrDefault(l => l.Name == eq.Key).Id,
                        EquipmentName = eq.Key,
                        ReservationId = _context.Reservations.FirstOrDefault(l =>
                                            l.RehearsalRoomId == roomId &&
                                            l.UserId == userId &&
                                            l.BandName == viewModel.BandName &&
                                            l.Start == viewModel.Day.AddHours(viewModel.StartHour) &&
                                            l.End == viewModel.Day.AddHours(viewModel.EndHour)).Id
                    };
                    pairs.Add(pair);
                }
            }

            foreach (var pair in pairs)
            {
                _context.ReservationEquipmentPairs.Add(pair);
            }

            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public bool UpdateReservationEquipmentTable(int reservationId, int studioId, Dictionary<string, bool> equipments)
        {
            foreach (var eq in equipments)
            {
                if (eq.Value == true) //ha most kibéreltük az eszközt, de eddig nem volt
                {
                    //ha benne van már az adatbázisban, akkor nem kell csinálni semmit
                    //if (ReservationEquipmentPairs.FirstOrDefault(l => l.ReservationId == reservationId && l.EquipmentName == eq.Key) != null) { }
                    //ha még nincs benne az adatbázisban, akkor hozzá kell adni
                    if (ReservationEquipmentPairs.FirstOrDefault(l => l.ReservationId == reservationId && l.EquipmentName == eq.Key && l.StudioId == studioId) == null)
                    {
                        ReservationEquipmentPair temp = new ReservationEquipmentPair
                        {
                            ReservationId = reservationId,
                            StudioId = studioId,
                            EquipmentId = _context.Equipments.FirstOrDefault(l => l.Name == eq.Key && l.StudioId == studioId).Id,
                            EquipmentName = eq.Key
                        };
                        _context.ReservationEquipmentPairs.Add(temp);
                    }
                }
                else //ha eddig ki volt bérelve az eszköz, de most lemondtuk VAGY eddig SEM volt bérelve
                {
                    //ha eddig sem volt kibérelve, akkor nem kell csinálni semmit
                    //if (ReservationEquipmentPairs.FirstOrDefault(l => l.ReservationId == reservationId && l.EquipmentName == eq.Key && l.StudioId == studioId) == null) { }
                    //ha eddig ki volt bérelve, és lemondtuk
                    if (ReservationEquipmentPairs.FirstOrDefault(l => l.ReservationId == reservationId && l.EquipmentName == eq.Key && l.StudioId == studioId) != null)
                    {
                        ReservationEquipmentPair temp = ReservationEquipmentPairs.FirstOrDefault(l =>
                                                        l.ReservationId == reservationId &&
                                                        l.StudioId == studioId &&
                                                        l.EquipmentName == eq.Key);
                        _context.ReservationEquipmentPairs.Remove(temp);
                    }
                }
            }

            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public IEnumerable<ReservationEquipmentPair> GetReservationEquipmentPairsForStudio(int? studioId)
        {
            if (studioId == null)
                return null;

            return ReservationEquipmentPairs.Where(l => l.StudioId == studioId);
        }

        public IEnumerable<ReservationEquipmentPair> GetReservationEquipmentPairsByEquipment(string equipmentName)
        {
            if (equipmentName == null)
                return null;

            return ReservationEquipmentPairs.Where(l => l.EquipmentName == equipmentName);
        }

        public IEnumerable<ReservationEquipmentPair> GetReservationEquipmentPairsForReservarion(int? reservationId)
        {
            if (reservationId == null)
                return null;

            return ReservationEquipmentPairs.Where(l => l.ReservationId == reservationId);
        }

        public List<string> GetEquipmentNamesForReservation(int? reservationId)
        {
            if (reservationId == null)
                return null;

            return ReservationEquipmentPairs.Where(l => l.ReservationId == reservationId).Select(l => l.EquipmentName).ToList();
        }

        #endregion

        #region Operations with users

        public Byte[] GetUserImage(int? userId)
        {
            if (userId == null)
                return null;

            return _context.Users.FirstOrDefault(l => l.Id == userId).ProfilePicture;
        }

        public bool UpdateProfilePicture(int userId, IFormFile image)
        {
            byte[] fileBytes = null;

            if (image.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    image.CopyTo(ms);
                    fileBytes = ms.ToArray();
                }
            }

            User user = _context.Users.FirstOrDefault(l => l.Id == userId);
            user.ProfilePicture = fileBytes;

            _context.Users.Update(user);

            try
            {
                _context.SaveChanges();
            }
            catch
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}
