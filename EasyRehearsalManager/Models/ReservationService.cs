﻿using EasyRehearsalManager.Model;
using IdentityModel.Client;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EasyRehearsalManager.Web.Models
{
    public class ReservationService : IReservationService
    {
        private readonly EasyRehearsalManagerContext _context;
        private readonly ReservationDateValidator _reservationDateValidator;
        private readonly UserManager<User> _userManager;

        public ReservationService(EasyRehearsalManagerContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
            _reservationDateValidator = new ReservationDateValidator(_context);
        }

        public IEnumerable<RehearsalStudio> Studios => _context.Studios
                                                            .Include(l => l.Rooms)
                                                            .Include(l => l.Equipments)
                                                            .OrderBy(l => l.Name);

        public IEnumerable<RehearsalRoom> Rooms => _context.Rooms
                                                        .Include(l => l.Reservations)
                                                        .Include(l => l.Studio)
                                                        .Include(l => l.Studio.Equipments)
                                                        .OrderBy(l => l.Number);

        public IEnumerable<Reservation> Reservations => _context.Reservations
                                                            .Include(l => l.RehearsalRoom)
                                                            .Include(l => l.RehearsalRoom.Studio)
                                                            //.Include(l => l.Equipments)
                                                            .OrderByDescending(l => l.Start);

        public IEnumerable<Equipment> Equipments => _context.Equipments
                                                        .Include(l => l.Studio);

        public IEnumerable<ReservationEquipmentPair> ReservationEquipmentPairs => _context.ReservationEquipmentPairs;

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

        public RehearsalRoom GetRoom(int? roomId)
        {
            if (roomId == null)
                return null;

            return Rooms.FirstOrDefault(l => l.Id == roomId);
        }

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

        public Equipment GetEquipment(int? equipmentId)
        {
            if (equipmentId == null)
                return null;

            return Equipments.FirstOrDefault(l => l.Id == equipmentId);
        }

        public ReservationEquipmentPair GetReservationEquipmentPair(int? pairId)
        {
            if (pairId == null)
                return null;

            return ReservationEquipmentPairs.FirstOrDefault(l => l.Id == pairId);
        }

        public IEnumerable<Equipment> GetEquipmentsForStudio(int? studioId)
        {
            return Equipments.Where(l => l.StudioId == studioId);
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

        public bool AddReservation(Reservation reservation)
        {
            _context.Reservations.Add(reservation);
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

        public bool RemoveStudio(int? studioId)
        {
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
            foreach (var room in _context.Rooms)
            {
                if (room.StudioId == studioId)
                {
                    foreach (var reservation in _context.Reservations)
                    {
                        if (reservation.RehearsalRoomId == room.Id)
                        {
                            _context.Reservations.Remove(reservation);
                        }
                    }
                    _context.Rooms.Remove(room);
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

        public bool RemoveRoom(int? roomId)
        {
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
            foreach (var reservation in _context.Reservations)
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

        public IEnumerable<RehearsalStudio> GetStudiosByOwner(int? ownerId)
        {
            if (ownerId == null)
                return Studios;

            return Studios.Where(l => l.UserId == ownerId);
        }

        public IEnumerable<RehearsalRoom> GetRoomsByOwnerId(int? ownerId)
        {
            if (ownerId == null)
                return Rooms;

            return Rooms.Where(l => l.Studio.UserId == ownerId);
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

        public int? GetStudioIdByEquipment(int? equipmentId)
        {
            if (equipmentId == null)
                return null;

            return Equipments.FirstOrDefault(l => l.Id == equipmentId).StudioId;
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

        /// <summary>
        /// Creates a new reservation and adds the room by the given ID including the studio
        /// and sets the start date to NOW and the end date to NOW+2hours.
        /// </summary>
        /// <param name="roomId">The ID of the room on which you want to make a reservation.</param>
        /// <returns>The new reservation.</returns>
        public ReservationViewModel NewReservation(int? roomId)
        {
            if (roomId == null)
                return null;

            RehearsalRoom room = Rooms.FirstOrDefault(l => l.Id == roomId);

            ReservationViewModel reservation = new ReservationViewModel { Room = room };

            reservation.Day = DateTime.Today;

            reservation.StartHour = DateTime.Now.Hour + 1;

            reservation.EndHour = DateTime.Now.Hour + 3;

            return reservation;
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

        public bool RehearsalStudioExist(int? studioId)
        {
            if (studioId == null)
                return false;

            var studio = GetStudio(studioId);

            if (studio == null)
                return false;

            return true;
        }

        public async Task<bool> SaveReservationAsync(int? roomId, string userName, ReservationViewModel reservation) //ez csak a foglalás létrehozásánál hívódik meg
        {
            /*
             * ide hozzá kell majd írni hogy a reservationEqipmentPairs táblába bekerüljön a megfelelő elem
             */
            if (roomId == null || reservation == null)
                return false;

            if (!Validator.TryValidateObject(reservation, new ValidationContext(reservation, null, null), null))
                return false;

            DateTime start = reservation.Day.AddHours(reservation.StartHour);
            DateTime end = reservation.Day.AddHours(reservation.EndHour);

            if (_reservationDateValidator.Validate(start, end, roomId.Value, "create", reservation.Id, reservation.Equipments) != ReservationDateError.None)
                return false;

            User user = await _userManager.FindByNameAsync(userName);

            if (user == null)
                return false;
            /*
            List<Equipment> equipments = new List<Equipment>();

            foreach (var equipment in reservation.Equipments)
            {
                if (equipment.Value)
                {
                    Equipment e = _context.Equipments.FirstOrDefault(l => l.Name == equipment.Key);
                    equipments.Add(e);
                }
            }
            */
            _context.Reservations.Add(new Reservation
            {
                RehearsalRoomId = reservation.Room.Id,
                UserId = user.Id,
                Start = reservation.Day.AddHours(reservation.StartHour),
                End = reservation.Day.AddHours(reservation.EndHour),
                BandName = reservation.BandName
                //Equipments = equipments
                //remove equipments
            });

            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }

            int studioId = GetStudioByRoomId(reservation.Room.Id).Id;

            List<ReservationEquipmentPair> pairs = new List<ReservationEquipmentPair>();
            foreach (var eq in reservation.Equipments)
            {
                if (eq.Value == true) //ha ezt kibéreltük
                {
                    ReservationEquipmentPair pair = new ReservationEquipmentPair
                    {
                        StudioId = studioId,
                        EquipmentId = _context.Equipments.Where(l => l.StudioId == studioId).FirstOrDefault(l => l.Name == eq.Key).Id,
                        EquipmentName = eq.Key,
                        ReservationId = _context.Reservations.FirstOrDefault(l => 
                                            l.RehearsalRoomId == reservation.Room.Id && 
                                            l.UserId == user.Id && 
                                            l.BandName == reservation.BandName && 
                                            l.Start == reservation.Day.AddHours(reservation.StartHour) &&
                                            l.End == reservation.Day.AddHours(reservation.EndHour)).Id //somehow we have to find the last(?) added reservation
                    };
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

        //roomId cannot be null, because it is checked before this function is called
        public ReservationDateError ValidateReservation(DateTime start, DateTime end, string action, int reservationId, Dictionary<string, bool> equipments, int roomId)
        {
            return _reservationDateValidator.Validate(start, end, roomId, action, reservationId, equipments);
        }

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

        public List<string> GetEquipmentNamesForReservation(int? reservationId)
        {
            if (reservationId == null)
                return null;

            return ReservationEquipmentPairs.Where(l => l.ReservationId == reservationId).Select(l => l.EquipmentName).ToList();
        }

        public Byte[] GetUserImage(int? userId)
        {
            if (userId == null)
                return null;

            return _context.UserImages
                .Where(l => l.UserId == userId)
                .Select(l => l.ImageSmall)
                .FirstOrDefault();
        }

        public bool NewRoomAdded(int? studioId)
        {
            var studio = _context.Studios.FirstOrDefault(l => l.Id == studioId);
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
    }
}
